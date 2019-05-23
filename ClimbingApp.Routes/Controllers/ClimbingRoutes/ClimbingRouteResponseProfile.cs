using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingRoutes
{
    public class ClimbingRouteResponseProfile : Profile
    {
        public ClimbingRouteResponseProfile()
        {
            this.CreateMap<ClimbingRoute, ClimbingRouteResponse>();
        }
    }
}
