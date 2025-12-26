using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    public static class InfrastructureConfigurationHelpers
    {
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            // Register repositories as singletons to maintain state across requests
            services.AddSingleton<IVehicleRepository, InMemoryVehicleRepository>();
            services.AddSingleton<IRentalRepository, InMemoryRentalRepository>();

            // Register Unit of Work
            services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();

            return services;
        }
    }
}
