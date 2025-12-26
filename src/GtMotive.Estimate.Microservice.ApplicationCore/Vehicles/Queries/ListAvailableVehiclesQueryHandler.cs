using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Dtos;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Handler for ListAvailableVehiclesQuery.
    /// </summary>
    public class ListAvailableVehiclesQueryHandler(IVehicleRepository vehicleRepository) : IRequestHandler<ListAvailableVehiclesQuery, ListAvailableVehiclesQueryResponse>
    {
        private readonly IVehicleRepository _vehicleRepository = vehicleRepository;

        /// <summary>
        /// Handles the ListAvailableVehiclesQuery.
        /// </summary>
        /// <param name="request">List available vehicles query.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>List available vehicles query response.</returns>
        public async Task<ListAvailableVehiclesQueryResponse> Handle(
            ListAvailableVehiclesQuery request,
            CancellationToken cancellationToken)
        {
            var availableVehicles = await _vehicleRepository.GetAvailableAsync().ConfigureAwait(false);

            var vehicles = availableVehicles
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    Brand = v.Brand,
                    Model = v.Model,
                    ManufacturingDate = v.ManufacturingDate,
                })
                .ToList();

            return new ListAvailableVehiclesQueryResponse(vehicles);
        }
    }
}
