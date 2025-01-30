using Moq;
using ProveYourSkills.Core.Models;
using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.Core.Services;
using ProveYourSkills.UI.ViewModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProveYourSkills.Tests.Core.Servicesp;

public class GridCellBuilderTests
{
    private readonly Mock<IUiComponentFactory> _uiComponentFactoryMock;
    private readonly GridCellBuilder _gridCellBuilder;

    public GridCellBuilderTests()
    {
        _uiComponentFactoryMock = new Mock<IUiComponentFactory>();
        _gridCellBuilder = new GridCellBuilder(_uiComponentFactoryMock.Object);
    }

    [Fact]
    public void Build_ShouldThrowInvalidOperationException_WhenNotFullyConfigured()
    {
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _gridCellBuilder.Build());
    }

    [StaFact]
    public void Build_ShouldReturnFrameworkElement_WhenFullyConfigured()
    {
        // Arrange
        var textBlock = new TextBlock();
        var border = new Border();
        var binding = new Binding();

        _uiComponentFactoryMock.Setup(f => f.CreateTextBlock(It.IsAny<System.Windows.Media.Brush>())).Returns(textBlock);
        _uiComponentFactoryMock.Setup(f => f.CreateBorder(It.IsAny<System.Windows.Media.Brush>())).Returns(border);
        _uiComponentFactoryMock.Setup(f => f.CreateContentBinding(It.IsAny<PostViewModel>())).Returns(binding);

        var postViewModel = new PostViewModel(new Post());
        _gridCellBuilder.CreateTextBlock(System.Windows.Media.Brushes.Black)
                        .CreateBorder(System.Windows.Media.Brushes.Red)
                        .CreateBinding(postViewModel);

        // Act
        var result = _gridCellBuilder.Build();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Border>(result);
        Assert.Equal(textBlock, ((Border)result).Child);
        Assert.Equal(binding, textBlock.GetBindingExpression(TextBlock.TextProperty).ParentBinding);
    }

    [StaFact]
    public void Reset_ShouldClearAllComponents()
    {
        // Arrange
        var textBlock = new TextBlock();
        var border = new Border();
        var binding = new Binding();

        _uiComponentFactoryMock.Setup(f => f.CreateTextBlock(It.IsAny<System.Windows.Media.Brush>())).Returns(textBlock);
        _uiComponentFactoryMock.Setup(f => f.CreateBorder(It.IsAny<System.Windows.Media.Brush>())).Returns(border);
        _uiComponentFactoryMock.Setup(f => f.CreateContentBinding(It.IsAny<PostViewModel>())).Returns(binding);

        var postViewModel = new PostViewModel(new Post());
        _gridCellBuilder.CreateTextBlock(System.Windows.Media.Brushes.Black)
                        .CreateBorder(System.Windows.Media.Brushes.Red)
                        .CreateBinding(postViewModel);

        // Act
        _gridCellBuilder.Reset();

        // Assert
        Assert.Throws<InvalidOperationException>(() => _gridCellBuilder.Build());
    }
}
