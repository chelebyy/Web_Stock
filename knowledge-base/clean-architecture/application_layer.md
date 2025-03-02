# Application Katmanı Geçişi

## Genel Bakış

Application katmanı, Clean Architecture'ın ikinci katmanıdır ve iş mantığının uygulanmasını sağlar. Bu katman, Domain katmanına bağımlıdır ancak Infrastructure katmanına bağımlı olmamalıdır. Application katmanı, DTO'ları, CQRS yapısını, validation kurallarını ve servis uygulamalarını içerir.

## Taşınacak Bileşenler

1. **DTO'lar (Data Transfer Objects)**
   - UserDto
   - RoleDto
   - LoginDto
   - RegisterDto
   - Diğer DTO'lar

2. **CQRS Yapısı**
   - Commands
   - Queries
   - Handlers

3. **Validation Kuralları**
   - FluentValidation kuralları

4. **Servis Arayüzleri ve Uygulamaları**
   - IAuthService
   - IAuditService
   - Diğer servisler

## Geçiş Adımları

### 1. NuGet Paketleri

Application katmanı için gerekli NuGet paketlerini ekleyelim:

```bash
dotnet add package AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjection
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

### 2. DTO Sınıfları

```csharp
namespace Stock.Application.Models.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public int? RoleId { get; set; }
    }

    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public string? RoleName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
```

### 3. AutoMapper Profilleri

```csharp
using AutoMapper;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

            // Role mappings
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

            // Other mappings
        }
    }
}
```

### 4. CQRS Yapısı - Commands

```csharp
using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
    }

    public class UpdateUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public int? RoleId { get; set; }
    }

    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
```

### 5. CQRS Yapısı - Queries

```csharp
using MediatR;
using Stock.Application.Models.DTOs;

namespace Stock.Application.Features.Users.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
    }

    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
    }

    public class GetUserByUsernameQuery : IRequest<UserDto>
    {
        public string Username { get; set; } = string.Empty;
    }
}
```

### 6. CQRS Yapısı - Handlers

```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            return _mapper.Map<UserDto>(user);
        }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.Users.GetUsersWithRolesAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
```

### 7. Validation Kuralları

```csharp
using FluentValidation;
using Stock.Application.Features.Users.Commands;

namespace Stock.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
        }
    }

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçerli bir kullanıcı ID'si belirtilmelidir");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalıdır")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olabilir");
        }
    }
}
```

### 8. Servis Arayüzleri

```csharp
using Stock.Application.Models.DTOs;

namespace Stock.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        string GenerateJwtToken(UserDto user);
    }

    public interface IAuditService
    {
        Task LogActionAsync(string action, string entityType, string entityId, string userId, string path, string details);
    }
}
```

### 9. Servis Uygulamaları

```csharp
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Stock.Application.Common.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
            
            if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Kullanıcı adı veya şifre hatalı"
                };
            }

            var userDto = _mapper.Map<UserDto>(user);
            var token = GenerateJwtToken(userDto);

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                RoleName = user.Role?.Name
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if username already exists
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Bu kullanıcı adı zaten kullanılıyor"
                };
            }

            // Check if passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    ErrorMessage = "Şifreler eşleşmiyor"
                };
            }

            // Create new user
            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
                RoleId = registerDto.RoleId,
                IsAdmin = false // Default to non-admin
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);
            var token = GenerateJwtToken(userDto);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                RoleName = user.Role?.Name
            };
        }

        public string GenerateJwtToken(UserDto user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : (user.RoleName ?? "User"))
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(double.Parse(jwtSettings["ExpirationHours"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
```

### 10. Dependency Injection

```csharp
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Behaviors;
using Stock.Application.Common.Interfaces;
using Stock.Application.Common.Mappings;
using Stock.Application.Common.Services;
using System.Reflection;

namespace Stock.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditService, AuditService>();

            return services;
        }
    }
}
```

## Dikkat Edilecek Noktalar

1. **Bağımlılık Yönetimi**
   - Application katmanı sadece Domain katmanına bağımlı olmalı
   - Infrastructure katmanına bağımlı olmamalı
   - Dış servislere erişim için interface'ler tanımlanmalı

2. **CQRS Yapısı**
   - Command ve Query'ler ayrı tutulmalı
   - Her Command/Query için bir Handler olmalı
   - Validation için Pipeline Behavior kullanılmalı

3. **AutoMapper Kullanımı**
   - Karmaşık mapping'ler için özel konfigürasyonlar yapılmalı
   - Circular reference'lar önlenmeli

4. **Validation**
   - FluentValidation ile validation kuralları tanımlanmalı
   - Validation hataları uygun şekilde ele alınmalı

## Sonraki Adımlar

Application katmanı tamamlandıktan sonra:

1. Infrastructure katmanı için DbContext yapılandırması
2. Repository uygulamaları
3. UnitOfWork uygulaması
4. Harici servis entegrasyonları 