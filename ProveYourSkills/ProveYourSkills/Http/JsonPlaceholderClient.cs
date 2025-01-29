using ProveYourSkills.Models;
using System.Net.Http;
using System.Text.Json;

public interface IJsonPlaceholderClient
{
    Task<IEnumerable<Post>> GetPosts();
}

public class JsonPlaceholderClient : IJsonPlaceholderClient
{
    private IHttpClientFactory _httpClientFactory;

    public JsonPlaceholderClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IEnumerable<Post>> GetPosts()
    {
        var posts = await Get<IEnumerable<Post>>("posts");

        return posts ?? Enumerable.Empty<Post>();
    }

    private async Task<T?> Get<T>(string endpoint)
    {
        var httpClient = _httpClientFactory.CreateClient(nameof(JsonPlaceholderClient));
        var responseMessage = await httpClient.GetAsync(endpoint);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve the data. Status code: {responseMessage.StatusCode}");
        }

        return await GetContent<T>(responseMessage);
    }

    private async Task<T?> GetContent<T>(HttpResponseMessage responseMessage)
    {
        var rawContent = await responseMessage.Content.ReadAsStringAsync();
        return DeserializeContent<T>(rawContent);
    }

    private T? DeserializeContent<T>(string rawContent)
    {
        var content = JsonSerializer.Deserialize<T>(rawContent);

        if (content == null)
        {
            return default;
        }

        return content;
    }
}