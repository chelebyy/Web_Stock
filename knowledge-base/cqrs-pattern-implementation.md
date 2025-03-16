# CQRS Deseni Implementasyonu

## Genel Bakış

CQRS (Command Query Responsibility Segregation) deseni, bir uygulamanın okuma (query) ve yazma (command) işlemlerini birbirinden ayıran bir mimari yaklaşımdır. Bu ayrım, her iki tarafın da bağımsız olarak ölçeklenmesine, optimize edilmesine ve geliştirilmesine olanak tanır.

## Bileşenler

### 1. Command Yapısı

Command'lar, sistemde bir değişiklik yapan işlemleri temsil eder. Her command, tek bir işlemi gerçekleştirmek için tasarlanmıştır ve genellikle void veya basit bir sonuç döndürür.

```csharp
public interface ICommand<TResponse>
{
}

public interface ICommand : ICommand<Unit>
{
}
```

### 2. Query Yapısı

Query'ler, sistemden veri almak için kullanılır ve herhangi bir değişiklik yapmazlar. Her query, belirli bir veri tipini döndürür.

```csharp
public interface IQuery<TResponse>
{
}
```

### 3. Handler Yapısı

Handler'lar, command ve query'leri işleyen sınıflardır. Her command ve query için bir handler sınıfı oluşturulur.

```csharp
public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
{
}

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}
```

### 4. Mediator Yapısı

Mediator, command ve query'leri ilgili handler'lara yönlendiren bir aracıdır. Bu, bileşenler arasındaki bağımlılıkları azaltır ve kodun daha modüler olmasını sağlar.

```csharp
public interface IMediator
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
}
```

## Implementasyon Adımları

### 1. Temel Yapıların Oluşturulması

İlk adım, CQRS deseninin temel yapılarını oluşturmaktır. Bu yapılar, `Stock.Application` projesinde `Common/CQRS` klasörü altında yer alacaktır.

```csharp
// Stock.Application/Common/CQRS/ICommand.cs
namespace Stock.Application.Common.CQRS
{
    public interface ICommand<TResponse>
    {
    }

    public interface ICommand : ICommand<Unit>
    {
    }
}

// Stock.Application/Common/CQRS/IQuery.cs
namespace Stock.Application.Common.CQRS
{
    public interface IQuery<TResponse>
    {
    }
}

// Stock.Application/Common/CQRS/ICommandHandler.cs
namespace Stock.Application.Common.CQRS
{
    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
    {
    }
}

// Stock.Application/Common/CQRS/IQueryHandler.cs
namespace Stock.Application.Common.CQRS
{
    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
}

// Stock.Application/Common/CQRS/Unit.cs
namespace Stock.Application.Common.CQRS
{
    public struct Unit
    {
        public static Unit Value => new Unit();
    }
}
```

### 2. Mediator Implementasyonu

Mediator, command ve query'leri ilgili handler'lara yönlendiren bir aracıdır. Bu implementasyon, `Stock.Infrastructure` projesinde `CQRS` klasörü altında yer alacaktır.

```csharp
// Stock.Application/Common/CQRS/IMediator.cs
namespace Stock.Application.Common.CQRS
{
    public interface IMediator
    {
        Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }
}

// Stock.Infrastructure/CQRS/Mediator.cs
namespace Stock.Infrastructure.CQRS
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {command.GetType().Name}");
            }

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method.Invoke(handler, new object[] { command, cancellationToken });

            return result;
        }

        public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"No handler registered for {query.GetType().Name}");
            }

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method.Invoke(handler, new object[] { query, cancellationToken });

            return result;
        }
    }
}
```

### 3. Dependency Injection Yapılandırması

CQRS bileşenlerini DI container'a kaydetmek için bir extension method oluşturulacaktır.

```csharp
// Stock.Infrastructure/CQRS/DependencyInjection.cs
namespace Stock.Infrastructure.CQRS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            // Mediator'ı kaydet
            services.AddScoped<IMediator, Mediator>();

            // Command ve Query handler'ları kaydet
            var assembly = Assembly.GetExecutingAssembly();
            
            // Command handler'ları kaydet
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            // Query handler'ları kaydet
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            return services;
        }
    }
}
```

### 4. Örnek Kullanım: Kullanıcı Oluşturma

CQRS desenini kullanarak bir kullanıcı oluşturma işlemi örneği:

