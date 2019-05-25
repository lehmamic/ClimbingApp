using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class CreateClimbingRouteRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [RegularExpression("[0-9][abcABC]")]
        public string Grade { get; set; }

        [Required]
        public ClimbingRouteType Type { get; set; }

        public Image Image { get; set; }
    }
}
