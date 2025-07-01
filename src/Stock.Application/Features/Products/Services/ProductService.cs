using AutoMapper;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Products;
using Stock.Domain.ValueObjects;
using Stock.Domain.Common;
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
        var productName = ProductName.From(dto.Name);
        var productDescription = ProductDescription.From(dto.Description);

        var stockLevelResult = StockLevel.From(dto.StockLevel);
        if (!stockLevelResult.IsSuccess)
        {
            throw new ArgumentException(stockLevelResult.Error);
        }

        var productResult = Product.Create(
            productName,
            productDescription,
            stockLevelResult.Value,
            dto.CategoryId);

        if (!productResult.IsSuccess)
        {
            throw new ArgumentException(productResult.Error);
        }

        var product = productResult.Value;
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

        var productName = ProductName.From(dto.Name);
        var productDescription = ProductDescription.From(dto.Description);

        var updateNameResult = product.UpdateName(productName);
        if (!updateNameResult.IsSuccess)
        {
            throw new ArgumentException(updateNameResult.Error);
        }

        var updateDescResult = product.UpdateDescription(productDescription);
        if (!updateDescResult.IsSuccess)
        {
            throw new ArgumentException(updateDescResult.Error);
        }
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
