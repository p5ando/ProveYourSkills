using ProveYourSkills.UI.ViewModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProveYourSkills.Core.Services.Abstractions;
public interface IUiComponentFactory
{
    TextBlock CreateTextBlock(System.Windows.Media.Brush textBrush);
    Border CreateBorder(System.Windows.Media.Brush borderBrush);
    Binding CreateContentBinding(PostViewModel post);
}
