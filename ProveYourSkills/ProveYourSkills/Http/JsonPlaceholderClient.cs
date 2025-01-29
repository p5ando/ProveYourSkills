using Microsoft.Extensions.Logging;
using ProveYourSkills.Http;
using ProveYourSkills.Models;
using System.Net.Http;

namespace Http;

public interface IJsonPlaceholderClient
{
    Task<IEnumerable<Post>> GetPosts();
}

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private const string PostsEndpoint = "posts";

    private IHttpClientFactory _httpClientFactory;
    private ILogger<JsonPlaceholderClient> _logger;

    public JsonPlaceholderClient(IHttpClientFactory httpClientFactory, ILogger<JsonPlaceholderClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(JsonPlaceholderClient));

        _logger.LogInformation("Initializing the GET HTTP reqeust towards the jsonplaceholder API");
        var posts = await HttpUtilities.Get<IEnumerable<Post>>(PostsEndpoint, httpClient, _logger);
        _logger.LogInformation("GET HTTP response towards the jsonplaceholder API successfully retrieved");

        return posts ?? Enumerable.Empty<Post>();
    }
}
