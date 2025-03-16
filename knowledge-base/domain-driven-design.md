# Domain-Driven Design (DDD) Prensiplerinin Uygulanması

## Genel Bakış

Domain-Driven Design (DDD), karmaşık yazılım projelerinde iş mantığını ve teknik uygulamayı birbirine bağlayan bir yazılım geliştirme yaklaşımıdır. Bu belge, Stock projemizde DDD prensiplerinin nasıl uygulandığını detaylandırmaktadır.

## DDD'nin Temel Prensipleri

1. **Ubiquitous Language (Yaygın Dil)**: Teknik ekip ve domain uzmanları arasında ortak bir dil oluşturulması
2. **Bounded Contexts (Sınırlı Bağlamlar)**: Büyük domain modellerinin daha küçük, yönetilebilir parçalara bölünmesi
3. **Entities (Varlıklar)**: Kimliği olan ve yaşam döngüsü boyunca değişebilen nesneler
4. **Value Objects (Değer Nesneleri)**: Değerleriyle tanımlanan, değişmez (immutable) nesneler
5. **Aggregates (Kümeler)**: İlişkili entity ve value object'lerin bir arada tutulduğu kümeler
6. **Domain Events (Domain Olayları)**: Domain'de meydana gelen önemli olaylar
7. **Repositories (Depolar)**: Domain nesnelerinin kalıcı depolanması için arayüzler
8. **Domain Services (Domain Servisleri)**: Entity veya value object'lere ait olmayan domain mantığı

## Uygulama Adımları

### 1. Domain Katmanının Yeniden Yapılandırılması

Domain katmanı, iş mantığının merkezi olarak yeniden yapılandırıldı:

```csharp
// src/Stock.Domain/Entities/Product.cs
namespace Stock.Domain.Entities
{
    public class Product : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int StockQuantity { get; private set; }
        public int CategoryId { get; private set; }
        public Category Category { get; private set; }
        
        // Value Objects
        public ProductStatus Status { get; private set; }
        public Money Cost { get; private set; }
        
        // Domain Events
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
        
        // Factory Method
        public static Product Create(string name, string description, decimal price, int stockQuantity, int categoryId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Price = price,
                StockQuantity = stockQuantity,
                CategoryId = categoryId,
                Status = ProductStatus.Active
            };
            
            product._domainEvents.Add(new ProductCreatedDomainEvent(product.Id));
            
            return product;
        }
        
        // Domain Logic
        public void UpdateStock(int quantity)
        {
            if (quantity < 0 && Math.Abs(quantity) > StockQuantity)
                throw new DomainException("Stok miktarı yetersiz");
                
            StockQuantity += quantity;
            
            if (quantity < 0)
                _domainEvents.Add(new ProductStockDecreasedDomainEvent(Id, Math.Abs(quantity)));
            else
                _domainEvents.Add(new ProductStockIncreasedDomainEvent(Id, quantity));
        }
        
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
```

### 2. Value Objects Uygulaması

Değer nesneleri, değişmez (immutable) ve değerleriyle tanımlanan nesnelerdir:

```csharp
// src/Stock.Domain/ValueObjects/Money.cs
namespace Stock.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }
        
        private Money() { }
        
        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new DomainException("Para miktarı negatif olamaz");
                
            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Para birimi belirtilmelidir");
                
            Amount = amount;
            Currency = currency;
        }
        
        public Money Add(Money money)
        {
            if (Currency != money.Currency)
                throw new DomainException("Farklı para birimleri toplanamaz");
                
            return new Money(Amount + money.Amount, Currency);
        }
        
        public Money Subtract(Money money)
        {
            if (Currency != money.Currency)
                throw new DomainException("Farklı para birimleri çıkarılamaz");
                
            return new Money(Amount - money.Amount, Currency);
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
```

### 3. Domain Events Uygulaması

Domain olayları, domain'de meydana gelen önemli olayları temsil eder:

```csharp
// src/Stock.Domain/Events/ProductCreatedDomainEvent.cs
namespace Stock.Domain.Events
{
    public class ProductCreatedDomainEvent : IDomainEvent
    {
        public int ProductId { get; }
        public DateTime OccurredOn { get; }
        
        public ProductCreatedDomainEvent(int productId)
        {
            ProductId = productId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
```

### 4. Repository Pattern Uygulaması

Repository pattern, domain nesnelerinin kalıcı depolanması için arayüzler sağlar:

```csharp
// src/Stock.Domain/Interfaces/IProductRepository.cs
namespace Stock.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetByIdWithCategoryAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    }
}

// src/Stock.Infrastructure/Repositories/ProductRepository.cs
namespace Stock.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        
        public async Task<Product> GetByIdWithCategoryAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        
        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }
        
        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products
                .Where(p => p.StockQuantity < threshold)
                .ToListAsync();
        }
    }
}
```

