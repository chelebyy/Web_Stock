using FluentValidation;
using Stock.Application.Features.Users.Commands;

namespace Stock.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.Sicil)
                .NotEmpty().WithMessage("Sicil boş olamaz.")
                .MaximumLength(20).WithMessage("Sicil 20 karakterden uzun olamaz.");
                // Sicil formatı kontrolü eklenebilir (sadece rakam vb.)

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
                // Daha karmaşık şifre kuralları eklenebilir (büyük harf, küçük harf, rakam, özel karakter)

            RuleFor(v => v.Adi)
                .NotEmpty().WithMessage("Ad boş olamaz.")
                .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.");

            RuleFor(v => v.Soyadi)
                .NotEmpty().WithMessage("Soyad boş olamaz.")
                .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.");

            RuleFor(v => v.RoleId)
                .NotEmpty().WithMessage("Rol ID boş olamaz.")
                .GreaterThan(0).WithMessage("Geçersiz Rol ID.");
        }
    }
} 