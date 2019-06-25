using System;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class CreateClimbingRouteRequestMappingProfile : Profile
    {
        public CreateClimbingRouteRequestMappingProfile()
        {
            this.CreateMap<CreateClimbingRouteRequest, ClimbingRoute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.ImageUri, opt => opt.Ignore());
        }
    }
}
