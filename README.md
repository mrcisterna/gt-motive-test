# Vehicle Rental Microservice

A complete .NET 9 microservice for managing vehicle rentals with hexagonal architecture, MediatR patterns, and comprehensive automated testing.

## ?? Overview

This microservice implements a vehicle rental management system with the following features:

- ? Create and manage vehicles in a fleet
- ? List available vehicles for rental
- ? Rent vehicles to customers
- ? Return rented vehicles
- ? Business rule enforcement (vehicles max 5 years old, 1 rental per vehicle)
- ? 45 automated tests (35 unit, 3 infrastructure, 7 functional)
- ? Hexagonal architecture with clean separation of concerns
- ? No external dependencies (runs with Docker or .NET 9)

## ?? Quick Start

### Option 1: Docker Compose (Recommended)

**Requirements:** Docker and Docker Compose

```bash
# Clone the repository
git clone https://github.com/mrcisterna/gt-motive-test.git
cd gt-motive-test/src

# Start the microservice
docker-compose up

# Microservice will be available at http://localhost:5000
```

**In another terminal, run tests:**
```bash
cd gt-motive-test/src
docker-compose run --rm tests
```

### Option 2: .NET 9 Local Development

**Requirements:** .NET 9 SDK

```bash
# Clone the repository
git clone https://github.com/mrcisterna/gt-motive-test.git
cd gt-motive-test/src

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run the microservice
dotnet run --project GtMotive.Estimate.Microservice.Host

# Microservice will be available at http://localhost:5000
```

**In another terminal, run tests:**
```bash
cd gt-motive-test/src
dotnet test microservice.sln
```

## ?? Running Tests

### All Tests
```bash
cd src
dotnet test microservice.sln
```

**Expected output:**
```
? Unit Tests:           35/35 PASSED
? Infrastructure Tests: 3/3 PASSED
? Functional Tests:     7/7 PASSED

Total: 45/45 PASSED
```

### Run Specific Test Project
```bash
# Unit tests only
dotnet test test/unit/GtMotive.Estimate.Microservice.UnitTests/GtMotive.Estimate.Microservice.UnitTests.csproj

# Infrastructure tests only
dotnet test test/infrastructure/GtMotive.Estimate.Microservice.InfrastructureTests/GtMotive.Estimate.Microservice.InfrastructureTests.csproj

# Functional tests only
dotnet test test/functional/GtMotive.Estimate.Microservice.FunctionalTests/GtMotive.Estimate.Microservice.FunctionalTests.csproj
```

### Run Tests with Coverage
```bash
dotnet test microservice.sln --collect:"XPlat Code Coverage"
```

## ?? API Endpoints

### Create Vehicle
```http
POST /api/vehicles
Content-Type: application/json

{
  "id": "V001",
  "brand": "BMW",
  "model": "X5",
  "manufacturingDate": "2024-01-01T00:00:00Z"
}

Response: 201 Created
{
  "vehicleId": "V001",
  "brand": "BMW",
  "model": "X5"
}
```

### List Available Vehicles
```http
GET /api/vehicles/available

Response: 200 OK
{
  "vehicles": [
    {
      "id": "V001",
      "brand": "BMW",
      "model": "X5",
      "manufacturingDate": "2024-01-01T00:00:00Z"
    }
  ]
}
```

### Rent Vehicle
```http
POST /api/rentals
Content-Type: application/json

{
  "vehicleId": "V001",
  "renterId": "CUSTOMER-001"
}

Response: 201 Created
{
  "rentalId": "R-xyz123",
  "vehicleId": "V001",
  "renterId": "CUSTOMER-001"
}
```

### Return Vehicle
```http
PUT /api/rentals/{rentalId}/return
Content-Type: application/json

Response: 200 OK / 204 No Content
```

## ?? Project Structure

```
src/
??? GtMotive.Estimate.Microservice.Domain/
?   ??? Entities/          (Vehicle, Rental - business logic)
?   ??? Interfaces/        (Repository interfaces)
?   ??? DomainException.cs (Business rule violations)
?
??? GtMotive.Estimate.Microservice.ApplicationCore/
?   ??? Vehicles/          (Create, List commands/queries)
?   ??? Rentals/           (Rent, Return commands)
?   ??? Common/            (Validation behavior)
?
??? GtMotive.Estimate.Microservice.Infrastructure/
?   ??? Repositories/      (In-memory implementations)
?   ??? UnitOfWork/
?   ??? Logging/
?
??? GtMotive.Estimate.Microservice.Api/
?   ??? Controllers/       (HTTP endpoints)
?   ??? Dtos/             (Data transfer objects)
?   ??? Filters/          (Exception handling)
?
??? GtMotive.Estimate.Microservice.Host/
    ??? Program.cs        (DI container setup)
    ??? appsettings.json  (Configuration)

test/
??? unit/                 (35 unit tests)
??? infrastructure/       (3 HTTP endpoint tests)
??? functional/          (7 integration tests)
```

## ?? Configuration

### Environment Variables (Optional)

Create a `.env` file in the `src` directory:

```env
DOTNET_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:5000
```

### Modify Port

