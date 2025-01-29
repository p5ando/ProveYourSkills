using ProveYourSkills.Models;
using System.ComponentModel;

public class PostViewModel : INotifyPropertyChanged
{
    private string? _content;

    public Post? Post { get; private set; }

    public string? Content
    {
        get { return _content; }

        private set
        {
            if (_content != value)
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
    }

    public PostViewModel(Post post)
    {
        Post = post;
        SwitchContent(displayId: true);
    }

    public void SwitchContent(bool displayId)
    {
        if (displayId)
        {
            Content = Post?.Id.ToString();
        }
        else
        {
            Content = Post?.UserId.ToString();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
