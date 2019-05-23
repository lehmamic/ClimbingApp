using System;
using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingSites
{
    public class ClimbingSiteResponseProfile : Profile
    {
        public ClimbingSiteResponseProfile()
        {
            this.CreateMap<ClimbingSite, ClimbingSiteResponse>();
        }
    }
}
