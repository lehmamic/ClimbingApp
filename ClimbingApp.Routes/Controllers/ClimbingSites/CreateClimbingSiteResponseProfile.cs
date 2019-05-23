using System;
using System.Collections.ObjectModel;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingSites
{
    public class CreateClimbingSiteResponseProfile : Profile
    {
        public CreateClimbingSiteResponseProfile()
        {
            this.CreateMap<CreateClimbingSiteRequest, ClimbingSite>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => new Collection<ClimbingRoute>()));
        }
    }
}
