using MediatR;

namespace Stock.Application.Features.Products.Commands.DeleteProduct
{
    // IRequest<Unit> olmalı (MediatR 12+ için)
    public class DeleteProductCommand : IRequest<Unit> 
    {
        public int Id { get; set; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
} 