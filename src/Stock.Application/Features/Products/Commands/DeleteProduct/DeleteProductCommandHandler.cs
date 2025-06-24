using MediatR;
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

        // Silme işleminde genellikle Mapper'a ihtiyaç duyulmaz
        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Dönüş tipi Task<Unit> olmalı
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Use Specification Pattern to get the product
            var productRepo = _unitOfWork.GetRepository<Product>();
            var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            // Use DeleteAsync for soft delete consistency
            await productRepo.DeleteAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value; // Unit.Value döndürülmeli
        }
    }
} 