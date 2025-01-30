using Microsoft.Extensions.Logging;
using Moq;
using ProveYourSkills.Core.Models;
using ProveYourSkills.Core.Services.Abstractions;
using System.Collections.ObjectModel;

namespace ProveYourSkills.UI.ViewModel.Tests;

public class PostGridViewModelTests
{
    private readonly Mock<IPostApiClient> _mockClient;
    private readonly Mock<ILogger<PostGridViewModel>> _mockLogger;
    private readonly PostGridViewModel _viewModel;

    public PostGridViewModelTests()
    {
        _mockClient = new Mock<IPostApiClient>();
        _mockLogger = new Mock<ILogger<PostGridViewModel>>();
        _viewModel = new PostGridViewModel(_mockClient.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task InitializePosts_LoadsPosts_WhenApiCallIsSuccessful()
    {
        // Arrange
        var posts = new List<Post>
        {
            new Post { Id = 1, Title = "Post 1" },
            new Post { Id = 2, Title = "Post 2" }
        };
        _mockClient.Setup(client => client.GetPostsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(posts);

        // Act
        await _viewModel.InitializePostsAsync();

        // Assert
        Assert.NotNull(_viewModel.PostCells);
        Assert.Equal(posts.Count, _viewModel.PostCells.Count);
    }

    [Fact]
    public async Task InitializePosts_SetsEmptyCollection_WhenNoPostsAreAvailable()
    {
        // Arrange
        _mockClient.Setup(client => client.GetPostsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<Post>());

        // Act
        await _viewModel.InitializePostsAsync();

        // Assert
        Assert.NotNull(_viewModel.PostCells);
        Assert.Empty(_viewModel.PostCells);
    }

    [Fact]
    public async Task ToggleValues_SwitchesContent_WhenPostCellsAreAvailable()
    {
        // Arrange
        var postViewModels = new ObservableCollection<PostViewModel>
        {
            new PostViewModel(new Post { Id = 1, UserId = 11}),
            new PostViewModel(new Post { Id = 2, UserId = 22})
        };
        _viewModel.PostCells = postViewModels;

        // Act
        await _viewModel.ToggleValuesAsync();

        // Assert
        Assert.False(_viewModel.DisplayIds);
        Assert.True(postViewModels.First().Content == "11");
        Assert.True(postViewModels.Last().Content == "22");
    }

    [Fact]
    public async Task ToggleValues_DoesNothing_WhenPostCellsAreNull()
    {
        // Arrange
        _viewModel.PostCells = null;

        // Act
        await _viewModel.ToggleValuesAsync();

        // Assert
        Assert.True(_viewModel.DisplayIds);
    }
}
