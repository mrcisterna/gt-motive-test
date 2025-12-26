using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    /// <summary>
    /// In-memory implementation of vehicle repository.
    /// </summary>
    public class InMemoryVehicleRepository : IVehicleRepository
    {
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
        private readonly Dictionary<string, Vehicle> vehicles = [];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly

        /// <summary>
        /// Gets a vehicle by identifier.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>The vehicle if found, otherwise null.</returns>
        public Task<Vehicle> GetByIdAsync(string id)
        {
            vehicles.TryGetValue(id, out var vehicle);
            return Task.FromResult(vehicle);
        }

        /// <summary>
        /// Gets all vehicles.
        /// </summary>
        /// <returns>List of vehicles.</returns>
        public Task<ICollection<Vehicle>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Vehicle>>([.. vehicles.Values]);
        }

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>List of available vehicles.</returns>
        public Task<ICollection<Vehicle>> GetAvailableAsync()
        {
            var available = vehicles.Values
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

            vehicles[vehicle.Id] = vehicle;
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

            if (!vehicles.ContainsKey(vehicle.Id))
            {
                throw new KeyNotFoundException($"No existe un vehículo con el identificador '{vehicle.Id}'.");
            }

            vehicles[vehicle.Id] = vehicle;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a vehicle.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteAsync(string id)
        {
            vehicles.Remove(id);
            return Task.CompletedTask;
        }
    }
}
