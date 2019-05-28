using AutoMapper;
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;

namespace ClimbingApp.ImageRecognition.Services
{
    public class TargetSearchResultEntryProfile : Profile
    {
        public TargetSearchResultEntryProfile()
        {
            CreateMap<Result, TargetSearchResultEntry>()
                .ForMember(dest => dest.Target, opt => opt.MapFrom(src => src.Product));
        }
    }
}
