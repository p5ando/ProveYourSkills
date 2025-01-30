using ProveYourSkills.Core.Models;

namespace ProveYourSkills.Core.Services.Abstractions;

public interface IPostApiClient
{
    Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken = default);
}
