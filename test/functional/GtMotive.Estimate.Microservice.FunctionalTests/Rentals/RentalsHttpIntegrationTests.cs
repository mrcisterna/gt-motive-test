using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

#nullable enable

namespace GtMotive.Estimate.Microservice.FunctionalTests.Rentals
{
    /// <summary>
    /// HTTP Integration tests for the Rentals API endpoint.
    /// Tests the microservice running in Docker at http://localhost:5000/.
    /// </summary>
    public class RentalsHttpIntegrationTests(HttpClientTestFixture fixture) : HttpIntegrationTestBase(fixture)
    {
        [Fact]
        public async Task RentVehicle_WithValidVehicleId_ReturnsCreatedStatusCode()
        {
            // Arrange
            var vehiclesEndpoint = new Uri("/api/vehicles", UriKind.Relative);
            var createVehicleData = new
            {
                id = Guid.NewGuid().ToString(),
                brand = "BMW",
                model = "X5",
                manufacturingDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            // First, create a vehicle
            var createVehicleJson = JsonSerializer.Serialize(createVehicleData);
            using var createVehicleContent = new StringContent(createVehicleJson, Encoding.UTF8, "application/json");
            var vehicleResponse = await HttpClient.PostAsync(vehiclesEndpoint, createVehicleContent);
            Assert.Equal(HttpStatusCode.Created, vehicleResponse.StatusCode);

            var vehicleContent = await vehicleResponse.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(vehicleContent);

            // Try to get vehicleId from either camelCase or PascalCase
            var vehicleId = GetPropertyValue(document.RootElement, "vehicleId", "VehicleId");

            Assert.NotNull(vehicleId);

            // Now rent the vehicle
            var rentalsEndpoint = new Uri("/api/rentals", UriKind.Relative);
            var rentVehicleData = new
            {
                vehicleId,
                renterId = Guid.NewGuid().ToString()
            };

            var rentJson = JsonSerializer.Serialize(rentVehicleData);
            using var rentContent = new StringContent(rentJson, Encoding.UTF8, "application/json");

            // Act
            var rentalResponse = await HttpClient.PostAsync(rentalsEndpoint, rentContent);

            // Assert
            Assert.Equal(HttpStatusCode.Created, rentalResponse.StatusCode);
            var rentalResponseContent = await rentalResponse.Content.ReadAsStringAsync();
            Assert.NotEmpty(rentalResponseContent);
            Assert.Contains("rentalId", rentalResponseContent, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task RentVehicle_WithInvalidVehicleId_ReturnsErrorResponse()
        {
            // Arrange
            var rentalsEndpoint = new Uri("/api/rentals", UriKind.Relative);
            var rentVehicleData = new
            {
                vehicleId = Guid.NewGuid().ToString(),
                renterId = Guid.NewGuid().ToString()
            };

            var rentJson = JsonSerializer.Serialize(rentVehicleData);
            using var rentContent = new StringContent(rentJson, Encoding.UTF8, "application/json");

            // Act
            var response = await HttpClient.PostAsync(rentalsEndpoint, rentContent);

            // Assert
            // Acceptance: The API should reject the request with any non-success status code
            // or it could return a valid error response (e.g., problem details)
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.True(
                !response.IsSuccessStatusCode || responseContent.Contains("error", StringComparison.OrdinalIgnoreCase),
                $"Expected error response for invalid vehicle ID. Got: {response.StatusCode} - {responseContent}");
        }

        [Fact]
        public async Task ReturnVehicle_WithValidRentalId_ReturnsOkStatusCode()
        {
            // Arrange - First create a vehicle and rent it
            var vehiclesEndpoint = new Uri("/api/vehicles", UriKind.Relative);
            var createVehicleData = new
            {
                id = Guid.NewGuid().ToString(),
                brand = "Audi",
                model = "A4",
                manufacturingDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };

            var createVehicleJson = JsonSerializer.Serialize(createVehicleData);
            using var createVehicleContent = new StringContent(createVehicleJson, Encoding.UTF8, "application/json");
            var vehicleResponse = await HttpClient.PostAsync(vehiclesEndpoint, createVehicleContent);
            Assert.Equal(HttpStatusCode.Created, vehicleResponse.StatusCode);

            var vehicleContent = await vehicleResponse.Content.ReadAsStringAsync();
            using var vehicleDoc = JsonDocument.Parse(vehicleContent);

            // Try to get vehicleId from either camelCase or PascalCase
            var vehicleId = GetPropertyValue(vehicleDoc.RootElement, "vehicleId", "VehicleId");

            Assert.NotNull(vehicleId);

            // Rent the vehicle
            var rentalsEndpoint = new Uri("/api/rentals", UriKind.Relative);
            var rentVehicleData = new
            {
                vehicleId,
                renterId = Guid.NewGuid().ToString()
            };

            var rentJson = JsonSerializer.Serialize(rentVehicleData);
            using var rentContent = new StringContent(rentJson, Encoding.UTF8, "application/json");
            var rentalResponse = await HttpClient.PostAsync(rentalsEndpoint, rentContent);
            Assert.Equal(HttpStatusCode.Created, rentalResponse.StatusCode);

            var rentalContent = await rentalResponse.Content.ReadAsStringAsync();
            using var rentalDoc = JsonDocument.Parse(rentalContent);

            // Try to get rentalId from either camelCase or PascalCase
            var rentalId = GetPropertyValue(rentalDoc.RootElement, "rentalId", "RentalId");

            Assert.NotNull(rentalId);

            // Return the vehicle
            var returnUri = new Uri($"/api/rentals/{rentalId}/return", UriKind.Relative);

            // Act
            using var returnResponseContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var returnResponse = await HttpClient.PutAsync(returnUri, returnResponseContent);

            // Assert
            Assert.True(returnResponse.StatusCode is HttpStatusCode.OK or HttpStatusCode.NoContent);
        }

        private static string? GetPropertyValue(JsonElement element, string camelCaseName, string pascalCaseName)
        {
            if (element.TryGetProperty(camelCaseName, out var camelCaseProperty))
            {
                return camelCaseProperty.GetString();
            }

            return element.TryGetProperty(pascalCaseName, out var pascalCaseProperty)
                ? pascalCaseProperty.GetString()
                : null;
        }
    }
}
