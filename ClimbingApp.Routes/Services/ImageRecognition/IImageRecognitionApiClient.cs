using System.Threading.Tasks;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public interface IImageRecognitionApiClient
    {
        Task<QueryResponse> Query(string base64Image);
    }
}
