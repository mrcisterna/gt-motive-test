using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Api
{
    /// <summary>
    /// Infrastructure tests for Vehicles REST API endpoint.
    /// </summary>
    public class CreateVehicleEndpointTests : IAsyncLifetime
    {
        private CustomWebApplicationFactory<Program> factory;
        private HttpClient client;

        public async Task InitializeAsync()
        {
            factory = new CustomWebApplicationFactory<Program>();
            client = factory.CreateClient();
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            client?.Dispose();
            factory?.Dispose();
            await Task.CompletedTask;
        }

        [Fact]
        public async Task CreateVehicle_WithValidRequest_ShouldReturnCreated()
        {
            // Arrange
            var request = new
            {
                id = "TEST-VEHICLE-001",
                brand = "Honda",
                model = "Civic",
                manufacturingDate = DateTime.UtcNow.AddYears(-2),
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/vehicles", content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseBody);

            var jsonDocument = JsonDocument.Parse(responseBody);
            Assert.True(jsonDocument.RootElement.TryGetProperty("vehicleId", out var vehicleIdElement));
            Assert.Equal("TEST-VEHICLE-001", vehicleIdElement.GetString());
        }

        [Fact]
        public async Task CreateVehicle_WithNullRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var content = new StringContent(
                JsonSerializer.Serialize((object)null),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/vehicles", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateVehicle_WithOldVehicle_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new
            {
                id = "TEST-VEHICLE-002",
                brand = "Ford",
                model = "Mustang",
                manufacturingDate = DateTime.UtcNow.AddYears(-6),
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await client.PostAsync("/api/vehicles", content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ListAvailableVehicles_WithNoVehicles_ShouldReturnEmptyList()
        {
            // Act
            var response = await client.GetAsync("/api/vehicles/available");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            var vehiclesArray = jsonDocument.RootElement.GetProperty("vehicles");
            Assert.Equal(0, vehiclesArray.GetArrayLength());
        }

        [Fact]
        public async Task ListAvailableVehicles_AfterCreatingVehicle_ShouldReturnVehicle()
        {
            // Arrange - Create a vehicle first
            var createRequest = new
            {
                id = "TEST-VEHICLE-003",
                brand = "Nissan",
                model = "Altima",
                manufacturingDate = DateTime.UtcNow.AddYears(-1),
            };

            var createContent = new StringContent(
                JsonSerializer.Serialize(createRequest),
                Encoding.UTF8,
                "application/json");

            await client.PostAsync("/api/vehicles", createContent);

            // Act
            var response = await client.GetAsync("/api/vehicles/available");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseBody = await response.Content.ReadAsStringAsync();

            var jsonDocument = JsonDocument.Parse(responseBody);
            var vehiclesArray = jsonDocument.RootElement.GetProperty("vehicles");
            Assert.True(vehiclesArray.GetArrayLength() > 0);
            Assert.True(vehiclesArray.GetArrayLength() >= 1);
        }
    }
}
