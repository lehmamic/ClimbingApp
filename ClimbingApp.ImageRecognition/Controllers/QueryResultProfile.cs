using System;
using AutoMapper;
using ClimbingApp.ImageRecognition.Services;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class QueryResultProfile : Profile
    {
        public QueryResultProfile()
        {
            CreateMap<TargetSearchResultEntry, QueryResult>();
        }
    }
}
