using System;
using AutoMapper;

namespace ClimbingApp.ImageRecognition.Services
{
    public class ReferenceImageProfile : Profile
    {
        public ReferenceImageProfile()
        {
            CreateMap<Google.Cloud.Vision.V1.ReferenceImage, ReferenceImage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ReferenceImageName.ReferenceImageId));
        }
    }
}
