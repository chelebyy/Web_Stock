using AutoMapper;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Products;
using Stock.Domain.ValueObjects;
using System.Threading;
using Stock.Domain.Specifications;

namespace Stock.Application.Features.Products.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var spec = new ProductByIdWithCategorySpecification(id);
        var product = await _unitOfWork.Products.FirstOrDefaultAsync(spec);
        
        if (product == null)
        {
            return null;
        }

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var spec = new ProductsWithCategorySpecification();
        var products = await _unitOfWork.Products.ListAsync(spec);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
    {
        var product = Product.Create(
            ProductName.From(dto.Name),
            ProductDescription.From(dto.Description),
            StockLevel.From(dto.StockLevel),
            dto.CategoryId);

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task UpdateProductAsync(UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(dto.Id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {dto.Id} not found");
        }

        product.UpdateName(ProductName.From(dto.Name));
        // Allow null description by passing dto.Description directly
        product.UpdateDescription(ProductDescription.From(dto.Description));
        int currentStock = product.StockLevel.Value;
        int difference = dto.StockLevel - currentStock;
        if (difference > 0) product.IncreaseStock(difference);
        else if (difference < 0) product.DecreaseStock(Math.Abs(difference));
        product.ChangeCategory(dto.CategoryId);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with id {id} not found");
        }

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(ISpecification<Product> spec)
    {
        var products = await _unitOfWork.Products.ListAsync(spec);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
