using System;
using System.Collections.Generic;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class QueryResponse
    {
        public IEnumerable<QueryResult> Results { get; set; }
    }
}
