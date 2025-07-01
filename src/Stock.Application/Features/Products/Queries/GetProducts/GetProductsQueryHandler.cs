using AutoMapper;
using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Models;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResponse<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            // Arama ve sayfalama için spesifikasyonu oluştur
            var spec = new ProductsWithCategorySpecification(
                nameFilter: request.SearchText,
                sortBy: request.SortField ?? "Name",
                sortDirection: request.SortOrder ?? "asc",
                pageNumber: request.Page,
                pageSize: request.PageSize
            );

            // Filtrelenmiş toplam kayıt sayısını almak için ayrı bir spesifikasyon
            var countSpec = new ProductsWithCategorySpecification(nameFilter: request.SearchText);
            var totalRecords = await _unitOfWork.Products.CountAsync(countSpec).ConfigureAwait(false);
            
            // Verileri spesifikasyona göre çek
            var products = await _unitOfWork.Products.ListAsync(spec, cancellationToken).ConfigureAwait(false);

            // Gelen verileri DTO'ya map'le
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            // Sayfalanmış yanıtı oluştur ve dön
            return new PagedResponse<ProductDto>(productsDto, request.Page, request.PageSize, totalRecords);
        }
    }
} 