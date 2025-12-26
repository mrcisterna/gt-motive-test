using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Vehicles controller.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="VehiclesController"/> class.
    /// </remarks>
    /// <param name="mediator">MediatR mediator instance.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;

        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="request">Create vehicle request.</param>
        /// <returns>Created vehicle response.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequestDto request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var command = new CreateVehicleCommand
            {
                Id = request.Id,
                Brand = request.Brand,
                Model = request.Model,
                ManufacturingDate = request.ManufacturingDate,
            };

            var result = await mediator.Send(command);

            var responseDto = new CreateVehicleResponseDto
            {
                VehicleId = result.VehicleId,
                Brand = result.Brand,
                Model = result.Model,
            };

            return CreatedAtAction(nameof(CreateVehicle), responseDto);
        }

        /// <summary>
        /// Lists available vehicles.
        /// </summary>
        /// <returns>List of available vehicles.</returns>
        [HttpGet("available")]
        public async Task<IActionResult> ListAvailableVehicles()
        {
            var query = new ListAvailableVehiclesQuery();
            var result = await mediator.Send(query);

            var responseDto = new ListAvailableVehiclesResponseDto(result.Vehicles);
            return Ok(responseDto);
        }
    }
}
