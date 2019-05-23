using System;
namespace ClimbingApp.Routes.Controllers
{
    public class UpdateClimbingRouteRequest
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
