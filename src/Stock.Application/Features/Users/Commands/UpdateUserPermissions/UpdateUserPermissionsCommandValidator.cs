using FluentValidation;

namespace Stock.Application.Features.Users.Commands.UpdateUserPermissions
{
    public class UpdateUserPermissionsCommandValidator : AbstractValidator<UpdateUserPermissionsCommand>
    {
        public UpdateUserPermissionsCommandValidator()
        {
            RuleFor(v => v.UserId)
                .NotEmpty().WithMessage("Kullanıcı ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir Kullanıcı ID belirtilmelidir.");

            RuleFor(v => v.Permissions)
                .NotNull().WithMessage("İzinler listesi boş olamaz.");
        }
    }
} 