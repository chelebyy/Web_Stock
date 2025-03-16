using FluentValidation;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// CreateUserCommand validatörü
    /// </summary>
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        /// <summary>
        /// Yeni bir CreateUserCommandValidator örneği oluşturur
        /// </summary>
        public CreateUserCommandValidator()
        {
            RuleFor(v => v.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakterden uzun olamaz.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("E-posta adresi boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.")
                .MaximumLength(100).WithMessage("E-posta adresi 100 karakterden uzun olamaz.");

            RuleFor(v => v.FullName)
                .NotEmpty().WithMessage("Tam ad boş olamaz.")
                .MaximumLength(100).WithMessage("Tam ad 100 karakterden uzun olamaz.");

            RuleFor(v => v.RoleId)
                .GreaterThan(0).WithMessage("Geçerli bir rol seçiniz.");
        }
    }
} 