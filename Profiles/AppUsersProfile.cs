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
        }
    }
}