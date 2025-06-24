using AutoMapper;
using Stock.Application.Features.Categories.DTOs;
using Stock.Application.Features.Products.DTOs;
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
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

            CreateMap<User, UserListItemDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

            // Role mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

            CreateMap<Role, RoleSlimDto>();

            // Category mappings
            CreateMap<Category, CategoryDto>();
        }
    }
} 