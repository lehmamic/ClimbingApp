namespace ClimbingApp.Routes.Entities
{
    public class ClimbingRoute
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }

        public ClimbingRouteType Type { get; set; }

        public string ImageUri { get; set; }
    }
}
