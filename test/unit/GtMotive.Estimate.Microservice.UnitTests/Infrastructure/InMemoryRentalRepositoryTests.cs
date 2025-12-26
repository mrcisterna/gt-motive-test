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
    /// Unit tests for InMemoryRentalRepository.
    /// </summary>
    public class InMemoryRentalRepositoryTests
    {
        [Fact]
        public async Task AddAsyncWithValidRentalAddsRentalToRepository()
        {
            // Arrange
            var repository = CreateRepository();
            var rental = CreateTestRental();

            // Act
            await repository.AddAsync(rental);

            // Assert
            var result = await repository.GetByIdAsync(rental.Id);
            result.Should().NotBeNull();
            result.Id.Should().Be(rental.Id);
            result.VehicleId.Should().Be(rental.VehicleId);
        }

        [Fact]
        public async Task AddAsyncWithNullRentalThrowsArgumentNullException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.AddAsync(null));
        }

        [Fact]
        public async Task AddAsyncWithMultipleRentalsAllAreAdded()
        {
            // Arrange
            var repository = CreateRepository();
            var rental1 = CreateTestRental("R001", "V001", "RENTER001");
            var rental2 = CreateTestRental("R002", "V002", "RENTER002");
            var rental3 = CreateTestRental("R003", "V003", "RENTER003");

            // Act
            await repository.AddAsync(rental1);
            await repository.AddAsync(rental2);
            await repository.AddAsync(rental3);

            // Assert
            var all = await repository.GetAllAsync();
            all.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetByIdAsyncWithExistingIdReturnsRental()
        {
            // Arrange
            var repository = CreateRepository();
            var rental = CreateTestRental("R001");
            await repository.AddAsync(rental);

            // Act
            var result = await repository.GetByIdAsync("R001");

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("R001");
            result.VehicleId.Should().Be(rental.VehicleId);
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
        public async Task GetAllAsyncWithNoRentalsReturnsEmptyCollection()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsyncWithMultipleRentalsReturnsAllRentals()
        {
            // Arrange
            var repository = CreateRepository();
            var rental1 = CreateTestRental("R001");
            var rental2 = CreateTestRental("R002");
            await repository.AddAsync(rental1);
            await repository.AddAsync(rental2);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainSingle(r => r.Id == "R001");
            result.Should().ContainSingle(r => r.Id == "R002");
        }

        [Fact]
        public async Task GetActiveRentalsByRenterAsyncWithNoRentalsReturnsEmptyCollection()
        {
            // Arrange
            var repository = CreateRepository();

            // Act
            var result = await repository.GetActiveRentalsByRenterAsync("RENTER001");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetActiveRentalsByRenterAsyncWithOnlyActiveRentalsReturnsAll()
        {
            // Arrange
            var repository = CreateRepository();
            var rental1 = CreateTestRental("R001", "V001", "RENTER001");
            var rental2 = CreateTestRental("R002", "V002", "RENTER001");
            await repository.AddAsync(rental1);
            await repository.AddAsync(rental2);

            // Act
            var result = await repository.GetActiveRentalsByRenterAsync("RENTER001");

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetActiveRentalsByRenterAsyncWithMixedStatusReturnsOnlyActive()
        {
            // Arrange
            var repository = CreateRepository();
            var activeRental = CreateTestRental("R001", "V001", "RENTER001");
            var completedRental = CreateTestRental("R002", "V002", "RENTER001");
            completedRental.Complete();

            await repository.AddAsync(activeRental);
            await repository.AddAsync(completedRental);

            // Act
            var result = await repository.GetActiveRentalsByRenterAsync("RENTER001");

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(r => r.Id == "R001");
        }

        [Fact]
        public async Task GetActiveRentalsByRenterAsyncWithDifferentRentersReturnsOnlyForSpecificRenter()
        {
            // Arrange
            var repository = CreateRepository();
            var renter1Rental = CreateTestRental("R001", "V001", "RENTER001");
            var renter2Rental = CreateTestRental("R002", "V002", "RENTER002");
            await repository.AddAsync(renter1Rental);
            await repository.AddAsync(renter2Rental);

            // Act
            var result = await repository.GetActiveRentalsByRenterAsync("RENTER001");

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(r => r.RenterId == "RENTER001");
        }

        [Fact]
        public async Task UpdateAsyncWithValidRentalUpdatesRental()
        {
            // Arrange
            var repository = CreateRepository();
            var rental = CreateTestRental("R001");
            await repository.AddAsync(rental);

            rental.Complete();

            // Act
            await repository.UpdateAsync(rental);

            // Assert
            var result = await repository.GetByIdAsync("R001");
            result.Should().NotBeNull();
            result.Status.Should().Be(RentalStatus.Completed);
        }

        [Fact]
        public async Task UpdateAsyncWithNonExistingRentalThrowsKeyNotFoundException()
        {
            // Arrange
            var repository = CreateRepository();
            var rental = CreateTestRental("R999");

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.UpdateAsync(rental));
        }

        [Fact]
        public async Task UpdateAsyncWithNullRentalThrowsArgumentNullException()
        {
            // Arrange
            var repository = CreateRepository();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => repository.UpdateAsync(null));
        }

        [Fact]
        public async Task DeleteAsyncWithExistingIdRemovesRental()
        {
            // Arrange
            var repository = CreateRepository();
            var rental = CreateTestRental("R001");
            await repository.AddAsync(rental);

            // Act
            await repository.DeleteAsync("R001");

            // Assert
            var result = await repository.GetByIdAsync("R001");
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

        private static InMemoryRentalRepository CreateRepository()
        {
            return new InMemoryRentalRepository();
        }

        private static Rental CreateTestRental(
            string id = "R001",
            string vehicleId = "V001",
            string renterId = "RENTER001")
        {
            return new Rental(id, vehicleId, renterId);
        }
    }
}
