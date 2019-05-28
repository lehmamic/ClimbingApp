using System;
using AutoMapper;
using Google.Cloud.Vision.V1;

namespace ClimbingApp.ImageRecognition.Services
{
    public class TargetSearchResultsProfile : Profile
    {
        public TargetSearchResultsProfile()
        {
            CreateMap<ProductSearchResults, TargetSearchResults>();
        }
    }
}
