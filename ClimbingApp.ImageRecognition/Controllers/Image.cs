using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class Image
    {
        [Required]
        public string Base64 { get; set; }
    }
}
