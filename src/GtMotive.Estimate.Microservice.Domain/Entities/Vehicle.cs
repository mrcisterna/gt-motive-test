using System;

namespace GtMotive.Estimate.Microservice.Domain.Entities
{
    /// <summary>
    /// Vehicle entity.
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="id">Vehicle identifier.</param>
        /// <param name="brand">Vehicle brand.</param>
        /// <param name="model">Vehicle model.</param>
        /// <param name="manufacturingDate">Manufacturing date.</param>
        public Vehicle(string id, string brand, string model, DateTime manufacturingDate)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new DomainException("Vehicle ID cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(brand))
            {
                throw new DomainException("Vehicle brand cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new DomainException("Vehicle model cannot be empty.");
            }

            ValidateManufacturingDate(manufacturingDate);

            Id = id;
            Brand = brand;
            Model = model;
            ManufacturingDate = manufacturingDate;
            Status = VehicleStatus.Available;
        }

        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the manufacturing date.
        /// </summary>
        public DateTime ManufacturingDate { get; set; }

        /// <summary>
        /// Gets or sets the vehicle status.
        /// </summary>
        public VehicleStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the current renter ID (if available).
        /// </summary>
        public string CurrentRenterId { get; set; }

        /// <summary>
        /// Marks the vehicle as rented.
        /// </summary>
        /// <param name="renterId">The renter identifier.</param>
        public void MarkAsRented(string renterId)
        {
            if (Status != VehicleStatus.Available)
            {
                throw new DomainException("Vehicle is not available for rental.");
            }

            if (string.IsNullOrWhiteSpace(renterId))
            {
                throw new DomainException("Renter ID cannot be empty.");
            }

            Status = VehicleStatus.Rented;
            CurrentRenterId = renterId;
        }

        /// <summary>
        /// Marks the vehicle as available.
        /// </summary>
        public void MarkAsAvailable()
        {
            Status = VehicleStatus.Available;
            CurrentRenterId = null;
        }

        /// <summary>
        /// Validates that the vehicle is not older than 5 years.
        /// </summary>
        /// <param name="manufacturingDate">The manufacturing date to validate.</param>
        /// <exception cref="DomainException">Thrown when the vehicle is older than 5 years.</exception>
        private static void ValidateManufacturingDate(DateTime manufacturingDate)
        {
            var maxAge = DateTime.UtcNow.AddYears(-5);
            if (manufacturingDate < maxAge)
            {
                throw new DomainException("Vehicle cannot be older than 5 years.");
            }
        }
    }
}
