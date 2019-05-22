using System.Collections.Generic;

namespace ClimbingApp.Routes.Entities
{
    public class ClimbingSite
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<ClimbingRoute> Routes { get; set; }
    }
}
