using System;
using Xunit;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Entities;

namespace GtMotive.Estimate.Microservice.UnitTests.Domain.Entities
{
    /// <summary>
    /// Unit tests for Vehicle entity.
    /// </summary>
    public class VehicleTests
    {
        [Fact]
        public void CreateVehicle_WithValidData_ShouldSucceed()
        {
            // Arrange
            var vehicleId = "VEHICLE001";
            var brand = "Toyota";
            var model = "Corolla";
            var manufacturingDate = DateTime.UtcNow.AddYears(-2);

            // Act
            var vehicle = new Vehicle(vehicleId, brand, model, manufacturingDate);

            // Assert
            Assert.Equal(vehicleId, vehicle.Id);
            Assert.Equal(brand, vehicle.Brand);
            Assert.Equal(model, vehicle.Model);
            Assert.Equal(VehicleStatus.Available, vehicle.Status);
        }

        [Fact]
        public void CreateVehicle_WithOlderThan5Years_ShouldThrowException()
        {
            // Arrange
            var vehicleId = "VEHICLE002";
            var brand = "Toyota";
            var model = "Corolla";
            var manufacturingDate = DateTime.UtcNow.AddYears(-6);

            // Act & Assert
            Assert.Throws<DomainException>(() => new Vehicle(vehicleId, brand, model, manufacturingDate));
        }

        [Fact]
        public void CreateVehicle_WithEmptyId_ShouldThrowException()
        {
            // Arrange
            var brand = "Toyota";
            var model = "Corolla";
            var manufacturingDate = DateTime.UtcNow.AddYears(-2);

            // Act & Assert
            Assert.Throws<DomainException>(() => new Vehicle(string.Empty, brand, model, manufacturingDate));
        }

        [Fact]
        public void MarkAsRented_WithValidRenterId_ShouldChangeStatus()
        {
            // Arrange
            var vehicle = new Vehicle("VEHICLE003", "Toyota", "Corolla", DateTime.UtcNow.AddYears(-2));
            var renterId = "RENTER001";

            // Act
            vehicle.MarkAsRented(renterId);

            // Assert
            Assert.Equal(VehicleStatus.Rented, vehicle.Status);
            Assert.Equal(renterId, vehicle.CurrentRenterId);
        }

        [Fact]
        public void MarkAsRented_WhenNotAvailable_ShouldThrowException()
        {
            // Arrange
            var vehicle = new Vehicle("VEHICLE004", "Toyota", "Corolla", DateTime.UtcNow.AddYears(-2));
            vehicle.MarkAsRented("RENTER001");

            // Act & Assert
            Assert.Throws<DomainException>(() => vehicle.MarkAsRented("RENTER002"));
        }

        [Fact]
        public void MarkAsAvailable_WhenRented_ShouldResetStatus()
        {
            // Arrange
            var vehicle = new Vehicle("VEHICLE005", "Toyota", "Corolla", DateTime.UtcNow.AddYears(-2));
            vehicle.MarkAsRented("RENTER001");

            // Act
            vehicle.MarkAsAvailable();

            // Assert
            Assert.Equal(VehicleStatus.Available, vehicle.Status);
            Assert.Null(vehicle.CurrentRenterId);
        }
    }
}
