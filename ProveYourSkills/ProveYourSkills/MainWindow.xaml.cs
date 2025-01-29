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

        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand =>
            _saveCommand ??= new AsyncRelayCommand(ExecuteSaveAsync);

        private async Task ExecuteSaveAsync()
        {
            try
            {
                _logger.LogInformation("Save command executed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during save operation");
            }
        }
    }
}