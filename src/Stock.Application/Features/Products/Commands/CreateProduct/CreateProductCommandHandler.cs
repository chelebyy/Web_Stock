using AutoMapper;
using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Interfaces;
// using Stock.Application.Interfaces.Repositories; // IUnitOfWork kullanılacak
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects; // Value Objects için eklendi
using Stock.Domain.Exceptions; // DomainException için eklendi
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if category exists
            var categoryExists = await _unitOfWork.Categories.ExistsAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                // Throw a more specific exception like NotFoundException
                // throw new DomainException($"Category not found with ID: {request.CategoryId}");
                throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
            }

            // 2. Value Object'leri oluştur
            var productName = ProductName.From(request.Name);
            var productDescription = request.Description != null ? ProductDescription.From(request.Description) : null;
            var stockLevel = StockLevel.From(request.StockLevel);

            // 3. Entity'yi oluştur
            var product = Product.Create(
                productName,
                productDescription,
                stockLevel,
                request.CategoryId
            );

            // 4. Entity'yi kaydet
            var createdProduct = await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. DTO'ya dönüştür ve dön
            var productDto = _mapper.Map<ProductDto>(createdProduct);
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId);
            productDto.CategoryName = category.Name;

            return productDto;
        }
    }
} 