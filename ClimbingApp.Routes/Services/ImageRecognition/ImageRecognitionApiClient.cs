using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class ImageRecognitionApiClient : IImageRecognitionApiClient
    {
        private readonly HttpClient httpClient;

        public ImageRecognitionApiClient(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<TargetResponse> CreateTarget(string name, string description, string base64Image)
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
                ReferenceImage = new Image
                {
                    Base64 = base64Image,
                },
            };

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("http://localhost:5001/api/v1/targets", request);
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

            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync("https://localhost:5001/api/v1/query", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<QueryResponse>();
            }

            throw new InvalidOperationException();
        }
    }
}
