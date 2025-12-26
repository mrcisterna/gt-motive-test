# HTTP Integration Tests

This folder contains HTTP integration tests for the GtMotive Estimate Microservice. These tests run against the actual Docker instance of the microservice.

## Overview

The integration tests in this project are designed to:

- **Test the actual running microservice** in Docker at `http://localhost:5000/`
- **Exclude the Host project** from the test composition
- **Test HTTP endpoints** directly rather than unit testing individual components
- **Verify end-to-end workflows** like creating vehicles and renting them

## Test Structure

### Infrastructure

- **`HttpClientTestFixture.cs`** - Manages the HTTP client and ensures the service is ready before tests run
- **`HttpIntegrationTestBase.cs`** - Base class for all HTTP integration tests
- **`HttpIntegrationCollection.cs`** - XUnit collection definition for test isolation

### Test Suites

#### Vehicles Tests (`VehiclesHttpIntegrationTests.cs`)

Tests for the Vehicles API endpoints:

- `ListAvailableVehicles_WhenCalled_ReturnsOkStatusCode` - Verifies the list endpoint works
- `ListAvailableVehicles_WhenCalled_ReturnsJsonContentType` - Verifies response format
- `CreateVehicle_WithValidData_ReturnsCreatedStatusCode` - Tests vehicle creation with valid data
- `CreateVehicle_WithMissingData_ReturnsBadRequest` - Tests validation of required fields

#### Rentals Tests (`RentalsHttpIntegrationTests.cs`)

Tests for the Rentals API endpoints:

- `RentVehicle_WithValidVehicleId_ReturnsCreatedStatusCode` - Tests creating a rental for an existing vehicle
- `RentVehicle_WithInvalidVehicleId_ReturnsBadRequest` - Tests validation of vehicle existence
- `ReturnVehicle_WithValidRentalId_ReturnsOkStatusCode` - Tests completing a rental

## Running the Tests

### Prerequisites

1. Ensure Docker is running
2. Start the microservice:
   ```bash
   cd src
   docker compose up -d
   ```

3. Verify the service is running:
   ```bash
   curl http://localhost:5000/health
   ```

### Running All Integration Tests

```bash
dotnet test ../test/functional/GtMotive.Estimate.Microservice.FunctionalTests --filter "Category=HttpIntegration"
```

Or from the test project directory:

```bash
dotnet test --filter "Category=HttpIntegration"
```

### Running Specific Test Suite

Vehicles tests only:
```bash
dotnet test --filter "FullyQualifiedName~VehiclesHttpIntegrationTests"
```

Rentals tests only:
```bash
dotnet test --filter "FullyQualifiedName~RentalsHttpIntegrationTests"
```

### Running with Verbose Output

```bash
dotnet test --verbosity detailed
```

## Test Workflow

The typical integration test workflow:

1. **HttpClientTestFixture** initializes and waits for the service to be ready
2. Each test uses the shared `HttpClient` to make requests to the service
3. Tests verify:
   - HTTP status codes
   - Response content type
   - Response data structure
   - Error handling
4. Tests are isolated and can run in any order

## Service Readiness

The `HttpClientTestFixture` automatically:

- Attempts to connect to `http://localhost:5000` 
- Polls the service with up to 5 retry attempts
- Waits 1 second between retries
- Throws an informative error if the service doesn't become ready

## Example Test Structure

```csharp
public class VehiclesHttpIntegrationTests : HttpIntegrationTestBase
{
    public VehiclesHttpIntegrationTests(HttpClientTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task CreateVehicle_WithValidData_ReturnsCreatedStatusCode()
    {
        // Arrange
        var endpoint = "/api/vehicles";
        var vehicleData = new { /* ... */ };
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await HttpClient.PostAsync(endpoint, content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
```

## Troubleshooting

### "Service did not become ready" Error

If you see this error, ensure:

1. Docker is running: `docker ps`
2. The microservice container is started: `docker compose up -d`
3. The service has enough time to initialize (may take 10-30 seconds)

### Connection Refused

- Verify the service is listening on port 5000: `netstat -an | findstr 5000`
- Check Docker logs: `docker compose logs microservice`

### Test Failures

Check the actual HTTP response:

```csharp
var responseContent = await response.Content.ReadAsStringAsync();
_output.WriteLine(responseContent);
```

## Best Practices

1. **Use meaningful test names** - Follow the pattern `Method_Scenario_ExpectedResult`
2. **Keep tests independent** - Each test should create its own data
3. **Use fixtures for setup** - Share expensive operations with XUnit fixtures
4. **Verify both success and failure cases** - Test happy path and error conditions
5. **Check response formats** - Verify JSON structure and field names

## Adding New Tests

When adding new integration tests:

1. Create a new test class inheriting from `HttpIntegrationTestBase`
2. Ensure the class is in the correct namespace
3. Use the `[Fact]` attribute for individual tests
4. Follow the AAA pattern: Arrange, Act, Assert
5. Use descriptive test method names

Example:

```csharp
public class MyFeatureHttpIntegrationTests : HttpIntegrationTestBase
{
    public MyFeatureHttpIntegrationTests(HttpClientTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task MyEndpoint_WithValidInput_ReturnsExpectedResult()
    {
        // Arrange
        var endpoint = "/api/myfeature";
        // ...

        // Act
        var response = await HttpClient.GetAsync(endpoint);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
```

## Related Documentation

- [Docker Setup](../../DOCKER_READINESS_REPORT.md)
- [Running the Service](../../DOCKER_QUICK_START.md)
- [Project Structure](../../PROJECT_STATUS.md)
