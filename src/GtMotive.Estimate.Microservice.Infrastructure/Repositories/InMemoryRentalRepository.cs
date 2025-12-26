using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    /// <summary>
    /// In-memory implementation of rental repository.
    /// </summary>
    public class InMemoryRentalRepository : IRentalRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1010:Opening square brackets should be spaced correctly", Justification = "<pendiente>")]
        private readonly List<Rental> rentals = [];

        /// <summary>
        /// Gets a rental by identifier.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <returns>The rental if found, otherwise null.</returns>
        public Task<Rental> GetByIdAsync(string id)
        {
            var rental = rentals.FirstOrDefault(r => r.Id == id);
            return Task.FromResult(rental);
        }

        /// <summary>
        /// Gets all rentals.
        /// </summary>
        /// <returns>List of rentals.</returns>
        public Task<ICollection<Rental>> GetAllAsync()
        {
            return Task.FromResult<ICollection<Rental>>(rentals);
        }

        /// <summary>
        /// Gets active rentals for a specific renter.
        /// </summary>
        /// <param name="renterId">Renter identifier.</param>
        /// <returns>List of active rentals.</returns>
        public Task<ICollection<Rental>> GetActiveRentalsByRenterAsync(string renterId)
        {
            var active = rentals
                .Where(r => r.RenterId == renterId && r.Status == RentalStatus.Active)
                .ToList();

            return Task.FromResult<ICollection<Rental>>(active);
        }

        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="rental">The rental to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AddAsync(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            rentals.Add(rental);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates an existing rental.
        /// </summary>
        /// <param name="rental">The rental to update.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UpdateAsync(Rental rental)
        {
            ArgumentNullException.ThrowIfNull(rental);

            var existingRental = rentals.FirstOrDefault(r => r.Id == rental.Id) ?? throw new KeyNotFoundException($"No rental found with Id '{rental.Id}' to update.");

            var index = rentals.IndexOf(existingRental);
            rentals[index] = rental;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a rental.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteAsync(string id)
        {
            var rental = rentals.FirstOrDefault(r => r.Id == id);
            if (rental != null)
            {
                rentals.Remove(rental);
            }

            return Task.CompletedTask;
        }
    }
}
