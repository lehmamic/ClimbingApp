using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class QueryRequest
    {
        [Required]
        public Image Image { get; set; }
    }
}
