using MediatR;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Exceptions;
using Stock.Domain.Specifications.Products;

namespace Stock.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        // Silme işleminde genellikle Mapper'a ihtiyaç duyulmaz
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork, ICacheService cacheService, ILogger<DeleteProductCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _logger = logger;
        }

        // Dönüş tipi Task<Unit> olmalı
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Use Specification Pattern to get the product
            var productRepo = _unitOfWork.GetRepository<Product>();
            var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken).ConfigureAwait(false);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            // Use DeleteAsync for soft delete consistency
            await productRepo.DeleteAsync(product, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Invalidate both the list cache and the specific item cache
            var listCachePrefix = "products_page";
            await _cacheService.RemoveByPrefixAsync(listCachePrefix).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", listCachePrefix);

            var cacheKey = $"products:{request.Id}";
            await _cacheService.RemoveAsync(cacheKey).ConfigureAwait(false);
            _logger.LogInformation("Cache invalidated for product with key: {CacheKey}", cacheKey);

            return Unit.Value; // Unit.Value döndürülmeli
        }
    }
} 