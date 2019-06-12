using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class QueryResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public QueryResultType Result { get; set; }

        public ClimbingSiteMatch ClimbingSite { get; set; }
    }
}
