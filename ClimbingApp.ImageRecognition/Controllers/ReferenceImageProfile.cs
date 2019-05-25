using System;
using AutoMapper;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class ReferenceImageProfile : Profile
    {
        public ReferenceImageProfile()
        {
            CreateMap<ClimbingApp.ImageRecognition.Services.ReferenceImage, ReferenceImage>();
        }
    }
}
