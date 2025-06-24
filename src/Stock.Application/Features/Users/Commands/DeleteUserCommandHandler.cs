using MediatR;
using Stock.Domain.Exceptions; // Correct namespace for Exceptions
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
// using Stock.Application.Exceptions; // Removed incorrect namespace
using Stock.Domain.Specifications; // Correct namespace for Specifications

namespace Stock.Application.Features.Users.Commands.DeleteUser;

/// <summary>
/// Handles the <see cref="DeleteUserCommand"/>.
/// </summary>
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool> // Correct return type
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Silinecek kullanıcıyı bul
        var userSpec = new UserByIdSpecification(request.Id);
        var userToDelete = await _unitOfWork.GetRepository<User>().FirstOrDefaultAsync(userSpec, cancellationToken);

        if (userToDelete == null)
        {
            // Kullanıcı bulunamadıysa NotFoundException fırlatmak iyi bir pratik.
            // throw new NotFoundException($"Silinecek kullanıcı bulunamadı (ID: {request.Id})."); // Using correct NotFoundException
            throw new NotFoundException(nameof(User), request.Id); // Use constructor
        }

        // 2. Kullanıcıyı sil
        _unitOfWork.GetRepository<User>().Delete(userToDelete); // Using synchronous Delete method
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Başarılı yanıt döndür (true)
        return true;
    }
} 