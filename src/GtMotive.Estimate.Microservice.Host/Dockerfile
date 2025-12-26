#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0-noble-amd64 AS build
ARG PersonalAccessToken
WORKDIR /src

# Set environment variables to avoid build issues
ENV DOTNET_SDK_CHECK=false
ENV MSBUILDTREATWARNINGINGASERROR=false
ENV NUGET_XMLDOC_MODE=skip

# Copy configuration files FIRST (these define package versions)
COPY ["NuGet.config", "."]
COPY ["Directory.Build.targets", "."]
COPY ["Directory.Build.props", "."]

# Copy project files
COPY ["src/GtMotive.Estimate.Microservice.Domain/GtMotive.Estimate.Microservice.Domain.csproj", "GtMotive.Estimate.Microservice.Domain/"]
COPY ["src/GtMotive.Estimate.Microservice.ApplicationCore/GtMotive.Estimate.Microservice.ApplicationCore.csproj", "GtMotive.Estimate.Microservice.ApplicationCore/"]
COPY ["src/GtMotive.Estimate.Microservice.Infrastructure/GtMotive.Estimate.Microservice.Infrastructure.csproj", "GtMotive.Estimate.Microservice.Infrastructure/"]
COPY ["src/GtMotive.Estimate.Microservice.Api/GtMotive.Estimate.Microservice.Api.csproj", "GtMotive.Estimate.Microservice.Api/"]
COPY ["src/GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj", "GtMotive.Estimate.Microservice.Host/"]

# Ensure the Host project has OutputType=Exe by creating a property
RUN printf '\n    <OutputType>Exe</OutputType>\n' > /tmp/output_type.xml && \
    grep -q '<OutputType>' GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj || \
    sed -i '/<TargetFramework>net9.0<\/TargetFramework>/a\    <OutputType>Exe</OutputType>' GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj

# Restore packages
RUN dotnet restore "GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj"

# Copy source code
COPY . .

# Remove global.json to avoid SDK version conflicts
RUN rm -f global.json

WORKDIR "/src/GtMotive.Estimate.Microservice.Host"

# Publish application with OutputType specified
RUN dotnet publish "GtMotive.Estimate.Microservice.Host.csproj" \
    -c Release \
    -o /app/publish \
    /p:TreatWarningsAsErrors=false \
    /p:DebugType=none \
    /p:DebugSymbols=false \
    /p:OutputType=Exe

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
HEALTHCHECK --interval=30s --timeout=3s --start-period=10s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1
ENTRYPOINT ["dotnet", "GtMotive.Estimate.Microservice.Host.dll"]