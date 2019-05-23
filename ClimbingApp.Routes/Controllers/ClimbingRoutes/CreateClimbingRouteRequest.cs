using System.ComponentModel.DataAnnotations;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class CreateClimbingRouteRequest
    {
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; }

        public string Description { get; set; }

        [RegularExpression("[0-9][abcABC]")]
        public string Grade { get; set; }

        [Required]
        public ClimbingRoutType Type { get; set; }

        public Image Image { get; set; }
    }
}
