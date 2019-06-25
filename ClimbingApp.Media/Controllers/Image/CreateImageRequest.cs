using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Media.Controllers.Image
{
    public class CreateImageRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Base64 { get; set; }
    }
}
