using AutoMapper;
using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Application.Models;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Categories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using System;

namespace Stock.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, PagedResponse<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllCategoriesQueryHandler> logger, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<PagedResponse<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"categories_page_{request.PageNumber}_size_{request.PageSize}_sort_{request.SortField}_{request.SortOrder}_name_{request.Name}";

            return await _cacheService.GetOrCreateAsync(cacheKey, async () =>
            {
                _logger.LogInformation("Cache miss for categories with key: {CacheKey}. Fetching from database.", cacheKey);

                var countSpec = new CategoriesSpecification(request.Name);
                var pagedSpec = new CategoriesSpecification(
                    request.Name,
                    request.SortField,
                    request.SortOrder,
                    request.PageNumber,
                    request.PageSize);

                var categoryRepo = _unitOfWork.GetRepository<Category>();

                var totalRecords = await categoryRepo.CountAsync(countSpec, cancellationToken);
                var categories = await categoryRepo.ListAsync(pagedSpec, cancellationToken);
                var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

                return new PagedResponse<CategoryDto>(categoryDtos, request.PageNumber, request.PageSize, totalRecords);

            }, slidingExpiration: TimeSpan.FromMinutes(10));
        }
    }
} 