using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClimbingApp.Routes.Services.Media
{
    public class MediaApiClient : IMediaApiClient
    {
        private readonly HttpClient httpClient;

        public MediaApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task UploadImage(string name, string base64)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (base64 == null)
            {
                throw new ArgumentNullException(nameof(base64));
            }

            var request = new CreateImageRequest
            {
                Name = name,
                Base64 = base64,
            };

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("http://localhost:5003/api/v1/images", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
