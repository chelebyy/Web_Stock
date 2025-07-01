using AutoMapper;
using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Exceptions;
using Stock.Domain.Specifications.Products;

namespace Stock.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products:{request.Id}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                var spec = new ProductByIdWithCategorySpecification(request.Id);

                var product = await _unitOfWork.Products.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false);

                if (product == null)
                {
                    throw new NotFoundException($"Product with ID {request.Id} not found.");
                }

                var productDto = _mapper.Map<ProductDto>(product);
                
                if (product.Category != null)
                {
                    productDto.CategoryName = product.Category.Name.Value;
                }

                return productDto;

            }, TimeSpan.FromMinutes(30), cancellationToken: cancellationToken);
        }
    }
} 