### 5. Domain Services Uygulaması

Domain servisleri, entity veya value object'lere ait olmayan domain mantığını içerir:

```csharp
// src/Stock.Domain/Services/InventoryService.cs
namespace Stock.Domain.Services
{
    public class InventoryService : IDomainService
    {
        public bool CanFulfillOrder(Product product, int quantity)
        {
            return product.StockQuantity >= quantity;
        }
        
        public void ProcessStockMovement(Product product, int quantity, StockMovementType movementType)
        {
            switch (movementType)
            {
                case StockMovementType.Addition:
                    product.UpdateStock(Math.Abs(quantity));
                    break;
                case StockMovementType.Reduction:
                    product.UpdateStock(-Math.Abs(quantity));
                    break;
                default:
                    throw new DomainException("Geçersiz stok hareketi tipi");
            }
        }
    }
}
```

### 6. Aggregate Roots Tanımlanması

Aggregate root'lar, ilişkili entity ve value object'lerin bir arada tutulduğu kümelerdir:

```csharp
// src/Stock.Domain/Interfaces/IAggregateRoot.cs
namespace Stock.Domain.Interfaces
{
    public interface IAggregateRoot
    {
    }
}
```

### 7. CQRS ile Entegrasyon

DDD, CQRS (Command Query Responsibility Segregation) ile entegre edildi:

```csharp
// src/Stock.Application/Products/Commands/CreateProduct/CreateProductCommand.cs
namespace Stock.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : ICommand<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
    }
    
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        
        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IDomainEventDispatcher domainEventDispatcher)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _domainEventDispatcher = domainEventDispatcher;
        }
        
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(
                request.Name,
                request.Description,
                request.Price,
                request.StockQuantity,
                request.CategoryId);
                
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            // Domain olaylarını yayınla
            foreach (var domainEvent in product.DomainEvents)
            {
                await _domainEventDispatcher.Dispatch(domainEvent, cancellationToken);
            }
            
            product.ClearDomainEvents();
            
            return product.Id;
        }
    }
}
```

### 8. Domain Event Dispatcher Uygulaması

Domain olaylarını yayınlamak için bir dispatcher uygulandı:

```csharp
// src/Stock.Infrastructure/DomainEvents/DomainEventDispatcher.cs
namespace Stock.Infrastructure.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        
        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task Dispatch(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
```

### 9. Domain Event Handlers Uygulaması

Domain olaylarını işlemek için handler'lar uygulandı:

```csharp
// src/Stock.Application/Products/EventHandlers/ProductCreatedDomainEventHandler.cs
namespace Stock.Application.Products.EventHandlers
{
    public class ProductCreatedDomainEventHandler : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly ILogger<ProductCreatedDomainEventHandler> _logger;
        
        public ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
        {
            _logger = logger;
        }
        
        public Task Handle(ProductCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Yeni ürün oluşturuldu: {ProductId}", notification.ProductId);
            
            // Burada e-posta gönderme, bildirim oluşturma gibi yan etkiler gerçekleştirilebilir
            
            return Task.CompletedTask;
        }
    }
}
```

## Faydalar

Domain-Driven Design'ın uygulanması aşağıdaki faydaları sağlamıştır:

1. **İş Mantığının Merkezileştirilmesi**: Domain katmanı, iş mantığının tek kaynağı haline geldi.
2. **Daha İyi İletişim**: Teknik ekip ve domain uzmanları arasında ortak bir dil oluşturuldu.
3. **Daha Modüler Yapı**: Bounded context'ler sayesinde sistem daha modüler hale geldi.
4. **Daha Sağlam Domain Modeli**: Entity'ler, value object'ler ve aggregate'ler sayesinde daha sağlam bir domain modeli oluşturuldu.
5. **Daha İyi Test Edilebilirlik**: Domain mantığı, altyapı detaylarından ayrıldığı için daha kolay test edilebilir hale geldi.
6. **Daha Esnek Mimari**: DDD, CQRS ve Event-Driven Architecture ile entegre edilerek daha esnek bir mimari oluşturuldu.

## Sonraki Adımlar

1. **Bounded Context'lerin Belirlenmesi**: Sistemdeki bounded context'lerin daha net belirlenmesi ve dokümante edilmesi.
2. **Context Map Oluşturulması**: Bounded context'ler arasındaki ilişkilerin belirlenmesi ve dokümante edilmesi.
3. **Mikroservis Mimarisine Geçiş**: DDD prensipleri kullanılarak mikroservis mimarisine geçiş hazırlığı yapılması. 