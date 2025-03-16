# Mediator Deseni Implementasyonu

## Genel Bakış

Mediator deseni, nesneler arasındaki iletişimi merkezi bir nesne üzerinden yöneterek bileşenler arasındaki bağımlılıkları azaltan bir davranışsal tasarım desenidir. Bu desen, CQRS (Command Query Responsibility Segregation) deseni ile birlikte kullanıldığında, uygulamanın modülerliğini ve bakım yapılabilirliğini önemli ölçüde artırır.

## Mediator Deseninin Faydaları

1. **Gevşek Bağlantı (Loose Coupling)**: Bileşenler birbirleriyle doğrudan iletişim kurmak yerine, mediator üzerinden iletişim kurarak bağımlılıkları azaltır.
2. **Merkezi İletişim**: Tüm iletişim merkezi bir noktadan geçtiği için, iletişim akışını izlemek ve yönetmek daha kolaydır.
3. **Genişletilebilirlik**: Yeni bileşenler eklemek veya mevcut bileşenleri değiştirmek daha kolaydır, çünkü diğer bileşenleri etkilemez.
4. **Test Edilebilirlik**: Bileşenler arasındaki bağımlılıklar azaldığı için, birim testleri yazmak daha kolaydır.

## CQRS ile Mediator Deseninin Entegrasyonu

CQRS deseni, okuma (query) ve yazma (command) işlemlerini ayırarak, her birinin bağımsız olarak optimize edilmesini sağlar. Mediator deseni ise, bu command ve query'lerin ilgili handler'lara yönlendirilmesini sağlar. Bu iki desenin birlikte kullanılması, aşağıdaki avantajları sağlar:

1. **Modülerlik**: Her command ve query, kendi handler'ı tarafından işlenir, bu da kodun daha modüler olmasını sağlar.
2. **Tek Sorumluluk İlkesi (SRP)**: Her handler, yalnızca bir işlemi gerçekleştirmekten sorumludur.
3. **Ölçeklenebilirlik**: Okuma ve yazma işlemleri bağımsız olarak ölçeklendirilebilir.
4. **Performans Optimizasyonu**: Okuma ve yazma işlemleri için farklı optimizasyon stratejileri uygulanabilir.

## Implementasyon Adımları

### 1. MediatR Kütüphanesinin Eklenmesi

MediatR, .NET için popüler bir mediator implementasyonudur. Bu kütüphane, command ve query'lerin ilgili handler'lara yönlendirilmesini sağlar.

```bash
dotnet add src/Stock.Application/Stock.Application.csproj package MediatR
dotnet add src/Stock.Application/Stock.Application.csproj package MediatR.Extensions.Microsoft.DependencyInjection
```

### 2. Application Katmanında MediatR'ın Yapılandırılması

```csharp
// src/Stock.Application/DependencyInjection.cs
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Mappings;

namespace Stock.Application
{
    /// <summary>
    /// Application katmanı için DI yapılandırması
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Application servislerini DI container'a kaydeder
        /// </summary>
        /// <param name="services">Servis koleksiyonu</param>
        /// <returns>Servis koleksiyonu</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper yapılandırması
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // MediatR yapılandırması
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            return services;
        }
    }
}
```

### 3. Command ve Query Sınıflarının Güncellenmesi

MediatR kullanırken, command ve query sınıfları `IRequest<TResponse>` arayüzünü, handler sınıfları ise `IRequestHandler<TRequest, TResponse>` arayüzünü implement etmelidir.

```csharp
// src/Stock.Application/Features/Users/Commands/CreateUser/CreateUserCommand.cs
using MediatR;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Yeni kullanıcı oluşturma komutu
    /// </summary>
    public class CreateUserCommand : IRequest<int>
    {
        /// <summary>
        /// Kullanıcı adı
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Şifre
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// E-posta adresi
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Tam ad
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Rol ID
        /// </summary>
        public int RoleId { get; set; }
    }
}
```

