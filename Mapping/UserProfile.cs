using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Auth;

namespace DatingApp.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
