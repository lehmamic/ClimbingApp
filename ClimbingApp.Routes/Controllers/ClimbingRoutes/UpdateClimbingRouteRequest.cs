using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class UpdateClimbingRouteRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [RegularExpression("[0-9][abcABC]")]
        public string Grade { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClimbingRouteType Type { get; set; }

        public Image Image { get; set; }
    }
}
