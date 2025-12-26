FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0-noble-amd64 AS build
WORKDIR /src

# Copy config files
COPY ["NuGet.config", "."]
COPY ["Directory.Build.targets", "."]
COPY ["Directory.Build.props", "."]

# Copy project files
COPY ["src/GtMotive.Estimate.Microservice.Domain/GtMotive.Estimate.Microservice.Domain.csproj", "GtMotive.Estimate.Microservice.Domain/"]
COPY ["src/GtMotive.Estimate.Microservice.ApplicationCore/GtMotive.Estimate.Microservice.ApplicationCore.csproj", "GtMotive.Estimate.Microservice.ApplicationCore/"]
COPY ["src/GtMotive.Estimate.Microservice.Infrastructure/GtMotive.Estimate.Microservice.Infrastructure.csproj", "GtMotive.Estimate.Microservice.Infrastructure/"]
COPY ["src/GtMotive.Estimate.Microservice.Api/GtMotive.Estimate.Microservice.Api.csproj", "GtMotive.Estimate.Microservice.Api/"]
COPY ["src/GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj", "GtMotive.Estimate.Microservice.Host/"]

RUN dotnet restore "GtMotive.Estimate.Microservice.Host/GtMotive.Estimate.Microservice.Host.csproj"

# Copy everything
COPY . .

# Merge src files into root
RUN if [ -d src/GtMotive.Estimate.Microservice.Host ] && [ -f src/GtMotive.Estimate.Microservice.Host/Program.cs ]; then \
    cp -rv src/GtMotive.Estimate.Microservice.Host/* GtMotive.Estimate.Microservice.Host/ && \
    cp -rv src/GtMotive.Estimate.Microservice.Api/* GtMotive.Estimate.Microservice.Api/ && \
    cp -rv src/GtMotive.Estimate.Microservice.ApplicationCore/* GtMotive.Estimate.Microservice.ApplicationCore/ && \
    cp -rv src/GtMotive.Estimate.Microservice.Domain/* GtMotive.Estimate.Microservice.Domain/ && \
    cp -rv src/GtMotive.Estimate.Microservice.Infrastructure/* GtMotive.Estimate.Microservice.Infrastructure/; \
fi

RUN rm -f global.json

WORKDIR "/src/GtMotive.Estimate.Microservice.Host"
RUN dotnet publish "GtMotive.Estimate.Microservice.Host.csproj" -c Release -o /app/publish

FROM base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GtMotive.Estimate.Microservice.Host.dll"]