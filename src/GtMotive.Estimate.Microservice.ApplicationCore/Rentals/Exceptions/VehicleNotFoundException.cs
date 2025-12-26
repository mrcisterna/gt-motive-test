using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Exceptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleNotFoundException"/> class.
    /// </summary>
    public class VehicleNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleNotFoundException"/> class.
        /// </summary>
        public VehicleNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        public VehicleNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleNotFoundException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="innerException">error.</param>
        public VehicleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
