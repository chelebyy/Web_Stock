using AutoMapper;
using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Specifications; // Specification'ları kullanmak için
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // ID'ye göre ürünü ve kategorisini getiren Specification
            var spec = new ProductByIdWithCategorySpecification(request.Id);

            var product = await _unitOfWork.GetRepository<Product>().FirstOrDefaultAsync(spec, cancellationToken);

            if (product == null)
            {
                return null; // Veya NotFoundException fırlatılabilir
            }

            return _mapper.Map<ProductDto>(product);
        }
    }

    // ID'ye göre ürünü ve kategorisini getiren Specification
    public class ProductByIdWithCategorySpecification : BaseSpecification<Product>
    {
        public ProductByIdWithCategorySpecification(int productId)
            : base(p => p.Id == productId)
        {
            AddInclude(p => p.Category); // Category bilgisini dahil et
        }
    }
} 