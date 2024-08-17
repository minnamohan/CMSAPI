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

        }
      
    }
}
