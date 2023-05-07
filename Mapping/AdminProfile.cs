using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Admin;

namespace DatingApp.Mapping
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<VipRequest, VipRequestDto>();
        }
    }
}
