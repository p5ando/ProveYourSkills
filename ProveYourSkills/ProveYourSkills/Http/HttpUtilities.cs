using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;

namespace ProveYourSkills.Http
{
    public static class HttpUtilities
    {
        public static async Task<T?> Get<T>(string endpoint, HttpClient httpClient, ILogger logger)
        {
            logger.LogInformation("Sending Get HTTP request");
            var responseMessage = await httpClient.GetAsync(endpoint);

            if (!responseMessage.IsSuccessStatusCode)
            {
                logger.LogError($"HTTP GET response did not indicated success. Error code: {responseMessage.StatusCode}");
                throw new Exception($"Failed to retrieve the data. Status code: {responseMessage.StatusCode}");
            }

            return await GetContent<T>(responseMessage, logger);
        }

        private static async Task<T?> GetContent<T>(HttpResponseMessage responseMessage, ILogger logger)
        {
            logger.LogInformation("Reading the response content");
            var rawContent = await responseMessage.Content.ReadAsStringAsync();
            return DeserializeContent<T>(rawContent, logger);
        }

        private static T? DeserializeContent<T>(string rawContent, ILogger logger)
        {
            var content = JsonSerializer.Deserialize<T>(rawContent);

            if (content == null)
            {
                logger.LogWarning($"Cound not deserialize the responce content to the indicated type {typeof(T).Name}");
                return default;
            }

            return content;
        }
    }
}
