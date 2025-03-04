using AutoMapper;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
                .ForMember(dest => dest.Sicil, opt => opt.MapFrom(src => src.Sicil));

            // Role mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));
        }
    }
} 