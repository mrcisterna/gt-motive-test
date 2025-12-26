using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Exceptions
{
    /// <summary>
    /// RentalNotFoundException class.
    /// </summary>
    public class RentalNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RentalNotFoundException"/> class.
        /// </summary>
        public RentalNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalNotFoundException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        public RentalNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalNotFoundException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="innerException">error.</param>
        public RentalNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
