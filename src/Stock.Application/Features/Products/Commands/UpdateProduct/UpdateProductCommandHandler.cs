using AutoMapper;
using MediatR;
using Stock.Domain.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;
using Stock.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Stock.Domain.Specifications.Products;
using Stock.Domain.Specifications.Categories;

namespace Stock.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.GetRepository<Product>();
            var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken);

            if (product == null)
            {
                throw new NotFoundException($"Product with ID {request.Id} not found.");
            }

            if (request.CategoryId != product.CategoryId)
            {
                var categoryRepo = _unitOfWork.GetRepository<Category>();
                var category = await categoryRepo.FirstOrDefaultAsync(new CategoryByIdSpecification(request.CategoryId), cancellationToken);
                if (category == null)
                {
                    throw new DomainException($"Category not found with ID: {request.CategoryId}");
                }
                product.ChangeCategory(request.CategoryId);
            }

            var newName = ProductName.From(request.Name);
            var newDescription = ProductDescription.From(request.Description);

            if (request.StockLevel != product.StockLevel.Value)
            {
                int difference = request.StockLevel - product.StockLevel.Value;
                if (difference > 0)
                {
                    product.IncreaseStock(difference);
                }
                else if (difference < 0)
                {
                    product.DecreaseStock(-difference);
                }
            }

            product.UpdateName(newName);
            product.UpdateDescription(newDescription);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
} 