```csharp
// src/Stock.Application/Features/Users/Commands/CreateUser/CreateUserCommandHandler.cs
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Yeni kullanıcı oluşturma komut işleyicisi
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir CreateUserCommandHandler örneği oluşturur
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="passwordService">Şifre servisi</param>
        /// <param name="logger">Logger</param>
        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _logger = logger;
        }

        /// <summary>
        /// Komutu işler
        /// </summary>
        /// <param name="request">İşlenecek komut</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Oluşturulan kullanıcının ID'si</returns>
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Yeni kullanıcı oluşturuluyor: {request.Username}");

            // Kullanıcı adının benzersiz olduğunu kontrol et
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarn($"Kullanıcı adı zaten mevcut: {request.Username}");
                throw new ApplicationException($"'{request.Username}' kullanıcı adı zaten kullanılıyor.");
            }

            // Şifreyi hashle
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Yeni kullanıcı oluştur
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
                IsAdmin = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            // Kullanıcıyı veritabanına ekle
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo($"Kullanıcı başarıyla oluşturuldu. ID: {user.Id}");

            return user.Id;
        }
    }
}
```

```csharp
// src/Stock.Application/Features/Users/Queries/GetUserById/GetUserByIdQuery.cs
using MediatR;
using Stock.Application.Features.Users.Dtos;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// ID'ye göre kullanıcı sorgulama
    /// </summary>
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        /// <summary>
        /// Kullanıcı ID
        /// </summary>
        public int Id { get; set; }
    }
}
```

```csharp
// src/Stock.Application/Features/Users/Queries/GetUserById/GetUserByIdQueryHandler.cs
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Stock.Application.Common.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Users.Dtos;
using Stock.Domain.Interfaces;

namespace Stock.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// ID'ye göre kullanıcı sorgulama işleyicisi
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir GetUserByIdQueryHandler örneği oluşturur
        /// </summary>
        /// <param name="unitOfWork">Unit of work</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        public GetUserByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Sorguyu işler
        /// </summary>
        /// <param name="request">İşlenecek sorgu</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Kullanıcı bilgileri</returns>
        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"ID'ye göre kullanıcı sorgulanıyor: {request.Id}");

            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(request.Id);
            if (user == null)
            {
                _logger.LogWarn($"Kullanıcı bulunamadı. ID: {request.Id}");
                throw new NotFoundException("Kullanıcı", request.Id);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
```

### 4. Controller'ların Güncellenmesi

Controller'lar, artık özel mediator implementasyonu yerine MediatR'ı kullanacaktır.

```csharp
// src/Stock.API/Controllers/UsersController.cs
using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Exceptions;
using Stock.Application.Common.Interfaces;
using Stock.Application.Features.Users.Commands.CreateUser;
using Stock.Application.Features.Users.Queries.GetUserById;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILoggerManager _logger;

        public UsersController(IMediator mediator, ILoggerManager logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var query = new GetUserByIdQuery { Id = id };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarn(ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kullanıcı getirme sırasında hata: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "İsteğiniz işlenirken bir hata oluştu." });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            try
            {
                var userId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
            }
            catch (ApplicationException ex)
            {
                _logger.LogWarn(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kullanıcı oluşturma sırasında hata: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "İsteğiniz işlenirken bir hata oluştu." });
            }
        }
    }
}
```

### 5. Davranış Pipeline'ları (Behavior Pipelines)

MediatR, request'lerin handler'lara gönderilmeden önce veya sonra çalışacak davranışlar (behaviors) eklemenize olanak tanır. Bu, loglama, validasyon, performans ölçümü gibi cross-cutting concerns'leri uygulamak için kullanılabilir.

```csharp
// src/Stock.Application/Common/Behaviors/LoggingBehavior.cs
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Common.Behaviors
{
    /// <summary>
    /// Loglama davranışı
    /// </summary>
    /// <typeparam name="TRequest">İstek tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt tipi</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir LoggingBehavior örneği oluşturur
        /// </summary>
        /// <param name="logger">Logger</param>
        public LoggingBehavior(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// İsteği işler
        /// </summary>
        /// <param name="request">İstek</param>
        /// <param name="next">Sonraki handler</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Yanıt</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInfo($"İşleniyor: {requestName}");

            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            _logger.LogInfo($"İşlendi: {requestName} ({stopwatch.ElapsedMilliseconds}ms)");

            return response;
        }
    }
}
```

