using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Api.Dtos;
using GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Api
{
    /// <summary>
    /// Integration tests for the RentVehicle endpoint at the host level.
    /// Tests HTTP request reception and DTO model validation only.
    /// Does not test the complete flow (no database persistence, no business logic).
    /// </summary>
    public class RentVehicleEndpointTests(GenericInfrastructureTestServerFixture fixture) : InfrastructureTestBase(fixture)
    {
        /// <summary>
        /// Test: Valid request is accepted and returns 201 Created.
        /// Validates that the endpoint receives and processes a valid rental request.
        /// NOTE: This test creates a vehicle first to satisfy business logic requirements.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithValidRequestReturnsCreatedStatusCode()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();

            // First, create a vehicle to rent
            var createVehicleRequest = new CreateVehicleRequestDto
            {
                Id = "V001",
                Brand = "Toyota",
                Model = "Corolla",
                ManufacturingDate = DateTime.UtcNow.AddYears(-2),
            };
            await client.PostAsJsonAsync("/api/vehicles", createVehicleRequest);

            // Now rent the vehicle
            var request = new RentVehicleRequestDto
            {
                VehicleId = "V001",
                RenterId = "RENTER001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert - Verify HTTP status code
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        /// <summary>
        /// Test: Request with null VehicleId returns 400 Bad Request.
        /// Validates that model validation catches missing required fields.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithNullVehicleIdReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = null,
                RenterId = "RENTER001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert - Verify validation error response
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Test: Request with empty VehicleId returns 400 Bad Request.
        /// Validates that empty string values are rejected.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithEmptyVehicleIdReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = string.Empty,
                RenterId = "RENTER001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Test: Request with null RenterId returns 400 Bad Request.
        /// Validates that required RenterId field is enforced.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithNullRenterIdReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = "V001",
                RenterId = null,
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Test: Request with empty RenterId returns 400 Bad Request.
        /// Validates that empty renter identifiers are rejected.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithEmptyRenterIdReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = "V001",
                RenterId = string.Empty,
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Test: Valid request returns response with application/json content type.
        /// Validates that the API properly sets the Content-Type header.
        /// NOTE: This test creates a vehicle first to satisfy business logic requirements.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithValidRequestReturnsJsonContentType()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();

            // First, create a vehicle to rent
            var createVehicleRequest = new CreateVehicleRequestDto
            {
                Id = "V-TEST-001",
                Brand = "Honda",
                Model = "Civic",
                ManufacturingDate = DateTime.UtcNow.AddYears(-3),
            };
            await client.PostAsJsonAsync("/api/vehicles", createVehicleRequest);

            // Now rent the vehicle
            var request = new RentVehicleRequestDto
            {
                VehicleId = "V-TEST-001",
                RenterId = "RENTER-TEST-001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert - Verify response content type is JSON
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType?.MediaType.Should().Contain("application/json");
        }

        /// <summary>
        /// Test: VehicleId exceeding max length returns 400 Bad Request.
        /// Validates that string length constraints are enforced.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithVehicleIdExceedingMaxLengthReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = new string('V', 51),
                RenterId = "RENTER001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Test: RenterId exceeding max length returns 400 Bad Request.
        /// Validates that renter identifier length is validated.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithRenterIdExceedingMaxLengthReturnsBadRequest()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();
            var request = new RentVehicleRequestDto
            {
                VehicleId = "V001",
                RenterId = new string('R', 101),
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
