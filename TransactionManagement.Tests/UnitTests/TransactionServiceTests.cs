using Microsoft.EntityFrameworkCore;
using Moq;
using TransactionManagement.BLL.Services;
using TransactionManagement.BLL.Interfaces; 
using TransactionManagement.DAL.Context;
using TransactionManagement.DTO.Models;

namespace TransactionManagement.Tests.UnitTests
{
    public class TransactionServiceTests
    {
        [Fact]
        public async Task UploadingCSVFile_Should_Add_Transaction_To_Database()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var mockDbContext = new ApiDbContext(options);

            var mockLocationService = new Mock<ILocation>();
            mockLocationService.Setup(m => m.GetLocationInfoAsync(It.IsAny<string>()))
                                .ReturnsAsync(new LocationDTO
                                {
                                    IANAZone = "America/New_York",
                                    UTC = "UTC-5",
                                    Coordinates = "6.602635264, -98.2909591552"
                                });

            var service = new TransactionService(mockDbContext, mockLocationService.Object);

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestFiles", "valid_data.csv");

            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // Act
            await service.UploadingCSVFile(fileStream);

            // Assert
            var transaction = await mockDbContext.Transactions.FirstOrDefaultAsync();
            Assert.NotNull(transaction);

            Assert.Equal("T-1-67.63636363636364_0.76", transaction.TransactionId);
            Assert.Equal("Adria Pugh", transaction.Name);
            Assert.Equal("odio.a.purus@protonmail.edu", transaction.Email);
            Assert.Equal(Math.Round(375.39,2), Math.Round(transaction.Amount,2));
            Assert.Equal(DateTime.Parse("2024-01-10 01:16:23"), transaction.TransactionDate);
            Assert.Equal("6.602635264, -98.2909591552", transaction.Location.Coordinates);
        }
    }
}