```csharp
// src/Stock.Application/Common/Behaviors/ValidationBehavior.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = Stock.Application.Common.Exceptions.ValidationException;

namespace Stock.Application.Common.Behaviors
{
    /// <summary>
    /// Validasyon davranışı
    /// </summary>
    /// <typeparam name="TRequest">İstek tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt tipi</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Yeni bir ValidationBehavior örneği oluşturur
        /// </summary>
        /// <param name="validators">Validatörler</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// İsteği işler
        /// </summary>
        /// <param name="request">İstek</param>
        /// <param name="next">Sonraki handler</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Yanıt</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
}
```

```csharp
// src/Stock.Application/Common/Exceptions/ValidationException.cs
using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Stock.Application.Common.Exceptions
{
    /// <summary>
    /// Validasyon hatası istisnası
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Validasyon hataları
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// Yeni bir ValidationException örneği oluşturur
        /// </summary>
        public ValidationException()
            : base("Bir veya daha fazla validasyon hatası oluştu.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Validasyon hatalarıyla yeni bir ValidationException örneği oluşturur
        /// </summary>
        /// <param name="failures">Validasyon hataları</param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());

            Errors = failureGroups;
        }
    }
}
```

### 6. Davranışların DI Container'a Kaydedilmesi

```csharp
// src/Stock.Application/DependencyInjection.cs
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Behaviors;
using Stock.Application.Common.Mappings;

namespace Stock.Application
{
    /// <summary>
    /// Application katmanı için DI yapılandırması
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Application servislerini DI container'a kaydeder
        /// </summary>
        /// <param name="services">Servis koleksiyonu</param>
        /// <returns>Servis koleksiyonu</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper yapılandırması
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // MediatR yapılandırması
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            // MediatR davranışlarını kaydet
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            // FluentValidation yapılandırması
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
}
```

### 7. Validasyon Sınıflarının Eklenmesi

```csharp
// src/Stock.Application/Features/Users/Commands/CreateUser/CreateUserCommandValidator.cs
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
```

## Örnek Kullanım Senaryoları

### 1. Kullanıcı Oluşturma

```csharp
// Controller
[HttpPost]
public async Task<IActionResult> Create(CreateUserCommand command)
{
    var userId = await _mediator.Send(command);
    return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
}

// Command
public class CreateUserCommand : IRequest<int>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
}

// Handler
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    // ...

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Kullanıcı oluşturma işlemleri
        return userId;
    }
}
```

### 2. Kullanıcı Sorgulama

```csharp
// Controller
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var query = new GetUserByIdQuery { Id = id };
    var result = await _mediator.Send(query);
    return Ok(result);
}

// Query
public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

// Handler
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    // ...

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // Kullanıcı sorgulama işlemleri
        return userDto;
    }
}
```

## Sonuç

Mediator deseni, CQRS deseni ile birlikte kullanıldığında, uygulamanın modülerliğini, bakım yapılabilirliğini ve test edilebilirliğini önemli ölçüde artırır. MediatR kütüphanesi, bu desenleri .NET uygulamalarında kolayca uygulamanıza olanak tanır.

Bu implementasyon, aşağıdaki avantajları sağlar:

1. **Modülerlik**: Her command ve query, kendi handler'ı tarafından işlenir.
2. **Tek Sorumluluk İlkesi (SRP)**: Her handler, yalnızca bir işlemi gerçekleştirmekten sorumludur.
3. **Gevşek Bağlantı (Loose Coupling)**: Bileşenler arasındaki bağımlılıklar azaltılır.
4. **Cross-Cutting Concerns**: Loglama, validasyon gibi cross-cutting concerns'ler, davranışlar (behaviors) aracılığıyla merkezi olarak uygulanabilir.
5. **Test Edilebilirlik**: Bileşenler arasındaki bağımlılıklar azaldığı için, birim testleri yazmak daha kolaydır. 