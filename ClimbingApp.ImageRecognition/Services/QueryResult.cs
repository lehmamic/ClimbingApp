using System;
using System.Collections.Generic;

namespace ClimbingApp.ImageRecognition.Services
{
    public class QueryResult
    {
        public IEnumerable<QueryResultEntry> Results { get; set; }
    }
}
