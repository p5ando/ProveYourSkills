using ProveYourSkills.UI.ViewModel;
using System.Windows;

namespace ProveYourSkills.Core.Services.Abstractions;

public interface IGridCellBuilder
{
    IGridCellBuilder Reset();
    IGridCellBuilder CreateBinding(PostViewModel post);
    IGridCellBuilder CreateTextBlock();
    IGridCellBuilder CreateBorder();
    FrameworkElement Build();
}
