using System.Threading.Tasks;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public interface IImageRecognitionApiClient
    {
        Task<TargetResponse> CreateTarget(string name, string description, string base64Image);

        Task<QueryResponse> Query(string base64Image);
    }
}
