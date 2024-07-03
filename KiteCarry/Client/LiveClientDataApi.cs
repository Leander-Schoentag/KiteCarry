using System.Net.Http;
using Newtonsoft.Json;

namespace KiteCarry.Client
{
    internal class LiveClientDataApi
    {
        private static readonly HttpClient _client;

        static LiveClientDataApi()
        {
            _client = new HttpClient(
                new HttpClientHandler
                {
                    // DOC: https://developer.riotgames.com/docs/lol#game-client-api_root-certificatessl-errors
                    // Bypass SSL certificate validation
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                        true
                }
            );
        }

        /// <summary>
        /// Fetches JSON data from a specified endpoint URL and deserializes it into a C# object
        /// </summary>
        public async Task<T> GetEndpointData<T>(string endpointUrl)
        {
            try
            {
                HttpResponseMessage httpResponse = await _client.GetAsync(endpointUrl);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }
                else
                {
                    return default;
                }
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