```csharp
// Stock.Application/Features/Users/Commands/CreateUser/CreateUserCommand.cs
namespace Stock.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand : ICommand<int>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
    }
}

// Stock.Application/Features/Users/Commands/CreateUser/CreateUserCommandHandler.cs
namespace Stock.Application.Features.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly ILoggerManager _logger;

        public CreateUserCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordService passwordService,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<int> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Creating new user: {command.Username}");

            // Kullanıcı adının benzersiz olduğunu kontrol et
            var existingUser = await _unitOfWork.Users.GetByUsernameAsync(command.Username);
            if (existingUser != null)
            {
                _logger.LogWarn($"Username already exists: {command.Username}");
                throw new ApplicationException($"Username '{command.Username}' is already taken.");
            }

            // Şifreyi hashle
            var passwordHash = _passwordService.HashPassword(command.Password);

            // Yeni kullanıcı oluştur
            var user = new User
            {
                Username = command.Username,
                PasswordHash = passwordHash,
                Email = command.Email,
                FullName = command.FullName,
                RoleId = command.RoleId,
                IsAdmin = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            // Kullanıcıyı veritabanına ekle
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo($"User created successfully. ID: {user.Id}");

            return user.Id;
        }
    }
}
```

### 5. Örnek Kullanım: Kullanıcı Sorgulama

CQRS desenini kullanarak bir kullanıcı sorgulama işlemi örneği:

```csharp
// Stock.Application/Features/Users/Queries/GetUserById/GetUserByIdQuery.cs
namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IQuery<UserDto>
    {
        public int Id { get; set; }
    }
}

// Stock.Application/Features/Users/Queries/GetUserById/GetUserByIdQueryHandler.cs
namespace Stock.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public GetUserByIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Getting user by ID: {query.Id}");

            var user = await _unitOfWork.Users.GetByIdAsync(query.Id);
            if (user == null)
            {
                _logger.LogWarn($"User not found. ID: {query.Id}");
                throw new NotFoundException($"User with ID {query.Id} not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
```

### 6. Controller Entegrasyonu

Controller'lar, Mediator aracılığıyla command ve query'leri gönderecektir.

```csharp
// Stock.API/Controllers/UsersController.cs
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
        public async Task<ActionResult<UserDto>> GetById(int id)
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
                _logger.LogError($"Error getting user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> Create(CreateUserCommand command)
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
                _logger.LogError($"Error creating user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
            }
        }
    }
}
```

## Faydaları

CQRS deseninin uygulanması aşağıdaki faydaları sağlar:

1. **Ayrılmış Sorumluluklar**: Okuma ve yazma işlemleri ayrı sınıflarda gerçekleştirilir, bu da kodun daha modüler ve bakımı daha kolay olmasını sağlar.

2. **Ölçeklenebilirlik**: Okuma ve yazma işlemleri bağımsız olarak ölçeklendirilebilir. Örneğin, okuma işlemleri için ayrı bir veritabanı kullanılabilir.

3. **Performans Optimizasyonu**: Okuma işlemleri için özel sorgular ve indeksler oluşturulabilir, yazma işlemleri için ise veri bütünlüğü ve tutarlılık sağlanabilir.

4. **Daha İyi Test Edilebilirlik**: Command ve query'ler ayrı sınıflarda olduğu için, her biri bağımsız olarak test edilebilir.

5. **Daha Temiz Kod**: Her command ve query'nin tek bir sorumluluğu olduğu için, kod daha temiz ve anlaşılır olur.

## Dikkat Edilmesi Gereken Noktalar

1. **Karmaşıklık**: CQRS, basit CRUD işlemleri için fazla karmaşık olabilir. Bu nedenle, projenin ihtiyaçlarına göre değerlendirilmelidir.

2. **Tutarlılık**: Okuma ve yazma modelleri ayrı olduğu için, veri tutarlılığı sağlamak için ek önlemler alınmalıdır.

3. **Öğrenme Eğrisi**: CQRS, geleneksel mimarilere göre daha karmaşık olduğu için, ekip üyelerinin bu deseni öğrenmesi zaman alabilir.

4. **Aşırı Mühendislik**: Her durumda CQRS kullanmak, gereksiz karmaşıklığa yol açabilir. Bu nedenle, projenin ihtiyaçlarına göre değerlendirilmelidir.

## Sonraki Adımlar

CQRS deseninin uygulanmasından sonra, aşağıdaki adımlar izlenebilir:

1. **Event Sourcing**: CQRS ile birlikte Event Sourcing kullanılarak, sistemdeki tüm değişiklikler olay olarak kaydedilebilir.

2. **Mikroservis Mimarisi**: CQRS, mikroservis mimarisi ile birlikte kullanılarak, her mikroservisin kendi command ve query'leri olabilir.

3. **Domain-Driven Design (DDD)**: CQRS, DDD ile birlikte kullanılarak, domain modeli daha doğru bir şekilde temsil edilebilir. 