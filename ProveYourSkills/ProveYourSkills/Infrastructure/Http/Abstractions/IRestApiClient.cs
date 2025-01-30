using ProveYourSkills.Infrastructure.Http.Settings;

namespace ProveYourSkills.Infrastructure.Http.Abstractions;

public interface IRestApiClient
{
    public Task<T?> GetAsync<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default);
}
