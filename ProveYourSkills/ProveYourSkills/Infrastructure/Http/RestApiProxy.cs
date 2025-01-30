using ProveYourSkills.Infrastructure.Http.Abstractions;
using ProveYourSkills.Infrastructure.Http.Settings;
using System.Net.Http;

namespace ProveYourSkills.Infrastructure.Http;

/// <summary>
/// The purpose of this class is only to show the usage of cancelation tokens as these are configured to
/// cancel the request if it lasts for 5 or more seconds. This proxy will generate a random value and if 
/// it is in a specific range if will delay execution for 10s which will trigger cancellation of the token.
/// There is 25% chances to trigger this behaivor 
/// </summary>
public class RestApiProxy : IRestApiClient
{
    private IRestApiClient restApiClient;

    public RestApiProxy(IHttpClientFactory httpClientFactory)
    {
        restApiClient = new RestApiClient(httpClientFactory);
    }

    public async Task<T?> GetAsync<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default)
    {
        var random = new Random();
        var randomValue = random.Next(1, 100);

        if (randomValue >= 75)
        {
            await Task.Delay(10000);
        }

        return await restApiClient.GetAsync<T>(endpoint, settings, cancellationToken);
    }
}