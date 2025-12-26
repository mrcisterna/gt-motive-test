namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Response from CreateVehicleCommand.
    /// </summary>
    public class CreateVehicleCommandResponse
    {
        /// <summary>
        /// Gets or sets the vehicle identifier.
        /// </summary>
        public string VehicleId { get; set; }

        /// <summary>
        /// Gets or sets the vehicle brand.
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the vehicle model.
        /// </summary>
        public string Model { get; set; }
    }
}
