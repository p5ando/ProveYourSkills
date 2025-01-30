using ProveYourSkills.UI.ViewModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProveYourSkills.Core.Services.Abstractions;
public interface IUiComponentFactory
{
    TextBlock CreateTextBlock();
    Border CreateBorder();
    Binding CreateContentBinding(PostViewModel post);
}
