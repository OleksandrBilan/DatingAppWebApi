using AutoMapper;
using DatingApp.DB.Models;
using DatingApp.DTOs;

namespace DatingApp.Mapping
{
    public class LookupProfile : Profile
    {
        public LookupProfile()
        {
            CreateMap<Sex, SexDto>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<CountryDto, Country>().ReverseMap();
        }
    }
}
