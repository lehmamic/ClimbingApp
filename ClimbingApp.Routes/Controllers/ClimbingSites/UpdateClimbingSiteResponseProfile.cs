using AutoMapper;
using ClimbingApp.Routes.Entities;

namespace ClimbingApp.Routes.Controllers.ClimbingSites
{
    public class UpdateClimbingSiteResponseProfile : Profile
    {
        public UpdateClimbingSiteResponseProfile()
        {
            this.CreateMap<UpdateClimbingSiteRequest, ClimbingSite>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Routes, opt => opt.Ignore());
        }
    }
}
