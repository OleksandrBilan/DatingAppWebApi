using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Auth;
using DatingApp.DTOs.Recommendations;
using DatingApp.DTOs.User;

namespace DatingApp.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Tuple<User, double>, RecommendedUserDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.SimilarityScore, opt => opt.MapFrom(src => src.Item2));
        }
    }
}
