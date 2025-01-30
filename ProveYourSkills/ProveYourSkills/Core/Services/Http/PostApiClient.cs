using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProveYourSkills.Core.Models;
using ProveYourSkills.Core.Services.Abstractions;
using ProveYourSkills.Infrastructure.Configuration;
using ProveYourSkills.Infrastructure.Http.Abstractions;
using ProveYourSkills.Infrastructure.Http.Settings;
using System.Net.Http;
using System.Text.Json;

namespace ProveYourSkills.Core.Services;

public class PostApiClient : IPostApiClient
{
    private const string PostsEndpoint = "posts";

    private IRestApiClient _restApiClient;
    private ILogger<PostApiClient> _logger;
    private RestApiSettings _restSettings;

    public PostApiClient(
        IRestApiClient restApiClient,
        ILogger<PostApiClient> logger,
        IOptionsSnapshot<AppSettings> options)
    {
        _restApiClient = restApiClient;
        _logger = logger;
        _restSettings = new RestApiSettings
        {
            BaseUri = new Uri(options.Value.BaseUri),
            ContentType = "application/json",
        };
    }

    public async Task<IEnumerable<Post>> GetPostsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation("Initializing the GET HTTP reqeust towards the jsonplaceholder API");
            var posts = await _restApiClient.GetAsync<IEnumerable<Post>>(PostsEndpoint, _restSettings, cancellationToken);
            _logger.LogInformation("GET HTTP response towards the jsonplaceholder API successfully retrieved");

            return posts ?? Enumerable.Empty<Post>();
        }
        catch (HttpRequestException ex)
        {
            var msg = $"HTTP error received while fetching the posts. Message: {ex.Message}";
            _logger.LogError(msg);
            throw new PostApiException(msg, ex);
        }
        catch (JsonException ex)
        {
            var msf = $"Error received while deserializing the response. Message: {ex.Message}.";
            _logger.LogError(msf);
            throw new PostApiException(msf, ex);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning($"Cancellation of the request initiated. Message: {ex.Message}.");
            // just ignore this request as it was cancelled
        }
        catch (Exception ex)
        {
            var msg = $"Unexpected error occurred while fetching the posts. Exception message: {ex.Message}";
            _logger.LogError(msg);
            throw new PostApiException(msg, ex);
        }

        return Enumerable.Empty<Post>();
    }
}


public class PostApiException : Exception
{
    public PostApiException(string message, Exception inner) : base(message, inner)
    {
    }
}
