using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ClimbingApp.Routes.Services.Media
{
    public class MediaApiClient : IMediaApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<MediaApiSettings> options;

        public MediaApiClient(HttpClient httpClient, IOptions<MediaApiSettings> options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
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

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{this.options.Value.BaseUrl}/images", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
