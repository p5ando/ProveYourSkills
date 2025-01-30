using Microsoft.Extensions.Logging;
using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.UI.ViewModel;
using System.Windows;
using System.Windows.Media;

namespace ProveYourSkills;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ILogger<MainWindow> _logger;
    private readonly IGridCellBuilder _gridCellBuilder;

    public MainWindow(PostGridViewModel viewModel, IGridCellBuilder gridCellBuilder, ILogger<MainWindow> logger)
    {
        _logger = logger;
        _gridCellBuilder = gridCellBuilder;
        DataContext = viewModel;
        InitializeComponent();
        this.Loaded += async (s, e) => await InitializeGridContentAsync(viewModel);
    }

    public async Task InitializeGridContentAsync(PostGridViewModel viewModel)
    {
        _logger.LogInformation("Initializing the Grid content");
            
        await viewModel.InitializePostsAsync();

        var textBrush = (Brush)Application.Current.Resources["TextBrush"];
        var borderBrush = (Brush)Application.Current.Resources["BorderBrush"];

        foreach (var post in viewModel?.PostCells ?? Enumerable.Empty<PostViewModel>())
        {
            var cell = _gridCellBuilder
                .Reset()
                .CreateBorder(borderBrush)
                .CreateTextBlock(textBrush)
                .CreateBinding(post)
                .Build();

            PostsGrid.Children.Add(cell);
        }

        _logger.LogInformation("The Grid content successfully initialized");

    }
}