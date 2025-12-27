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
        /// Test: Valid request returns response with application/json content type.
        /// Validates that the API properly sets the Content-Type header.
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
        /// Test: Rental response contains required properties.
        /// Validates that the response includes RentalId, VehicleId, and RentalDate.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        [Fact]
        public async Task RentVehicleWithValidRequestReturnsCompleteResponse()
        {
            // Arrange
            var client = Fixture.Server.CreateClient();

            var createVehicleRequest = new CreateVehicleRequestDto
            {
                Id = "V-RESPONSE-001",
                Brand = "Ford",
                Model = "Focus",
                ManufacturingDate = DateTime.UtcNow.AddYears(-1),
            };
            await client.PostAsJsonAsync("/api/vehicles", createVehicleRequest);

            var request = new RentVehicleRequestDto
            {
                VehicleId = "V-RESPONSE-001",
                RenterId = "RENTER-RESPONSE-001",
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/rentals", request);
            var responseDto = await response.Content.ReadFromJsonAsync<RentVehicleResponseDto>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseDto.Should().NotBeNull();
            responseDto!.RentalId.Should().NotBeEmpty();
            responseDto.VehicleId.Should().Be("V-RESPONSE-001");
            responseDto.RentalDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }
    }
}
