using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class ClimbingRouteResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ClimbingRouteType Type { get; set; }

        public string ImageUri { get; set; }
    }
}
