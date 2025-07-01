using AutoMapper;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Common.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
            // Başka eşlemeler gerekirse buraya eklenebilir
        }
    }
} 