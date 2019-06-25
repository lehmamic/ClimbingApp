using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.Query
{
    public class ClimbingRouteMatchProfile : Profile
    {
        public ClimbingRouteMatchProfile()
        {
            CreateMap<ClimbingRoute, ClimbingRouteMatch>()
                .ForMember(dest => dest.Site, opt => opt.Ignore()); ;
        }
    }
}
