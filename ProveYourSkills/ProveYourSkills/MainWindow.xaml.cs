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
        public MainWindow(PostGridViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            this.Loaded += async (s, e) => await InitializeData(viewModel);
        }

        public async Task InitializeData(PostGridViewModel viewModel)
        {
            await viewModel.InitializePosts();

            foreach(var post in viewModel.PostCells!)
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
            textbox.Foreground = new SolidColorBrush(Color.FromRgb(174, 68, 90));

            return textbox;
        }

        private Border CreateBorder()
        {
            var border = new Border();
            border.BorderBrush = new SolidColorBrush(Color.FromRgb(232, 188, 185));
            border.BorderThickness = new Thickness(1);
            border.Padding = new Thickness(1);
            border.Margin = new Thickness(1);

            return border;
        }
    }
}