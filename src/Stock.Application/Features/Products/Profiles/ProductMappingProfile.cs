using AutoMapper;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Features.Products.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.StockLevel, opt => opt.MapFrom(src => src.StockLevel.Value))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
            
            CreateMap<Product, ProductSummaryDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.StockLevel, opt => opt.MapFrom(src => src.StockLevel.Value))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));
        }
    }
} 