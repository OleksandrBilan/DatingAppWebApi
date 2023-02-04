using AutoMapper;
using DatingApp.DB.Models;
using DatingApp.DTOs;

namespace DatingApp.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.SexPreferences, opt => opt.MapFrom<SexPreferencesResolver>());
        }
    }

    public class SexPreferencesResolver : IValueResolver<RegisterDto, User, byte>
    {
        public byte Resolve(RegisterDto dto, User user, byte value, ResolutionContext context)
        {
            byte sum = 0;
            foreach (var sex in dto.SexPreferences)
                sum += (byte)sex;
            return sum;
        }
    }
}
