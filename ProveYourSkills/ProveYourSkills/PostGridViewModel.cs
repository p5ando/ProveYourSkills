using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ProveYourSkills
{
    public class PostGridViewModel : INotifyPropertyChanged
    {
        private IJsonPlaceholderClient _client;
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

        public PostGridViewModel(IJsonPlaceholderClient client)
        {
            _client = client;
            ToggleContentCommand = new AsyncRelayCommand(ToggleValues);
        }

        // notify property changed
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Loads all posts into the ViewModel
        /// </summary>
        /// <returns></returns>
        public async Task InitializePosts()
        {
            var posts = await _client.GetPosts();
            PostCells = new ObservableCollection<PostViewModel>(
                posts.Select(post => new PostViewModel(post))
            );
        }

        /// <summary>
        /// change the content for all the cell within the PostCells Observable collection
        /// </summary>
        /// <returns></returns>
        public Task ToggleValues()
        {
            displayIds = !displayIds;

            if (PostCells == null)
            {
                return Task.CompletedTask;
            }

            foreach (var postCell in PostCells)
            {
                postCell.SwitchContent(displayIds);
            }

            return Task.CompletedTask;
        }
    }
}
