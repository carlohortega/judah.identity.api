using AutoMapper;
using Eis.Identity.Api.Dtos;
using Eis.Identity.Api.Models;

namespace Eis.Identity.Api.Profiles
{
    public class AppUsersProfile : Profile
    {
        public AppUsersProfile()
        {
            // Source -> Target
            CreateMap<AppUser, AppUserReadDto>();
            CreateMap<AppUserCreateDto, AppUser>();
            CreateMap<AppUserReadDto, AppUserPublishedDto>();
            CreateMap<AppUser, GrpcIdentityModel>()
            .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ObjectId, opt => opt.MapFrom(src => src.ObjectId));
        }
    }
}