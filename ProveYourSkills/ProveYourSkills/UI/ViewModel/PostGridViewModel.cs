using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using ProveYourSkills.Core.Services.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProveYourSkills.UI.ViewModel;

public class PostGridViewModel : INotifyPropertyChanged
{
    private readonly IPostApiClient _client;
    private readonly ILogger<PostGridViewModel> _logger;

    private ObservableCollection<PostViewModel> _postCells = new ObservableCollection<PostViewModel>();
    private bool _displayIds = true;

    public AsyncRelayCommand? ToggleContentCommand { get; init; }

    /// <summary>
    /// Collection that contains all posts that are going to be represented in the window
    /// </summary>
    public ObservableCollection<PostViewModel>? PostCells
    {
        get { return _postCells; }
        set
        {
            if (_postCells != value && value != null)
            {
                _postCells = value;
                OnPropertyChanged(nameof(PostCells));
            }
        }
    }
    public bool DisplayIds { get { return _displayIds; } }

    public event PropertyChangedEventHandler? PropertyChanged;

    public PostGridViewModel(IPostApiClient client, ILogger<PostGridViewModel> logger)
    {
        _client = client;
        _logger = logger;
        ToggleContentCommand = new AsyncRelayCommand(ToggleValuesAsync);
    }

    // notify property changed
    protected virtual void OnPropertyChanged(string propertyName)
    {
        _logger.LogInformation($"Property '{propertyName}' has been changed");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Loads all posts into the ViewModel
    /// </summary>
    /// <returns></returns>
    public async Task InitializePostsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Initializing the post collection");     
        PostCells = new ObservableCollection<PostViewModel>(await GetPostViewModelsAsync(cancellationToken));
        _logger.LogInformation($"The post collection has been initialized with {PostCells.Count} element(s)");
    }

    /// <summary>
    /// change the content for all the cell within the PostCells Observable collection
    /// </summary>
    /// <returns></returns>
    public Task ToggleValuesAsync()
    {
        _logger.LogInformation($"Initializing the content values switch from {_displayIds} to {!_displayIds}");

        if (PostCells == null || !PostCells.Any())
        {
            _logger.LogWarning($"There aren't any post in the collection... Switch aborted");
            return Task.CompletedTask;
        }

        _displayIds = !_displayIds;

        foreach (var postCell in PostCells)
        {
            postCell.SwitchContent(_displayIds);
        }

        _logger.LogInformation($"Content values switched successfully from {!_displayIds} to {_displayIds}");

        return Task.CompletedTask;
    }

    private async Task<IEnumerable<PostViewModel>> GetPostViewModelsAsync(CancellationToken cancellationToken)
    {
        var posts = await _client.GetPostsAsync(cancellationToken);
        return posts.Select(post => new PostViewModel(post));
    }
}
