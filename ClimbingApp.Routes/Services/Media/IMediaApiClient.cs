using System.Threading.Tasks;

namespace ClimbingApp.Routes.Services.Media
{
    public interface IMediaApiClient
    {
        Task UploadImage(string name, string base64);
    }
}