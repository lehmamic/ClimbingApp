namespace ClimbingApp.Routes.Controllers.Query
{
    public class ClimbingSiteMatch
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ClimbingRouteMatch Route { get; set; }
    }
}
