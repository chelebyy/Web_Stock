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
            .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.StockLevel.Value))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty));

        CreateMap<CreateProductDto, Product>()
            .ConstructUsing((src, ctx) => Product.Create(
                ProductName.From(src.Name),
                ProductDescription.From(src.Description),
                StockLevel.From(src.StockLevel),
                src.CategoryId));

        CreateMap<UpdateProductDto, Product>()
            .ConstructUsing((src, ctx) => Product.Create(
                ProductName.From(src.Name),
                ProductDescription.From(src.Description),
                StockLevel.From(src.StockLevel),
                src.CategoryId))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
} 