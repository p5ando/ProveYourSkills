using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.UI.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ProveYourSkills.Core.Services;

public class UiComponentFactory : IUiComponentFactory
{
    public Binding CreateContentBinding(PostViewModel post)
    {
        var contentBinding = new Binding("Content")
        {
            Source = post,
            Mode = BindingMode.OneWay
        };

        return contentBinding;
    }

    public TextBlock CreateTextBlock()
    {
        var textbox = new TextBlock();

        textbox.HorizontalAlignment = HorizontalAlignment.Center;
        textbox.VerticalAlignment = VerticalAlignment.Center;
        // colors should be defined in resource definitions or configuration
        textbox.Foreground = new SolidColorBrush(Color.FromRgb(174, 68, 90));

        return textbox;
    }

    public Border CreateBorder()
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

