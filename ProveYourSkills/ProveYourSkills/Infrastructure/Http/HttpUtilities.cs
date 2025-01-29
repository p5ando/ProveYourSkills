using System.Net.Http;
using System.Text.Json;

namespace ProveYourSkills.Infrastructure.Http
{
    public static class HttpUtilities
    {
        public static async Task<T?> GetContent<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var rawContent = await responseMessage.Content.ReadAsStringAsync();
            return DeserializeContent<T>(rawContent);
        }

        public static T? DeserializeContent<T>(string rawContent)
        {
            var content = JsonSerializer.Deserialize<T>(rawContent);

            if (content == null)
            {
                throw new JsonException($"Cound not deserialize the responce content to the indicated type {typeof(T).Name}");
            }

            return content;
        }
    }
}
