using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure;

namespace GtMotive.Estimate.Microservice.InfrastructureTests
{
    /// <summary>
    /// Custom web application factory for integration testing.
    /// </summary>
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing repository registrations
                var vehicleRepoDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(IVehicleRepository));
                if (vehicleRepoDescriptor != null)
                {
                    services.Remove(vehicleRepoDescriptor);
                }

                var rentalRepoDescriptor = services.FirstOrDefault(d =>
                    d.ServiceType == typeof(IRentalRepository));
                if (rentalRepoDescriptor != null)
                {
                    services.Remove(rentalRepoDescriptor);
                }

                // Re-register repositories for testing (fresh in-memory instances)
                services.AddInMemoryRepositories();
            });
        }
    }
}
