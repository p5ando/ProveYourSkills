using CommunityToolkit.Mvvm.Input;
using Http;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProveYourSkills
{
    public class PostGridViewModel : INotifyPropertyChanged
    {
        private IJsonPlaceholderClient _client;
        private ILogger<PostGridViewModel> _logger;
        private ObservableCollection<PostViewModel>? _postCells;
        public bool displayIds = true;

        public AsyncRelayCommand? ToggleContentCommand { get; init; }

        /// <summary>
        /// Collection that contains all posts that are going to be represented in the window
        /// </summary>
        public ObservableCollection<PostViewModel>? PostCells
        {
            get { return _postCells; }
            set
            {
                if (_postCells != value)
                {
                    _postCells = value;
                    OnPropertyChanged(nameof(PostCells));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PostGridViewModel(IJsonPlaceholderClient client, ILogger<PostGridViewModel> logger)
        {
            _client = client;
            _logger = logger;
            ToggleContentCommand = new AsyncRelayCommand(ToggleValues);
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
        public async Task InitializePosts()
        {
            _logger.LogInformation("Initializing the post collection");
            PostCells = new ObservableCollection<PostViewModel>(await GetPostViewModels());
            _logger.LogInformation($"The post collection has been initialized with {PostCells.Count} element(s)");
        }

        /// <summary>
        /// change the content for all the cell within the PostCells Observable collection
        /// </summary>
        /// <returns></returns>
        public Task ToggleValues()
        {
            _logger.LogInformation($"Initializing the content values switch from {displayIds} to {!displayIds}");

            if (PostCells == null)
            {
                _logger.LogWarning($"There aren't any post in the collection... Switch aborted");
                return Task.CompletedTask;
            }

            displayIds = !displayIds;

            foreach (var postCell in PostCells)
            {
                postCell.SwitchContent(displayIds);
            }

            _logger.LogInformation($"Content values switched successfully from {!displayIds} to {displayIds}");

            return Task.CompletedTask;
        }

        private async Task<IEnumerable<PostViewModel>> GetPostViewModels()
        {
            var posts = await _client.GetPosts();
            return posts.Select(post => new PostViewModel(post));
        }
    }
}
