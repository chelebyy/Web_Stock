using AutoMapper;
using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Models;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Products;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using Stock.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Stock.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// GetAllProductsQuery sorgusunu işleyen handler sınıfı.
    /// Sayfalama, filtreleme ve sıralama özellikleri desteği sağlar.
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResponse<ProductListItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cacheService, ILogger<GetAllProductsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<PagedResponse<ProductListItemDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"products_page_{request.PageNumber}_size_{request.PageSize}_sort_{request.SortBy}_{request.SortDirection}_name_{request.NameFilter}_cat_{request.CategoryId}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Cache miss for products with key: {CacheKey}. Fetching from database.", cacheKey);

                var spec = new ProductsWithCategorySpecification(
                    request.NameFilter,
                    request.CategoryId,
                    request.SortBy,
                    request.SortDirection
                );

                var countSpec = new ProductsWithCategorySpecification(
                    request.NameFilter,
                    request.CategoryId
                );

                spec.ApplyPaging((request.PageNumber - 1) * request.PageSize, request.PageSize);

                var products = await _unitOfWork.GetRepository<Product>().ListAsync(spec, cancellationToken);
                var totalRecords = await _unitOfWork.GetRepository<Product>().CountAsync(countSpec, cancellationToken);

                var productsDto = _mapper.Map<IReadOnlyList<ProductListItemDto>>(products);

                return new PagedResponse<ProductListItemDto>(productsDto, request.PageNumber, request.PageSize, totalRecords);

            }, slidingExpiration: TimeSpan.FromMinutes(5));
        }
    }
} 