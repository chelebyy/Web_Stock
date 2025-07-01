using FluentValidation;
using Stock.Domain.ValueObjects;

namespace Stock.Application.Features.Users.Commands.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Kullanıcı ID'si geçerli olmalıdır.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(50)
                .WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-posta boş olamaz.")
                .EmailAddress()
                .WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Adi)
                .NotEmpty()
                .WithMessage("Ad boş olamaz.")
                .MaximumLength(50)
                .WithMessage("Ad 50 karakterden uzun olamaz.");

            RuleFor(x => x.Soyadi)
                .NotEmpty()
                .WithMessage("Soyad boş olamaz.")
                .MaximumLength(50)
                .WithMessage("Soyad 50 karakterden uzun olamaz.");

            RuleFor(x => x.Sicil)
                .NotEmpty()
                .WithMessage("Sicil numarası boş olamaz.")
                .MaximumLength(20)
                .WithMessage("Sicil numarası 20 karakterden uzun olamaz.");

            RuleFor(x => x.RoleId)
                .GreaterThan(0)
                .WithMessage("Rol ID'si geçerli olmalıdır.");

            // Value Objects validation
            RuleFor(x => new { x.Adi, x.Soyadi })
                .Must(x => FullName.Create(x.Adi, x.Soyadi).IsSuccess)
                .WithMessage("Geçersiz ad soyad bilgisi.");

            RuleFor(x => x.Sicil)
                .Must(sicil => Sicil.Create(sicil).IsSuccess)
                .WithMessage("Geçersiz sicil numarası.");
        }
    }
} 