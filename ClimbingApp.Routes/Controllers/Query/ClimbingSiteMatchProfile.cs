using System;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class ClimbingSiteMatchProfile : Profile
    {
        public ClimbingSiteMatchProfile()
        {
            CreateMap<ClimbingSite, ClimbingSiteMatch>()
                .ForMember(dest => dest.Route, opt => opt.Ignore());
        }
    }
}
