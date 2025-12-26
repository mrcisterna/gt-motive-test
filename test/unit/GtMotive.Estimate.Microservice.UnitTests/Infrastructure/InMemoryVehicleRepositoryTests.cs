using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using GtMotive.Estimate.Microservice.Domain.Entities;
using GtMotive.Estimate.Microservice.Infrastructure.Repositories;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.Infrastructure
{
    /// <summary>
    /// Unit tests for InMemoryVehicleRepository.
    /// </summary>
    public class InMemoryVehicleRepositoryTests
    {
        [Fact]
        public async Task AddAsyncWithValidVehicleAddsVehicleToRepository()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle = CreateTestVehicle();

            // Act
            await repository.AddAsync(vehicle);

            // Assert
            var result = await repository.GetByIdAsync(vehicle.Id);
            result.Should().NotBeNull();
            result.Id.Should().Be(vehicle.Id);
            result.Brand.Should().Be(vehicle.Brand);
        }

        [Fact]
        public async Task AddAsyncWithNullVehicleThrowsArgumentNullException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null));
        }

        [Fact]
        public async Task AddAsyncWithMultipleVehiclesAllAreAdded()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle1 = CreateTestVehicle("V001");
            var vehicle2 = CreateTestVehicle("V002");
            var vehicle3 = CreateTestVehicle("V003");

            // Act
            await repository.AddAsync(vehicle1);
            await repository.AddAsync(vehicle2);
            await repository.AddAsync(vehicle3);

            // Assert
            var all = await repository.GetAllAsync();
            all.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetByIdAsyncWithExistingIdReturnsVehicle()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle = CreateTestVehicle("V001");
            await repository.AddAsync(vehicle);

            // Act
            var result = await repository.GetByIdAsync("V001");

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("V001");
            result.Brand.Should().Be(vehicle.Brand);
        }

        [Fact]
        public async Task GetByIdAsyncWithNonExistingIdReturnsNull()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync("NONEXISTENT");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsyncWithEmptyIdReturnsNull()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetByIdAsync(string.Empty);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsyncWithNoVehiclesReturnsEmptyCollection()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsyncWithMultipleVehiclesReturnsAllVehicles()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle1 = CreateTestVehicle("V001");
            var vehicle2 = CreateTestVehicle("V002");
            await repository.AddAsync(vehicle1);
            await repository.AddAsync(vehicle2);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(v => v.Id == "V001");
            result.Should().ContainSingle(v => v.Id == "V002");
        }

        [Fact]
        public async Task GetAvailableAsyncWithNoVehiclesReturnsEmptyCollection()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetAvailableAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAvailableAsyncWithOnlyAvailableVehiclesReturnsAllVehicles()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle1 = CreateTestVehicle("V001");
            var vehicle2 = CreateTestVehicle("V002");
            await repository.AddAsync(vehicle1);
            await repository.AddAsync(vehicle2);

            // Act
            var result = await repository.GetAvailableAsync();

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAvailableAsyncWithMixedStatusReturnsOnlyAvailable()
        {
            // Arrange
            var repository = CreateRepository();
            var availableVehicle = CreateTestVehicle("V001");
            var rentedVehicle = CreateTestVehicle("V002");
            rentedVehicle.MarkAsRented("RENTER001");

            await repository.AddAsync(availableVehicle);
            await repository.AddAsync(rentedVehicle);

            // Act
            var result = await repository.GetAvailableAsync();

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(v => v.Id == "V001");
        }

        [Fact]
        public async Task UpdateAsyncWithValidVehicleUpdatesVehicle()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle = CreateTestVehicle("V001");
            await repository.AddAsync(vehicle);

            vehicle.MarkAsRented("RENTER001");

            // Act
            await repository.UpdateAsync(vehicle);

            // Assert
            var result = await repository.GetByIdAsync("V001");
            result.Should().NotBeNull();
            result.Status.Should().Be(VehicleStatus.Rented);
        }

        [Fact]
        public async Task UpdateAsyncWithNonExistingVehicleThrowsKeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle = CreateTestVehicle("V999");

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateAsync(vehicle));
        }

        [Fact]
        public async Task UpdateAsyncWithNullVehicleThrowsArgumentNullException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(null));
        }

        [Fact]
        public async Task DeleteAsyncWithExistingIdRemovesVehicle()
        {
            // Arrange
            var repository = CreateRepository();
            var vehicle = CreateTestVehicle("V001");
            await repository.AddAsync(vehicle);

            // Act
            await repository.DeleteAsync("V001");

            // Assert
            var result = await repository.GetByIdAsync("V001");
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsyncWithNonExistingIdDoesNotThrowException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var act = async () => await repository.DeleteAsync("NONEXISTENT");

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteAsyncWithEmptyIdDoesNotThrowException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var act = async () => await repository.DeleteAsync(string.Empty);

            // Assert
            await act.Should().NotThrowAsync();
        }

        private static InMemoryVehicleRepository CreateRepository()
        {
            return new InMemoryVehicleRepository();
        }

        private static Vehicle CreateTestVehicle(string id = "V001", string brand = "Toyota", string model = "Corolla")
        {
            return new Vehicle(id, brand, model, DateTime.UtcNow.AddYears(-2));
        }
    }
}
