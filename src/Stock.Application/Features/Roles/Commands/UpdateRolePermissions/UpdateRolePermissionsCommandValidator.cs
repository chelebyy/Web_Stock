using FluentValidation;
using System.Collections.Generic;

namespace Stock.Application.Features.Roles.Commands.UpdateRolePermissions
{
    public class UpdateRolePermissionsCommandValidator : AbstractValidator<UpdateRolePermissionsCommand>
    {
        public UpdateRolePermissionsCommandValidator()
        {
            RuleFor(v => v.RoleId)
                .NotEmpty().WithMessage("Rol ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir Rol ID belirtilmelidir.");

            RuleFor(v => v.PermissionIds)
                .NotNull().WithMessage("İzin listesi boş olamaz.");
        }
    }
} 