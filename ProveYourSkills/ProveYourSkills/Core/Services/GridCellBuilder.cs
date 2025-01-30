using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.UI.ViewModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace ProveYourSkills.Core.Services;

public class GridCellBuilder : IGridCellBuilder
{
    TextBlock? _textBlock;
    Border? _border;
    Binding? _binding;

    private IUiComponentFactory _uiComponentFactoryService;

    public GridCellBuilder(IUiComponentFactory uiComponentFactoryService)
    {
        _uiComponentFactoryService = uiComponentFactoryService;
    }

    public FrameworkElement Build()
    {
        if (_textBlock == null || _border == null || _binding == null)
        {
            throw new InvalidOperationException("The builder is not fully configured");
        }

        _textBlock.SetBinding(TextBlock.TextProperty, _binding);
        _border.Child = _textBlock;
        return _border;
    }

    public IGridCellBuilder CreateBinding(PostViewModel post)
    {
        _binding = _uiComponentFactoryService.CreateContentBinding(post);
        return this;
    }

    public IGridCellBuilder CreateBorder()
    {
        _border = _uiComponentFactoryService.CreateBorder();
        return this;
    }

    public IGridCellBuilder CreateTextBlock()
    {
        _textBlock = _uiComponentFactoryService.CreateTextBlock();
        return this;
    }

    public IGridCellBuilder Reset()
    {
        _textBlock = null;
        _border = null;
        _binding = null;
        return this;
    }
}
