using AutoMapper;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;

namespace Stock.Application.Common.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
            .ForMember(dest => dest.StockLevel, opt => opt.MapFrom(src => src.StockLevel.Value))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        // CreateProductDto ve UpdateProductDto için mapping'ler service layer'da manuel yapılacak
        // çünkü Product.Create() Result<Product> döndürüyor
    }
} 