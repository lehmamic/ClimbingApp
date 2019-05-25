using ClimbingApp.Routes.Controllers.ClimbingRoutes;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class ClimbingRouteMatch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public ClimbingRouteType Type { get; set; }
    }
}
