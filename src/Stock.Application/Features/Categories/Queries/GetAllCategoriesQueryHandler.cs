using AutoMapper;
using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Interfaces; // ICategoryRepository için
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Loglama için
using Stock.Domain.Entities; // Category için

namespace Stock.Application.Features.Categories.Queries
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

        public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetAllCategoriesQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Tüm kategoriler getiriliyor.");
            // GetAllAsync metodunu kullan
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            _logger.LogInformation("{Count} adet kategori getirildi.", categoryDtos.Count());
            
            return categoryDtos;
        }
    }
} 