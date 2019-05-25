using System;
using AutoMapper;
using ClimbingApp.ImageRecognition.Services;

namespace ClimbingApp.ImageRecognition.Controllers
{
    public class TargetResponseProfile : Profile
    {
        public TargetResponseProfile()
        {
            CreateMap<Target, TargetResponse>();
        }
    }
}
