using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Rentals controller.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RentalsController"/> class.
    /// </remarks>
    /// <param name="mediator">MediatR instance.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        /// <summary>
        /// Rents a vehicle.
        /// </summary>
        /// <param name="request">Rent vehicle request.</param>
        /// <returns>Rental response.</returns>
        [HttpPost]
        public async Task<IActionResult> RentVehicle([FromBody] RentVehicleRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var command = new RentVehicleCommand
            {
                VehicleId = request.VehicleId,
                RenterId = request.RenterId,
            };

            var result = await mediator.Send(command);

            var responseDto = new RentVehicleResponseDto
            {
                RentalId = result.RentalId,
                VehicleId = result.VehicleId,
                RentalDate = result.RentalDate,
            };

            return CreatedAtAction(nameof(RentVehicle), responseDto);
        }

        /// <summary>
        /// Returns a rented vehicle.
        /// </summary>
        /// <param name="rentalId">Rental identifier.</param>
        /// <returns>Return vehicle response.</returns>
        [HttpPut("{rentalId}/return")]
        public async Task<IActionResult> ReturnVehicle(string rentalId)
        {
            var command = new ReturnVehicleCommand
            {
                RentalId = rentalId,
            };

            var result = await mediator.Send(command);

            var responseDto = new ReturnVehicleResponseDto
            {
                RentalId = result.RentalId,
                ReturnDate = result.ReturnDate,
            };

            return Ok(responseDto);
        }
    }
}
