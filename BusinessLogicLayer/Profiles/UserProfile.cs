using AuthenticationAPI.Models;
using AutoMapper;
using DAL.Entities;
using Service.Dtos;

namespace Service.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, RegisterModel>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash)).ReverseMap();
        }
    }
}
