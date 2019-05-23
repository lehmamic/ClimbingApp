using System;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers
{
    public class UpdateClimbingRouteRequestProfile : Profile
    {
        public UpdateClimbingRouteRequestProfile()
        {
            this.CreateMap<UpdateClimbingRouteRequest, ClimbingRoute>()
                .ForMember(dest => dest.Id, opt => opt.UseDestinationValue());
        }
    }
}
