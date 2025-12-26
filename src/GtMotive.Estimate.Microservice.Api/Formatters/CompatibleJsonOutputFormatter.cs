using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace GtMotive.Estimate.Microservice.Api.Formatters
{
    /// <summary>
    /// Custom JSON output formatter that works around the PipeWriter.UnflushedBytes issue in .NET 9.
    /// This formatter uses a memory stream approach instead of PipeWriter to avoid serialization issues
    /// in TestServer environments.
    /// See: https://github.com/dotnet/runtime/issues/98641.
    /// </summary>
    public class CompatibleJsonOutputFormatter : TextOutputFormatter
    {
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public CompatibleJsonOutputFormatter(JsonSerializerOptions jsonSerializerOptions)
        {
            this.jsonSerializerOptions = jsonSerializerOptions ?? new JsonSerializerOptions();

            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(selectedEncoding);

            var httpContext = context.HttpContext;

            // Use memory stream approach to avoid PipeWriter issues
            await using var memoryStream = new System.IO.MemoryStream();

            // Serialize to memory stream
            await JsonSerializer.SerializeAsync(memoryStream, context.Object, context.ObjectType ?? typeof(object), jsonSerializerOptions);

            // Write to response
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(httpContext.Response.Body);
        }
    }
}