Edit `src/GtMotive.Estimate.Microservice.Host/appsettings.json`:

```json
{
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5001"
      }
    }
  }
}
```

## ?? Business Rules

### Rule 1: Maximum 5 Years Old
Vehicles must be less than 5 years old from the current date.

**Location:** `Domain/Entities/Vehicle.cs`

**Error:** `400 Bad Request` - "Vehicle cannot be older than 5 years."

### Rule 2: One Vehicle Per Rental
Each vehicle can only have one active rental at a time.

**Location:** `Domain/Entities/Vehicle.cs` + `ApplicationCore/Rentals/Commands/RentVehicleCommandHandler.cs`

**Error:** `400 Bad Request` - "Vehicle is not available for rental."

## ??? Troubleshooting

### Docker: Port 5000 already in use
```bash
# Find process using port 5000
lsof -i :5000

# Kill the process
kill -9 <PID>

# Or use a different port
docker-compose -f docker-compose.yml up -p 5001:5000
```

### Tests fail with JSON parsing errors
This is usually due to camelCase/PascalCase mismatch. The solution is already implemented in test fixtures.

**Reference:** `test/functional/GtMotive.Estimate.Microservice.FunctionalTests/Rentals/RentalsHttpIntegrationTests.cs`

### .NET CLI issues
```bash
# Clear cache and restore
dotnet clean
dotnet restore

# Rebuild
dotnet build

# Run tests again
dotnet test microservice.sln
```

## ?? Architecture

This microservice follows **Hexagonal Architecture** principles:

```
???????????????????????????????????
?  API Layer (REST Endpoints)     ?
???????????????????????????????????
?  ApplicationCore (Commands/Queries)
???????????????????????????????????
?  Domain (Business Logic)        ?
???????????????????????????????????
?  Infrastructure (Persistence)   ?
???????????????????????????????????
```

**Benefits:**
- ? Clean separation of concerns
- ? Easy to test at multiple levels
- ? Flexible to change persistence layer
- ? No external dependencies in domain

## ?? Documentation

For detailed information, see:

- **Quick Start:** `START_HERE.md` - 1-minute overview
- **Comprehensive Guide:** `MASTER_PROMPT.md` - Complete project reference
- **How to Use:** `HOW_TO_USE_MASTER_PROMPT.md` - For future development
- **Verification:** `COMPLIANCE_VERIFICATION.md` - Full requirement checklist
- **Presentation:** `PRESENTATION_GUIDE.md` - Demo script

## ?? Development Workflow

### Adding a New Endpoint

1. **Define in Domain** (if business logic)
   ```csharp
   // Domain/Entities/YourEntity.cs
   ```

2. **Create Command/Query** (in ApplicationCore)
   ```csharp
   // ApplicationCore/YourFeature/Commands/MyCommandHandler.cs
   public class MyCommand : IRequest<MyResponse>
   public class MyCommandHandler : IRequestHandler<MyCommand, MyResponse>
   public class MyCommandValidator : AbstractValidator<MyCommand>
   ```

3. **Add Controller** (in Api)
   ```csharp
   // Api/Controllers/MyController.cs
   [HttpPost]
   public async Task<IActionResult> MyEndpoint(MyRequestDto request)
   {
       var result = await mediator.Send(new MyCommand { ... });
       return CreatedAtAction(nameof(MyEndpoint), result);
   }
   ```

4. **Write Tests**
   - Unit test: `test/unit/...`
   - Infrastructure test: `test/infrastructure/.../MyEndpointTests.cs`
   - Functional test: `test/functional/.../MyWorkflowTests.cs`

5. **Run Tests**
   ```bash
   dotnet test microservice.sln
   ```

## ?? Deployment

### Docker Production Build
```bash
cd src
docker build -f Dockerfile -t vehicle-rental:latest .
docker run -p 5000:5000 vehicle-rental:latest
```

### Requirements for Production
- ? .NET 9 Runtime (included in Docker image)
- ? No external databases (uses in-memory storage)
- ? Port 5000 accessible
- ? Optional: Configure appsettings for environment

## ?? Support

For issues or questions:

1. Check `MASTER_PROMPT.md` for project reference
2. Review `PRESENTATION_GUIDE.md` for examples
3. Examine test cases for usage patterns
4. See `HOW_TO_USE_MASTER_PROMPT.md` for development guidelines

## ?? License

This project is provided as-is for educational and commercial use.

## ? Key Features

| Feature | Status | Location |
|---------|--------|----------|
| Create vehicles | ? | `POST /api/vehicles` |
| List available | ? | `GET /api/vehicles/available` |
| Rent vehicle | ? | `POST /api/rentals` |
| Return vehicle | ? | `PUT /api/rentals/{id}/return` |
| Max 5 years validation | ? | `Domain/Entities/Vehicle.cs` |
| 1 rental per vehicle | ? | `Domain/Entities/Vehicle.cs` |
| Comprehensive tests | ? | 45 tests (all passing) |
| Docker support | ? | `docker-compose.yml` |
| .NET 9 local | ? | `dotnet run` |

---

**Last Updated:** 26/12/2025  
**Status:** ? Production Ready  
**Tests:** 45/45 Passing
