using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class Image
    {
        [Required]
        public string Base64 { get; set; }
    }
}