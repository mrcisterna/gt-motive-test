using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Vehicles
{
    /// <summary>
    /// HTTP Integration tests for the Vehicles API endpoint.
    /// Tests the microservice running in Docker at http://localhost:5000/.
    /// </summary>
    public class VehiclesHttpIntegrationTests(HttpClientTestFixture fixture) : HttpIntegrationTestBase(fixture)
    {
        [Fact]
        public async Task ListAvailableVehicles_WhenCalled_ReturnsOkStatusCode()
        {
            // Arrange
            var endpoint = new Uri("/api/vehicles/available", UriKind.Relative);

            // Act
            var response = await HttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);
        }

        [Fact]
        public async Task ListAvailableVehicles_WhenCalled_ReturnsJsonContentType()
        {
            // Arrange
            var endpoint = new Uri("/api/vehicles/available", UriKind.Relative);

            // Act
            var response = await HttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Contains(
                "application/json",
                response.Content.Headers.ContentType.MediaType,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateVehicle_WithValidData_ReturnsCreatedStatusCode()
        {
            // Arrange
            var endpoint = new Uri("/api/vehicles", UriKind.Relative);
            var vehicleData = new
            {
                id = Guid.NewGuid().ToString(),
                brand = "Tesla",
                model = "Model 3",
                manufacturingDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            var json = JsonSerializer.Serialize(vehicleData);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PostAsync(endpoint, content);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(responseContent);
            Assert.Contains(
                "vehicleId",
                responseContent,
                StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task CreateVehicle_WithMissingData_ReturnsBadRequest()
        {
            // Arrange
            var endpoint = new Uri("/api/vehicles", UriKind.Relative);
            var invalidVehicleData = new
            {
                brand = "Tesla"
            };

            var json = JsonSerializer.Serialize(invalidVehicleData);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PostAsync(endpoint, content);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
