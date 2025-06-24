using FluentValidation;

namespace Stock.Application.Features.Roles.Commands.UpdateRole;

/// <summary>
/// Validator for the <see cref="UpdateRoleCommand"/>.
/// </summary>
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(v => v.Id)
            .GreaterThan(0).WithMessage("Geçerli bir Rol ID girilmelidir.");

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Rol adı boş olamaz.")
            .MaximumLength(50).WithMessage("Rol adı 50 karakterden uzun olamaz.");

        // Benzersizlik kontrolü yine Handler içinde yapılacak.
    }
} 