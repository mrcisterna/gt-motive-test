using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.Host.Configuration;
using GtMotive.Estimate.Microservice.Host.DependencyInjection;
using GtMotive.Estimate.Microservice.Infrastructure;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using IdentityServer4.AccessTokenValidation;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder();

// Configuración.
if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("serilogsettings.json", optional: true, reloadOnChange: true);
    AddKeyVaultConfiguration(builder);
}

// Configuración de logging para el arranque del host.
builder.Logging.ClearProviders();

Log.Logger = CreateBootstrapLogger();

builder.Host.UseSerilog();

// Agregar servicios al contenedor.
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);
    builder.Services.AddApplicationInsightsKubernetesEnricher();
}

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GtMotive.Estimate.Microservice.Api.Filters.BusinessExceptionFilter>();

    // Solución para el problema de .NET 9 PipeWriter.UnflushedBytes - eliminar SystemTextJsonOutputFormatter y agregar CompatibleJsonOutputFormatter
    var systemTextJsonFormatter = options.OutputFormatters
        .OfType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>()
        .FirstOrDefault();

    if (systemTextJsonFormatter != null)
    {
        options.OutputFormatters.Remove(systemTextJsonFormatter);
        var jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        };
        options.OutputFormatters.Add(new GtMotive.Estimate.Microservice.Api.Formatters.CompatibleJsonOutputFormatter(jsonOptions));
    }
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>() ?? new AppSettings
{
    JwtAuthority = "http://localhost:5000"
};

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddControllers()
    .WithApiControllers();

builder.Services.AddBaseInfrastructure(builder.Environment.IsDevelopment());

// Register repositories for dependency injection
builder.Services.AddInMemoryRepositories();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                               ForwardedHeaders.XForwardedProto;

    // Solo proxies de loopback permitidos por defecto.
    // Limpiar esa restricción porque los forwarders están habilitados por configuración explícita.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

if (!string.IsNullOrEmpty(appSettings?.JwtAuthority))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    })
    .AddIdentityServerAuthentication(options =>
    {
        options.Authority = appSettings.JwtAuthority;
        options.ApiName = "estimate-api";
        options.SupportedTokens = SupportedTokens.Jwt;
    });
}

builder.Services.AddSwagger(appSettings, builder.Configuration);

var app = builder.Build();

// Configuración de logging.
Log.Logger = CreateRuntimeLogger(app, builder);

var pathBase = new PathBase(builder.Configuration.GetValue("PathBase", defaultValue: PathBase.DefaultPathBase));

if (!pathBase.IsDefault)
{
    app.UsePathBase(pathBase.CurrentWithoutTrailingSlash);
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerInApplication(pathBase, builder.Configuration);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

static void AddKeyVaultConfiguration(WebApplicationBuilder builder)
{
    var keyVaultName = builder.Configuration.GetValue<string>("KeyVaultName");
    if (!string.IsNullOrEmpty(keyVaultName))
    {
        try
        {
            var secretClient = new SecretClient(
                new System.Uri($"https://{keyVaultName}.vault.azure.net/"),
                new DefaultAzureCredential());

            builder.Configuration.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
        catch (AuthenticationFailedException)
        {
            // KeyVault no disponible
        }
        catch (Azure.RequestFailedException)
        {
            // KeyVault no disponible
        }
    }
}

static Serilog.ILogger CreateBootstrapLogger()
{
    return new LoggerConfiguration()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            formatProvider: CultureInfo.InvariantCulture)
        .CreateBootstrapLogger();
}

static Serilog.ILogger CreateRuntimeLogger(WebApplication app, WebApplicationBuilder builder)
{
    return app.Environment.IsDevelopment() ?
        new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                theme: AnsiConsoleTheme.Literate,
                formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger() :
        new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "addoperation")
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.ApplicationInsights(
                app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
}
