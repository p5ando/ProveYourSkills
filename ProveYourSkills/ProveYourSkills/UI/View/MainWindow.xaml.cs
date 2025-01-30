using Microsoft.Extensions.Logging;
using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.UI.ViewModel;
using System.Windows;

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

        foreach(var post in viewModel?.PostCells ?? Enumerable.Empty<PostViewModel>())
        {
            // create ui elements
            //var border = _uiComponentFactory.CreateBorder();
            //var textbox = _uiComponentFactory.CreateTextBlock();
            //var contentBinding = _uiComponentFactory.CreateContentBinding(post);

            // connect elements and view model
            //textbox.SetBinding(TextBlock.TextProperty, contentBinding);
            //border.Child = textbox;
            var cell = _gridCellBuilder
                .Reset()
                .CreateBorder()
                .CreateTextBlock()
                .CreateBinding(post)
                .Build();

            PostsGrid.Children.Add(cell);
        }

        _logger.LogInformation("The Grid content successfully initialized");

    }
}