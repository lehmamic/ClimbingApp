using AutoMapper;
using Google.Cloud.Vision.V1;

namespace ClimbingApp.ImageRecognition.Services
{
    public class TargetProfile : Profile
    {
        public TargetProfile()
        {
            CreateMap<Product, Target>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ProductName.ProductId))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ReferenceImages, opt => opt.Ignore());
        }
    }
}
