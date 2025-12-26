using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Dtos;

namespace GtMotive.Estimate.Microservice.Api.Dtos
{
    /// <summary>
    /// Response DTO for listing available vehicles.
    /// </summary>
    public class ListAvailableVehiclesResponseDto(ICollection<VehicleDto> vehicles)
    {
        /// <summary>
        /// Gets the list of vehicles.
        /// </summary>
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
        public ICollection<VehicleDto> Vehicles { get; } = vehicles;
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly
    }
}
