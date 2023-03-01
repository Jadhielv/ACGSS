using ACGSS.Domain.DTOs;
using ACGSS.Domain.Entities;
using AutoMapper;

namespace ACGSS.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
