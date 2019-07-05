using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class CreateClimbingRouteRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Grade { get; set; }

        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClimbingRouteType Type { get; set; }

        [Required]
        public Image Image { get; set; }
    }
}
