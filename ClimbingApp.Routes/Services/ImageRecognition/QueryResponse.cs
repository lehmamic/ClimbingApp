using System.Collections.Generic;

namespace ClimbingApp.Routes.Services.ImageRecognition
{
    public class QueryResponse
    {
        public IEnumerable<QueryResult> Results { get; set; }
    }
}
