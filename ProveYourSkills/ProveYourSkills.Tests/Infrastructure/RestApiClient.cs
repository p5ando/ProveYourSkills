using Moq;
using Moq.Protected;
using ProveYourSkills.Infrastructure.Http;
using ProveYourSkills.Infrastructure.Http.Settings;
using System.Net;
using System.Text.Json;

namespace ProveYourSkills.Infrastructure.Http;

public class RestApiClientTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly RestApiClient _restApiClient;

    public RestApiClientTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        _restApiClient = new RestApiClient(_mockHttpClientFactory.Object);
    }

    [Fact]
    public async Task Get_ReturnsData_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedData = new { UserId = 1, Id = 1, Title = "Post 1", Body = "Body 1" };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedData))
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var settings = new RestApiSettings
        {
            BaseUri = new Uri("https://jsonplaceholder.typicode.com/"),
            ContentType = "application/json"
        };

        // Act
        var result = await _restApiClient.GetAsync<object>("posts/1", settings);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedData.UserId, ((JsonElement)result).GetProperty("UserId").GetInt32());
        Assert.Equal(expectedData.Id, ((JsonElement)result).GetProperty("Id").GetInt32());
        Assert.Equal(expectedData.Title, ((JsonElement)result).GetProperty("Title").GetString());
        Assert.Equal(expectedData.Body, ((JsonElement)result).GetProperty("Body").GetString());
    }

    [Fact]
    public async Task Get_ThrowsException_WhenApiCallFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var settings = new RestApiSettings
        {
            BaseUri = new Uri("https://jsonplaceholder.typicode.com/"),
            ContentType = "application/json"
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _restApiClient.GetAsync<object>("posts/1", settings));
    }

    [Fact]
    public async Task Get_ThrowsOperationCanceledException_WhenRequestIsCancelled()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new OperationCanceledException());

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var settings = new RestApiSettings
        {
            BaseUri = new Uri("https://jsonplaceholder.typicode.com/"),
            ContentType = "application/json"
        };

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => _restApiClient.GetAsync<object>("posts/1", settings, cancellationToken));
    }
}
