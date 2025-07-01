using AutoMapper;
using Stock.Application.Features.Categories.DTOs;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;

namespace Stock.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName.ToString()))
                .ForMember(dest => dest.Adi, opt => opt.MapFrom(src => src.FullName.Adi))
                .ForMember(dest => dest.Soyadi, opt => opt.MapFrom(src => src.FullName.Soyadi))
                .ForMember(dest => dest.Sicil, opt => opt.MapFrom(src => src.Sicil.Value))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name.Value : null));

            CreateMap<User, UserListItemDto>()
                .ForMember(dest => dest.Adi, opt => opt.MapFrom(src => src.FullName.Adi))
                .ForMember(dest => dest.Soyadi, opt => opt.MapFrom(src => src.FullName.Soyadi))
                .ForMember(dest => dest.Sicil, opt => opt.MapFrom(src => src.Sicil.Value))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name.Value : null));

            // Role mappings
            CreateMap<Role, RoleSlimDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

            // Category mappings
            CreateMap<Category, CategoryDto>();

            // Permission mappings
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        }
    }
} 