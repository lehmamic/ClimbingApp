using AutoMapper;

namespace ClimbingApp.Media.Controllers.Image
{
    public class CreateImageRequestProfile : Profile
    {
        public CreateImageRequestProfile()
        {
            CreateMap<CreateImageRequest, Entities.Image>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => $"{nameof(Image)}s/{src.Name}"));
        }
    }
}
