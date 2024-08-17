using AutoMapper;
using CMSAPI.Models.DTOs;
using CMSAPI.Models.Entities;
namespace CMSAPI.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping configuration between Customer and CustomerDto
            CreateMap<Customer, CustomerDto>().ReverseMap();
            // Mapping configuration between User and UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ReverseMap();
        }
      
    }
}
