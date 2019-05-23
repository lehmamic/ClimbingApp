namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class ClimbingRouteResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public ClimbingRoutType Type { get; set; }
    }
}
