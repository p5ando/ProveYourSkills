using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProveYourSkills.Core.Models;
using ProveYourSkills.Infrastructure.Http;
using System.Net.Http;
using System.Text.Json;

namespace ProveYourSkills.Core.Http;

public interface IPostApiClient
{
    Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken = default);
}

public class AppSettings
{
    public string BaseUri = "https://jsonplaceholder.typicode.com";
}

public class PostApiClient : IPostApiClient
{
    private const string PostsEndpoint = "posts";

    private IRestApiClient _restApiClient;
    private ILogger<PostApiClient> _logger;
    private RestApiSettings _restSettings;

    public PostApiClient(IRestApiClient restApiClient, ILogger<PostApiClient> logger, IOptions<AppSettings> options)
    {
        _restApiClient = restApiClient;
        _logger = logger;
        _restSettings = new RestApiSettings
        {
            BaseUri = new Uri(options.Value.BaseUri),
            ContentType = "application/json",
        };
    }

    public async Task<IEnumerable<Post>> GetPosts(CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Initializing the GET HTTP reqeust towards the jsonplaceholder API");
            var posts = await _restApiClient.Get<IEnumerable<Post>>(PostsEndpoint, _restSettings, cancellationToken);
            _logger.LogInformation("GET HTTP response towards the jsonplaceholder API successfully retrieved");

            return posts ?? Enumerable.Empty<Post>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError($"Error received while collection the posts. Message: {ex.Message}.");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Error received while serializing the response. Message: {ex.Message}.");
            throw;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning($"Cancellation of the request initiated. Message: {ex.Message}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Unexpected error: {ex.Message}");
            throw;
        }

        return Enumerable.Empty<Post>();
    }
}
