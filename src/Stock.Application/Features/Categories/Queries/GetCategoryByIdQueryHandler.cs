using AutoMapper;
using MediatR;
using Stock.Application.Features.Categories.DTOs;
using Stock.Domain.Entities; // Category için
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Categories; // CategoryByIdSpecification için
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging; // Loglama için

namespace Stock.Application.Features.Categories.Queries
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ID: {CategoryId} olan kategori getiriliyor.", request.Id);
            
            var spec = new CategoryByIdSpecification(request.Id);
            var category = await _unitOfWork.GetRepository<Category>().FirstOrDefaultAsync(spec, cancellationToken);

            if (category == null)
            {
                _logger.LogWarning("ID: {CategoryId} olan kategori bulunamadı.", request.Id);
                return null; // Bulunamadıysa null dön
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            _logger.LogInformation("ID: {CategoryId} olan kategori başarıyla getirildi.", request.Id);
            
            return categoryDto;
        }
    }
} 