using AutoMapper;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
            
            CreateMap<UserDto, User>();
            
            // User summary mapping
            CreateMap<User, UserSummaryDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));
            
            // Role mappings
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();
            
            // Diğer mappingler buraya eklenebilir
        }
    }
} 