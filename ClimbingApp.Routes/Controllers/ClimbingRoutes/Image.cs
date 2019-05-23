using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class Image
    {
        [Required]
        public string Base64 { get; set; }
    }
}