using AutoMapper;
using DatingApp.DB.Models.Chats;
using DatingApp.DB.Models.Recommendations;
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

            CreateMap<RecommendedUser, RecommendedUserDto>();

            CreateMap<UsersChat, UsersChatDto>();

            CreateMap<Tuple<UsersChat, int>, UsersChatDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item1.Id))
                .ForMember(dest => dest.User1, opt => opt.MapFrom(src => src.Item1.User1))
                .ForMember(dest => dest.User2, opt => opt.MapFrom(src => src.Item1.User2))
                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.Item1.CreatedDateTime))
                .ForMember(dest => dest.UnreadMessagesCount, opt => opt.MapFrom(src => src.Item2));

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.Name));
        }
    }
}
