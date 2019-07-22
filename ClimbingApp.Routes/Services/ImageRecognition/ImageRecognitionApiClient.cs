using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class ImageRecognitionApiClient : IImageRecognitionApiClient
    {
        private readonly HttpClient httpClient;
        private readonly IOptions<ImageRecognitionApiSettings> options;

        public ImageRecognitionApiClient(HttpClient httpClient, IOptions<ImageRecognitionApiSettings> options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<TargetResponse> CreateTarget(string name, string description, IReadOnlyDictionary<string, string> labels, string base64Image)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (base64Image == null)
            {
                throw new ArgumentNullException(nameof(base64Image));
            }

            var request = new CreateTargetRequest
            {
                DisplayName = name,
                Description = description,
                Labels = labels,
                ReferenceImage = new Image
                {
                    Base64 = base64Image,
                },
            };

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{this.options.Value.BaseUrl}/targets", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<TargetResponse>();
            }

            throw new InvalidOperationException();
        }

        public async Task<QueryResponse> Query(string base64Image)
        {
            if (base64Image == null)
            {
                throw new ArgumentNullException(nameof(base64Image));
            }

            var request = new QueryRequest
            {
                Image = new Image { Base64 = base64Image },
            };

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync($"{this.options.Value.BaseUrl}/query", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<QueryResponse>();
            }

            throw new InvalidOperationException();
        }
    }
}
