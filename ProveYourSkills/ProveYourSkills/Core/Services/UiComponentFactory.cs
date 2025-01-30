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

    public TextBlock CreateTextBlock(Brush textBrush)
    {
        var textbox = new TextBlock();

        textbox.HorizontalAlignment = HorizontalAlignment.Center;
        textbox.VerticalAlignment = VerticalAlignment.Center;
        textbox.Foreground = textBrush;

        return textbox;
    }

    public Border CreateBorder(Brush borderBrush)
    {
        var border = new Border();
        border.BorderBrush = borderBrush;
        border.BorderThickness = new Thickness(1);
        border.Padding = new Thickness(1);
        border.Margin = new Thickness(1);

        return border;
    }
}

