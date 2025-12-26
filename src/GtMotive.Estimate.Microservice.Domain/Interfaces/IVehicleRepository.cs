using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Vehicle repository interface.
    /// </summary>
    public interface IVehicleRepository
    {
        /// <summary>
        /// Gets a vehicle by identifier.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle if found, otherwise null.</returns>
        Task<Vehicle> GetByIdAsync(string id);

        /// <summary>
        /// Gets all vehicles.
        /// </summary>
        /// <returns>List of vehicles.</returns>
        Task<ICollection<Vehicle>> GetAllAsync();

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>List of available vehicles.</returns>
        Task<ICollection<Vehicle>> GetAvailableAsync();

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Vehicle vehicle);

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Vehicle vehicle);

        /// <summary>
        /// Deletes a vehicle.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string id);
    }
}
