using System;
using System.Threading.Tasks;
using Xunit;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Create;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.List;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Create;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Infrastructure.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.UnitOfWork;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Vehicles
{
    /// <summary>
    /// Functional tests for vehicle rental workflow.
    /// </summary>
    public class VehicleRentalWorkflowTests
    {
        private readonly IVehicleRepository vehicleRepository;
        private readonly IRentalRepository rentalRepository;
        private readonly IUnitOfWork unitOfWork;

        public VehicleRentalWorkflowTests()
        {
            // Initialize in-memory repositories
            vehicleRepository = new InMemoryVehicleRepository();
            rentalRepository = new InMemoryRentalRepository();
            unitOfWork = new InMemoryUnitOfWork();
        }

        [Fact]
        public async Task CreateVehicle_ThenListAvailable_ShouldReturnCreatedVehicle()
        {
            // Arrange
            var vehicleId = "VEHICLE-FUNC-001";
            var brand = "BMW";
            var model = "X5";
            var manufacturingDate = DateTime.UtcNow.AddYears(-2);

            var createVehiclePresenter = new TestCreateVehiclePresenter();
            var createVehicleUseCase = new CreateVehicleUseCase(
                vehicleRepository,
                unitOfWork,
                createVehiclePresenter);

            var createInput = new CreateVehicleInput
            {
                Id = vehicleId,
                Brand = brand,
                Model = model,
                ManufacturingDate = manufacturingDate,
            };

            // Act - Create vehicle
            await createVehicleUseCase.Execute(createInput);

            // Assert - Vehicle created
            Assert.NotNull(createVehiclePresenter.Output);
            Assert.Equal(vehicleId, createVehiclePresenter.Output.VehicleId);

            // Act - List available vehicles
            var listPresenter = new TestListAvailableVehiclesPresenter();
            var listUseCase = new ListAvailableVehiclesUseCase(vehicleRepository, listPresenter);
            await listUseCase.Execute(new ListAvailableVehiclesInput());

            // Assert - Vehicle should be in the list
            Assert.NotNull(listPresenter.Output);
            Assert.Single(listPresenter.Output.Vehicles);
            Assert.Contains(listPresenter.Output.Vehicles, v => v.Id == vehicleId);
        }

        [Fact]
        public async Task RentVehicle_WithValidData_ShouldCreateRental()
        {
            // Arrange
            var vehicleId = "VEHICLE-FUNC-002";
            var renterId = "RENTER-FUNC-001";

            // Create a vehicle first
            var vehicle = new Domain.Entities.Vehicle(
                vehicleId,
                "Mercedes",
                "C-Class",
                DateTime.UtcNow.AddYears(-1));
            await vehicleRepository.AddAsync(vehicle);

            var rentPresenter = new TestRentVehiclePresenter();
            var rentUseCase = new RentVehicleUseCase(
                vehicleRepository,
                rentalRepository,
                unitOfWork,
                rentPresenter,
                new TestNotFoundPort());

            var rentInput = new RentVehicleInput
            {
                VehicleId = vehicleId,
                RenterId = renterId,
            };

            // Act
            await rentUseCase.Execute(rentInput);

            // Assert
            Assert.NotNull(rentPresenter.Output);
            Assert.Equal(vehicleId, rentPresenter.Output.VehicleId);

            // Verify vehicle status changed
            var updatedVehicle = await vehicleRepository.GetByIdAsync(vehicleId);
            Assert.Equal(Domain.Entities.VehicleStatus.Rented, updatedVehicle.Status);
        }

        [Fact]
        public async Task RentVehicle_WhenRenterHasActiveRental_ShouldThrowException()
        {
            // Arrange
            var renterId = "RENTER-FUNC-002";
            var vehicle1Id = "VEHICLE-FUNC-003";
            var vehicle2Id = "VEHICLE-FUNC-004";

            // Create two vehicles
            var vehicle1 = new Domain.Entities.Vehicle(
                vehicle1Id,
                "Audi",
                "A4",
                DateTime.UtcNow.AddYears(-2));
            var vehicle2 = new Domain.Entities.Vehicle(
                vehicle2Id,
                "Volvo",
                "S90",
                DateTime.UtcNow.AddYears(-1));

            await vehicleRepository.AddAsync(vehicle1);
            await vehicleRepository.AddAsync(vehicle2);

            var rentPresenter = new TestRentVehiclePresenter();
            var rentUseCase = new RentVehicleUseCase(
                vehicleRepository,
                rentalRepository,
                unitOfWork,
                rentPresenter,
                new TestNotFoundPort());

            // Rent first vehicle
            var rentInput1 = new RentVehicleInput { VehicleId = vehicle1Id, RenterId = renterId };
            await rentUseCase.Execute(rentInput1);

            // Act & Assert - Try to rent second vehicle with same renter
            var rentInput2 = new RentVehicleInput { VehicleId = vehicle2Id, RenterId = renterId };
            var exception = await Assert.ThrowsAsync<Domain.DomainException>(() => rentUseCase.Execute(rentInput2));
            Assert.Contains("only have one active rental", exception.Message);
        }

        // Test presenter implementations
        private class TestCreateVehiclePresenter : ICreateVehicleOutputPort
        {
            public CreateVehicleOutput Output { get; private set; }

            public void StandardHandle(CreateVehicleOutput response)
            {
                Output = response;
            }
        }

        private class TestListAvailableVehiclesPresenter : IListAvailableVehiclesOutputPort
        {
            public ListAvailableVehiclesOutput Output { get; private set; }

            public void StandardHandle(ListAvailableVehiclesOutput response)
            {
                Output = response;
            }
        }

        private class TestRentVehiclePresenter : IRentVehicleOutputPort
        {
            public RentVehicleOutput Output { get; private set; }

            public void StandardHandle(RentVehicleOutput response)
            {
                Output = response;
            }
        }

        private class TestNotFoundPort : ApplicationCore.UseCases.IOutputPortNotFound
        {
            public void NotFoundHandle(string message)
            {
                throw new ApplicationCore.UseCases.NotFoundOutputException(message);
            }
        }
    }

    /// <summary>
    /// Exception for not found output port.
    /// </summary>
    public class NotFoundOutputException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundOutputException"/> class.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public NotFoundOutputException(string message)
            : base(message)
        {
        }
    }
}
