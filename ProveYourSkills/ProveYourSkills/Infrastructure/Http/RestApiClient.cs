using System.Net.Http;

namespace ProveYourSkills.Infrastructure.Http
{
    public interface IRestApiClient
    {
        public Task<T?> Get<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default);
    }

    public class RestApiSettings
    {
        public required Uri BaseUri { get; set; }
        public required string ContentType { get; set; }
    }

    public class RestApiClient : IRestApiClient
    {
        private IHttpClientFactory _httpClientFactory;

        public RestApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T?> Get<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default)
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

            return await HttpUtilities.GetContent<T>(responseMessage, cancellationToken);
        }
    }

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

        public async Task<T?> Get<T>(string endpoint, RestApiSettings settings, CancellationToken cancellationToken = default)
        {
            var random = new Random();
            var randomValue = random.Next(1, 100);

            if(randomValue >= 75)
            {
                await Task.Delay(10000);
            }

            return await restApiClient.Get<T>(endpoint, settings, cancellationToken);
        }
    }
}
