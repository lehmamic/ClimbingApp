using ClimbingApp.Routes.Controllers.ClimbingRoutes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class ClimbingRouteMatch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public string ImageUri { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ClimbingRouteType Type { get; set; }

        public ClimbingSiteMatch Site { get; set; }
    }
}
