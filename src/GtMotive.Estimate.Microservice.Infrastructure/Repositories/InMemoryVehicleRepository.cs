using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of vehicle repository.
    /// </summary>
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "<pendiente>")]
        private readonly List<Vehicle> vehicles = [];

        /// <summary>
        /// Gets a vehicle by identifier.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle if found, otherwise null.</returns>
        public Task<Vehicle> GetByIdAsync(string id)
        {
            var vehicle = vehicles.FirstOrDefault(v => v.Id == id);
            return Task.FromResult(vehicle);
        }

        /// <summary>
        /// Gets all vehicles.
        /// </summary>
        /// <returns>List of vehicles.</returns>
        public Task<ICollection<Vehicle>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Vehicle>>(vehicles);
        }

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>List of available vehicles.</returns>
        public Task<ICollection<Vehicle>> GetAvailableAsync()
        {
            var available = vehicles
                .Where(v => v.Status == VehicleStatus.Available)
                .ToList();

            return Task.FromResult<ICollection<Vehicle>>(available);
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AddAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            vehicles.Add(vehicle);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an existing vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UpdateAsync(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            var existingVehicle = vehicles.FirstOrDefault(v => v.Id == vehicle.Id) ?? throw new KeyNotFoundException($"No existe un vehículo con el identificador '{vehicle.Id}'.");

            var index = vehicles.IndexOf(existingVehicle);
            vehicles[index] = vehicle;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a vehicle.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteAsync(string id)
        {
            var vehicle = vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                vehicles.Remove(vehicle);
            }

            return Task.CompletedTask;
        }
    }
}
