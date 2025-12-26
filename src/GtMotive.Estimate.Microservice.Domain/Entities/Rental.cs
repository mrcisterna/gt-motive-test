using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Rental entity.
    /// </summary>
    public class Rental
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Rental"/> class.
        /// </summary>
        /// <param name="id">Rental identifier.</param>
        /// <param name="vehicleId">Vehicle identifier.</param>
        /// <param name="renterId">Renter identifier.</param>
        public Rental(string id, string vehicleId, string renterId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new DomainException("Rental ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(vehicleId))
            {
                throw new DomainException("Vehicle ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(renterId))
            {
                throw new DomainException("Renter ID cannot be empty.");
            }

            Id = id;
            VehicleId = vehicleId;
            RenterId = renterId;
            RentalDate = DateTime.UtcNow;
            Status = RentalStatus.Active;
        }

        /// <summary>
        /// Gets or sets the rental identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the renter identifier.
        /// </summary>
        public string RenterId { get; set; }

        /// <summary>
        /// Gets or sets the rental start date.
        /// </summary>
        public DateTime RentalDate { get; set; }

        /// <summary>
        /// Gets or sets the rental return date.
        /// </summary>
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Gets or sets the rental status.
        /// </summary>
        public RentalStatus Status { get; set; }

        /// <summary>
        /// Completes the rental.
        /// </summary>
        public void Complete()
        {
            if (Status != RentalStatus.Active)
            {
                throw new DomainException("Only active rentals can be completed.");
            }

            ReturnDate = DateTime.UtcNow;
            Status = RentalStatus.Completed;
        }
    }
}
