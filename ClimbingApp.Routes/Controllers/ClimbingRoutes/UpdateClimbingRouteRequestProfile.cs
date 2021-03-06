﻿using System;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class UpdateClimbingRouteRequestProfile : Profile
    {
        public UpdateClimbingRouteRequestProfile()
        {
            this.CreateMap<UpdateClimbingRouteRequest, ClimbingRoute>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUri, opt => opt.Ignore());
        }
    }
}
