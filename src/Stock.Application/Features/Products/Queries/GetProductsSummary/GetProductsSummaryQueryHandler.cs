using AutoMapper;
using MediatR;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Models;
using Stock.Domain.Interfaces;
using Stock.Domain.Specifications.Products;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Products.Queries.GetProductsSummary
{
    public class GetProductsSummaryQueryHandler : IRequestHandler<GetProductsSummaryQuery, PagedResponse<ProductSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsSummaryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<ProductSummaryDto>> Handle(GetProductsSummaryQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductsWithCategorySpecification(
                nameFilter: request.Search,
                pageNumber: request.PageNumber,
                pageSize: request.PageSize);

            var countSpec = new ProductsWithCategorySpecification(nameFilter: request.Search);
            var totalRecords = await _unitOfWork.Products.CountAsync(countSpec).ConfigureAwait(false);
            
            var products = await _unitOfWork.Products.ListAsync(spec, cancellationToken).ConfigureAwait(false);

            var productsSummaryDto = _mapper.Map<IEnumerable<ProductSummaryDto>>(products);

            return new PagedResponse<ProductSummaryDto>(productsSummaryDto, request.PageNumber, request.PageSize, totalRecords);
        }
    }
} 