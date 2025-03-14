# Clean Architecture Uyumluluğu

## Genel Bakış

Clean Architecture, yazılım sistemlerinin katmanlı bir yapıda tasarlanmasını ve bu katmanlar arasındaki bağımlılıkların içten dışa doğru olmasını öngören bir mimari yaklaşımdır. Bu yaklaşım, sistemin daha modüler, test edilebilir ve sürdürülebilir olmasını sağlar.

## Temel Prensipler

1. **Bağımlılık Kuralı**: Kaynak kodu bağımlılıkları yalnızca içten dışa doğru olmalıdır. İç katmanlar, dış katmanlara bağımlı olmamalıdır.
2. **Soyutlama**: İç katmanlar, dış katmanlarla iletişim kurmak için soyutlamaları (interface) kullanmalıdır.
3. **Ayrılabilirlik**: Katmanlar birbirinden bağımsız olarak geliştirilebilmeli ve test edilebilmelidir.
4. **Değiştirilebilirlik**: Dış katmanlar, iç katmanları etkilemeden değiştirilebilmelidir.

## Stock Projesi Katman Yapısı

Stock projesi, Clean Architecture prensiplerine uygun olarak aşağıdaki katmanlardan oluşmaktadır:

1. **Domain Katmanı (Stock.Domain)**: 
   - Sistemin çekirdek iş mantığını ve varlıklarını (entities) içerir.
   - Diğer katmanlara bağımlı olmamalıdır.
   - Repository interface'leri burada tanımlanır.

2. **Application Katmanı (Stock.Application)**:
   - İş akışlarını (use cases) ve uygulama mantığını içerir.
   - Domain katmanına bağımlıdır.
   - DTO'lar ve mapping işlemleri burada tanımlanır.
   - CQRS pattern'i (Command Query Responsibility Segregation) burada uygulanır.

3. **Infrastructure Katmanı (Stock.Infrastructure)**:
   - Veritabanı erişimi, harici servisler, loglama gibi teknik detayları içerir.
   - Domain ve Application katmanlarına bağımlıdır.
   - Repository implementasyonları burada yer alır.

4. **Presentation Katmanı (Stock.API)**:
   - Kullanıcı arayüzü ve API endpoint'lerini içerir.
   - Diğer tüm katmanlara bağımlıdır.
   - Controller'lar burada yer alır.

## Karşılaşılan Sorunlar ve Çözümler

### 1. Domain Katmanının Application Katmanına Bağımlılığı

**Sorun**: Domain katmanında Application katmanındaki DTO'ların kullanılması, bağımlılık kuralını ihlal etmektedir.

**Örnek**:
```csharp
// Hatalı yaklaşım
public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<UserSummaryDto>> GetUserSummariesAsync();
}
```

**Çözüm**:
1. Domain katmanında sadece domain entity'leri kullanılmalıdır.
2. Application katmanında, domain entity'lerini DTO'lara dönüştüren mapping işlemleri yapılmalıdır.

```csharp
// Doğru yaklaşım
// Domain katmanı
public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetUserSummariesAsync();
}

// Application katmanı
public class GetUserSummariesQueryHandler : IRequestHandler<GetUserSummariesQuery, IEnumerable<UserSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<IEnumerable<UserSummaryDto>> Handle(GetUserSummariesQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Users.GetUserSummariesAsync();
        return _mapper.Map<IEnumerable<UserSummaryDto>>(users);
    }
}
```

### 2. Infrastructure Katmanının Domain Katmanına Bağımlılığı

**Sorun**: Infrastructure katmanında domain entity'lerin doğrudan kullanılması, domain katmanının değişmesi durumunda infrastructure katmanının da değişmesine neden olabilir.

**Çözüm**:
1. Domain katmanında repository interface'leri tanımlanmalıdır.
2. Infrastructure katmanında bu interface'lerin implementasyonları yapılmalıdır.
3. Dependency Injection kullanılarak, infrastructure katmanındaki implementasyonlar application katmanına enjekte edilmelidir.

```csharp
// Domain katmanı
public interface IUserRepository : IRepository<User>
{
    Task<User> GetByIdAsync(int id);
}

// Infrastructure katmanı
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}

// API katmanı (Startup.cs)
public void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IUserRepository, UserRepository>();
}
```

## İyi Uygulama Örnekleri

### 1. CQRS Pattern'i

Command Query Responsibility Segregation (CQRS) pattern'i, okuma (query) ve yazma (command) işlemlerinin ayrılmasını sağlar. Bu pattern, Clean Architecture ile uyumlu bir şekilde uygulanabilir.

```csharp
// Query
public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }
}

// Query Handler
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        return _mapper.Map<UserDto>(user);
    }
}

// Command
public class CreateUserCommand : IRequest<UserDto>
{
    public string Username { get; set; }
    public string Email { get; set; }
}

// Command Handler
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }
}
```

### 2. Repository Pattern

Repository pattern, veri erişim katmanını soyutlamak için kullanılır. Bu pattern, Clean Architecture ile uyumlu bir şekilde uygulanabilir.

```csharp
// Domain katmanı
public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

// Infrastructure katmanı
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }
}
```

## Sonuç

Clean Architecture, yazılım sistemlerinin daha modüler, test edilebilir ve sürdürülebilir olmasını sağlar. Stock projesi, bu prensiplere uygun olarak tasarlanmıştır ve geliştirme sürecinde karşılaşılan sorunlar, Clean Architecture prensiplerine uygun olarak çözülmüştür.

Projenin geliştirilmesi sırasında, Clean Architecture prensiplerine uygun olarak hareket edilmesi, projenin uzun vadede daha sürdürülebilir olmasını sağlayacaktır. 