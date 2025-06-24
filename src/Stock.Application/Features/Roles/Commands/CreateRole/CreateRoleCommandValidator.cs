using FluentValidation;

namespace Stock.Application.Features.Roles.Commands.CreateRole;

/// <summary>
/// Validator for the <see cref="CreateRoleCommand"/>.
/// </summary>
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Rol adı boş olamaz.")
            .MaximumLength(50).WithMessage("Rol adı 50 karakterden uzun olamaz.");

        // İleride rol adı için benzersizlik kontrolü de eklenebilir.
        // Ancak bu kontrolü Handler içinde yapmak, veritabanı erişimi gerektirdiği için daha uygundur.
    }
} 