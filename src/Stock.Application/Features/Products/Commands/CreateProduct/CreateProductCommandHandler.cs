using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Common;
using Stock.Domain.Entities;
using Stock.Domain.Exceptions;
using Stock.Domain.Interfaces;
using Stock.Domain.ValueObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly ICacheService _cacheService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateProductCommandHandler> logger, IBackgroundTaskQueue backgroundTaskQueue, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
            _cacheService = cacheService;
        }

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // TODO: Mikroservis Hazırlık Adımı: Kategori varlık kontrolü kaldırıldı.
            // Bu kontrol, gelecekte "Product" ve "Category" servisleri ayrıldığında,
            // olay tabanlı bir yaklaşımla (event-driven) veya API Gateway seviyesinde yapılmalıdır.
            // Örneğin, bir "ProductCreated" olayı fırlatılabilir ve "Category" servisi bu olayı dinleyerek
            // kendi tarafında gerekli güncellemeleri (örneğin kategoriye ait ürün sayısını artırma) yapabilir.
            // Şimdilik, kategori ID'sinin geçerli olduğu varsayılmaktadır.
            /*
            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, cancellationToken).ConfigureAwait(false);
            if (category == null)
            {
                return Result<ProductDto>.Failure(DomainErrors.Category.NotFound(request.CategoryId));
            }
            */

            var productName = ProductName.From(request.Name);
            var productDescription = ProductDescription.From(request.Description);

            var stockLevelResult = StockLevel.From(request.StockLevel);
            if (!stockLevelResult.IsSuccess) return Result<ProductDto>.Failure(stockLevelResult.Error);

            var productResult = Product.Create(
                productName,
                productDescription,
                stockLevelResult.Value,
                request.CategoryId);
                
            if (!productResult.IsSuccess)
            {
                return Result<ProductDto>.Failure(productResult.Error);
            }

            var product = productResult.Value;

            await _unitOfWork.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Invalidate cache
            var cachePrefix = "products_page";
            await _cacheService.RemoveByPrefixAsync(cachePrefix).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", cachePrefix);
            
            // SIMULATE EVENT: ProductCreated
            _logger.LogInformation("EVENT_FIRED: ProductCreated - ProductId: {ProductId}, CategoryId: {CategoryId}", product.Id, product.CategoryId);
            
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                _logger.LogInformation("BACKGROUND JOB: Yeni ürün eklendi: {ProductName} (ID: {ProductId}). İlgili sistemlere bildirim gönderiliyor...", product.Name.Value, product.Id);
                await Task.Delay(TimeSpan.FromSeconds(5), token).ConfigureAwait(false);
                _logger.LogInformation("BACKGROUND JOB: Bildirim görevi tamamlandı: {ProductName} (ID: {ProductId}).", product.Name.Value, product.Id);
            });

            var productDto = _mapper.Map<ProductDto>(product);
            // productDto.CategoryName = category.Name.Value; // Bu bilgi artık doğrudan alınamıyor. API Gateway veya başka bir mekanizma ile zenginleştirilebilir.

            return Result<ProductDto>.Success(productDto);
        }
    }
} 