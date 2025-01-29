using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Input;

namespace ProveYourSkills
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }

    public interface IMainViewModel
    {
        ICommand SaveCommand { get; }
    }

    public class MainViewModel : IMainViewModel
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly IJsonPlaceholderClient _placeholderClient;

        public MainViewModel(ILogger<MainViewModel> logger, IJsonPlaceholderClient placeholderClient)
        {
            _logger = logger;
            _placeholderClient = placeholderClient;
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new AsyncRelayCommand(ExecuteSaveAsync);

        private async Task ExecuteSaveAsync()
        {
            try
            {
                var json1 = await _placeholderClient.GetPosts();
                _logger.LogInformation("Save command executed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during save operation");
            }
        }
    }
}