using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    /// <summary>
    /// Rental repository interface.
    /// </summary>
    public interface IRentalRepository
    {
        /// <summary>
        /// Gets a rental by identifier.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <returns>The rental if found, otherwise null.</returns>
        Task<Rental> GetByIdAsync(string id);

        /// <summary>
        /// Gets all rentals.
        /// </summary>
        /// <returns>List of rentals.</returns>
        Task<ICollection<Rental>> GetAllAsync();

        /// <summary>
        /// Gets active rentals for a specific renter.
        /// </summary>
        /// <param name="renterId">Renter identifier.</param>
        /// <returns>List of active rentals.</returns>
        Task<ICollection<Rental>> GetActiveRentalsByRenterAsync(string renterId);

        /// <summary>
        /// Adds a new rental.
        /// </summary>
        /// <param name="rental">The rental to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(Rental rental);

        /// <summary>
        /// Updates an existing rental.
        /// </summary>
        /// <param name="rental">The rental to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateAsync(Rental rental);

        /// <summary>
        /// Deletes a rental.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(string id);
    }
}
