using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;

namespace Stock.Application.Features.Products.Services
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task UpdateProductAsync(UpdateProductDto updateProductDto);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsAsync(ISpecification<Product> spec);
    }
} 