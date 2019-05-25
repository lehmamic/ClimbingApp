using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class Image
    {
        [Required]
        public string Base64 { get; set; }
    }
}
