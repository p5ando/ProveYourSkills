using ProveYourSkills.Core.Models;
using ProveYourSkills.Core.Services;
using ProveYourSkills.UI.ViewModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ProveYourSkills.Tests.Core.Services
{
    public class UiComponentFactoryTests
    {
        private readonly UiComponentFactory _factory;

        public UiComponentFactoryTests()
        {
            _factory = new UiComponentFactory();
        }

        [Fact]
        public void CreateContentBinding_ShouldReturnBindingWithCorrectProperties()
        {
            // Arrange
            var postViewModel = new PostViewModel(new Post { Id = 1, UserId = 1, Title = "Test Post", Body = "Test Body" });

            // Act
            var binding = _factory.CreateContentBinding(postViewModel);

            // Assert
            Assert.NotNull(binding);
            Assert.Equal("Content", binding.Path.Path);
            Assert.Equal(postViewModel, binding.Source);
            Assert.Equal(BindingMode.OneWay, binding.Mode);
        }

        [StaFact]
        public void CreateTextBlock_ShouldReturnTextBlockWithCorrectProperties()
        {
            // Arrange
            var textBrush = new SolidColorBrush(Colors.Red);

            // Act
            var textBlock = _factory.CreateTextBlock(textBrush);

            // Assert
            Assert.NotNull(textBlock);
            Assert.Equal(HorizontalAlignment.Center, textBlock.HorizontalAlignment);
            Assert.Equal(VerticalAlignment.Center, textBlock.VerticalAlignment);
            Assert.Equal(textBrush, textBlock.Foreground);
        }

        [StaFact]
        public void CreateBorder_ShouldReturnBorderWithCorrectProperties()
        {
            // Arrange
            var borderBrush = new SolidColorBrush(Colors.Blue);

            // Act
            var border = _factory.CreateBorder(borderBrush);

            // Assert
            Assert.NotNull(border);
            Assert.Equal(borderBrush, border.BorderBrush);
            Assert.Equal(new Thickness(1), border.BorderThickness);
            Assert.Equal(new Thickness(1), border.Padding);
            Assert.Equal(new Thickness(1), border.Margin);
        }
    }
}