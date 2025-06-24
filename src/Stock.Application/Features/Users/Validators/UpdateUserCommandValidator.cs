using FluentValidation;
using Stock.Application.Features.Users.Commands;

namespace Stock.Application.Features.Users.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Kullanıcı ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçersiz Kullanıcı ID.");

            RuleFor(v => v.Sicil)
                .NotEmpty().WithMessage("Sicil boş olamaz.")
                .MaximumLength(20).WithMessage("Sicil 20 karakterden uzun olamaz.");
                // Sicil formatı kontrolü eklenebilir

            RuleFor(v => v.Adi)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");

            RuleFor(v => v.Soyadi)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.");

            RuleFor(v => v.RoleId)
                .NotEmpty().WithMessage("Rol ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçersiz Rol ID.");
            
            // IsActive için özel bir kurala gerek yok, bool olduğu için.
        }
    }
} 