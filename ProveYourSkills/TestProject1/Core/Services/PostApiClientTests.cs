using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProveYourSkills.Core.Models;
using ProveYourSkills.Core.Services;
using ProveYourSkills.Infrastructure.Configuration;
using ProveYourSkills.Infrastructure.Http.Abstractions;
using ProveYourSkills.Infrastructure.Http.Settings;
using System.Text.Json;

namespace TestProject1.Core.Services;

public class PostApiClientTests
{
    private readonly Mock<IRestApiClient> _mockRestApiClient;
    private readonly Mock<ILogger<PostApiClient>> _mockLogger;
    private readonly Mock<IOptionsSnapshot<AppSettings>> _mockOptions;
    private readonly PostApiClient _postApiClient;

    public PostApiClientTests()
    {
        _mockRestApiClient = new Mock<IRestApiClient>();
        _mockLogger = new Mock<ILogger<PostApiClient>>();
        _mockOptions = new Mock<IOptionsSnapshot<AppSettings>>();

        var appSettings = new AppSettings { BaseUri = "https://jsonplaceholder.typicode.com/" };
        _mockOptions.Setup(o => o.Value).Returns(appSettings);

        _postApiClient = new PostApiClient(_mockRestApiClient.Object, _mockLogger.Object, _mockOptions.Object);
    }

    [Fact]
    public async Task GetPosts_ReturnsPosts_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedPosts = new List<Post>
        {
            new Post { UserId = 1, Id = 1, Title = "Post 1", Body = "Body 1" },
            new Post { UserId = 2, Id = 2, Title = "Post 2", Body = "Body 2" }
        };

        _mockRestApiClient
            .Setup(client => client.GetAsync<IEnumerable<Post>>(It.IsAny<string>(), It.IsAny<RestApiSettings>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedPosts);

        // Act
        var result = await _postApiClient.GetPostsAsync();

        // Assert
        Assert.Equal(expectedPosts, result);
    }

    [Fact]
    public async Task GetPosts_ReturnsEmptyList_WhenApiCallFailsWithHttpRequestException()
    {
        // Arrange
        _mockRestApiClient
            .Setup(client => client.GetAsync<IEnumerable<Post>>(It.IsAny<string>(), It.IsAny<RestApiSettings>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Request failed"));

        // Act &Assert
        await Assert.ThrowsAsync<PostApiException>(() => _postApiClient.GetPostsAsync());
        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("HTTP error")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPosts_ReturnsEmptyList_WhenApiCallFailsWithJsonException()
    {
        // Arrange
        _mockRestApiClient
            .Setup(client => client.GetAsync<IEnumerable<Post>>(It.IsAny<string>(), It.IsAny<RestApiSettings>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new JsonException("Serialization failed"));

        // Act & Assert
        await Assert.ThrowsAsync<PostApiException>(() => _postApiClient.GetPostsAsync());
        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error received while deserializing")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPosts_ReturnsEmptyList_WhenRequestIsCancelled()
    {
        // Arrange
        var cancellationToken = new CancellationToken(true);

        // Act
        var result = await _postApiClient.GetPostsAsync(cancellationToken);

        // Assert
        Assert.Empty(result);
        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Cancellation")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPosts_ReturnsEmptyList_WhenUnexpectedExceptionOccurs()
    {
        // Arrange
        _mockRestApiClient
            .Setup(client => client.GetAsync<IEnumerable<Post>>(It.IsAny<string>(), It.IsAny<RestApiSettings>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Some random error"));

        // Act & Assert
        await Assert.ThrowsAsync<PostApiException>(() => _postApiClient.GetPostsAsync());
        _mockLogger.Verify(
            x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Unexpected")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
