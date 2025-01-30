using ProveYourSkills.Infrastructure.Http.Abstractions;
using ProveYourSkills.Infrastructure.Http.Settings;
using System.Net.Http;

namespace ProveYourSkills.Infrastructure.Http;

public class RestApiClient : IRestApiClient
{
    private IHttpClientFactory _httpClientFactory;

    public RestApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T?> GetAsync<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = settings.BaseUri;
        httpClient.DefaultRequestHeaders.Add("Accept", settings.ContentType);

        var responseMessage = await httpClient.GetAsync(endpoint);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to retrieve the data. Status code: {responseMessage.StatusCode}");
        }

        return await HttpUtilities.GetContentAsync<T>(responseMessage, cancellationToken);
    }
}
