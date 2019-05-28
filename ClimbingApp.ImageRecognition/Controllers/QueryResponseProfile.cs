using System;
using AutoMapper;
using ClimbingApp.ImageRecognition.Services;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class QueryResponseProfile :Profile
    {
        public QueryResponseProfile()
        {
            CreateMap<TargetSearchResults, QueryResponse>();
        }
    }
}
