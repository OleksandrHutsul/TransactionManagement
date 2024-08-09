using System.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using TransactionManagement.BLL.Services;

namespace TransactionManagement.Tests.UnitTests
{
    public class LocationServiceTests
    {
        [Fact]
        public async Task GetLocationInfoAsync_Should_Return_Correct_LocationDTO_For_Given_Coordinates()
        {
            // Arrange
            var geoNamesUsername = "hutsuloleksandr";
            var expectedResponse = JObject.FromObject(new
            {
                timezoneId = "America/Toronto",
                rawOffset = "-5"
            }).ToString();

            var httpClientMock = new Mock<HttpMessageHandler>();

            httpClientMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
                {
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(expectedResponse)
                    };
                });

            var httpClient = new HttpClient(httpClientMock.Object);
            var service = new LocationService(httpClient, geoNamesUsername);

            var coordinates = "51.110318592, -77.2466440192";

            // Act
            var result = await service.GetLocationInfoAsync(coordinates);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("America/Toronto", result.IANAZone);
            Assert.Equal("-5", result.UTC);
            Assert.Equal(coordinates, result.Coordinates);
        }
    }
}