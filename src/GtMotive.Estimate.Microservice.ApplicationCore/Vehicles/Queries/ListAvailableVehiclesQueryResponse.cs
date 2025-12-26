using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Dtos;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Response from ListAvailableVehiclesQuery.
    /// </summary>
    public class ListAvailableVehiclesQueryResponse(ICollection<VehicleDto> vehicles)
    {
        /// <summary>
        /// Gets the collection of available vehicles.
        /// </summary>
        public ICollection<VehicleDto> Vehicles { get; } = vehicles;
    }
}
