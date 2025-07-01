using AutoMapper;
using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Common; // Result<T> için
using Stock.Domain.Entities; // Category için
using Stock.Domain.Interfaces; // IUnitOfWork ve ICategoryRepository için
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Loglama için
using System;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Features.Categories.Commands
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        // Hem Repository hem de UnitOfWork (SaveChanges için) inject et
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;
        private readonly ICacheService _cacheService;

        public CreateCategoryCommandHandler(
            ICategoryRepository categoryRepository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<CreateCategoryCommandHandler> logger,
            ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork; // UnitOfWork'ü inject et
            _mapper = mapper;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni kategori oluşturuluyor: {CategoryName}", request.Name);

            // Category entity'sini oluşturmak için factory metodunu kullan
            var createResult = Category.Create(request.Name, request.Description);

            if (!createResult.IsSuccess) // IsFailure yerine !IsSuccess
            {
                _logger.LogWarning("Kategori oluşturma başarısız (Domain Validation): {Error}", createResult.Error);
                return Result<CategoryDto>.Failure(createResult.Error);
            }

            var newCategory = createResult.Value;

            try
            {
                // Repository'ye ekle
                await _categoryRepository.AddAsync(newCategory, cancellationToken);

                // Değişiklikleri kaydet (IUnitOfWork üzerinden)
                var saveResult = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (saveResult <= 0)
                {
                    _logger.LogError("Kategori oluşturulurken veritabanına kaydetme başarısız.");
                    return Result<CategoryDto>.Failure("Failed to save the new category.");
                }

                // Invalidate cache
                var prefix = "categories_page";
                await _cacheService.RemoveByPrefixAsync(prefix);
                _logger.LogInformation("Cache invalidated for prefix: {CachePrefix}", prefix);

                _logger.LogInformation("Kategori başarıyla oluşturuldu: ID {CategoryId}, Name: {CategoryName}", newCategory.Id, newCategory.Name.Value);

                var categoryDto = _mapper.Map<CategoryDto>(newCategory);
                return Result<CategoryDto>.Success(categoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken bir hata oluştu: {CategoryName}", request.Name);
                return Result<CategoryDto>.Failure("Failed to save the new category.");
            }
        }
    }
} 