using Microsoft.Extensions.Logging;
using ProveYourSkills.UI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ProveYourSkills
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger<MainWindow> _logger;
        
        public MainWindow(PostGridViewModel viewModel, ILogger<MainWindow> logger)
        {
            _logger = logger;
            DataContext = viewModel;
            InitializeComponent();
            this.Loaded += async (s, e) => await InitializeGridContent(viewModel);
        }

        public async Task InitializeGridContent(PostGridViewModel viewModel)
        {
            _logger.LogInformation("Initializing the Grid content");
            
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(5000);
            
            await viewModel.InitializePosts(tokenSource.Token);

            if (!viewModel.PostCells!.Any())
            {
                return;
            }

            foreach(var post in viewModel.PostCells)
            {
                // create ui elements
                var border = CreateBorder();
                var textbox = CreateTextBlock();
                var contentBinding =  CreateBinding(post);
                
                // connect elements and view model
                textbox.SetBinding(TextBlock.TextProperty, contentBinding);
                border.Child = textbox;
                PostsGrid.Children.Add(border);
            }

            _logger.LogInformation("The Grid content successfully initialized");

        }

        private Binding CreateBinding(PostViewModel post)
        {
            var contentBinding = new Binding("Content")
            {
                Source = post,
                Mode = BindingMode.OneWay
            };

            return contentBinding;
        }

        private TextBlock CreateTextBlock()
        {
            var textbox = new TextBlock();

            textbox.HorizontalAlignment = HorizontalAlignment.Center;
            textbox.VerticalAlignment = VerticalAlignment.Center;
            // colors should be defined in resource definitions or configuration
            textbox.Foreground = new SolidColorBrush(Color.FromRgb(174, 68, 90));

            return textbox;
        }
        
        private Border CreateBorder()
        {
            var border = new Border();
            // colors should be defined in resource definitions or configuration
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 188, 185));
            border.BorderThickness = new Thickness(1);
            border.Padding = new Thickness(1);
            border.Margin = new Thickness(1);

            return border;
        }
    }
}