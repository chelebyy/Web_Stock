# Domain-Driven Design (DDD) Prensiplerinin Uygulanması

## Genel Bakış

Domain-Driven Design (DDD), karmaşık yazılım projelerinde iş mantığını ve teknik uygulamayı birbirine bağlayan bir yazılım geliştirme yaklaşımıdır. Bu yaklaşım, iş mantığını domain modelleri etrafında organize ederek, kodun daha anlaşılır, bakımı daha kolay ve iş gereksinimlerine daha uygun olmasını sağlar.

## DDD'nin Temel Prensipleri

### 1. Ubiquitous Language (Yaygın Dil)

Geliştirme ekibi ve domain uzmanları arasında ortak bir dil oluşturulması, iletişim sorunlarını azaltır ve herkesin aynı terminolojiyi kullanmasını sağlar.

- **Uygulama**: Tüm kod tabanında, veritabanı şemalarında, API'lerde ve dokümantasyonda tutarlı bir terminoloji kullanılacak.
- **Örnek**: "Kullanıcı" yerine "Personel" veya "Çalışan" gibi domain-spesifik terimler kullanılacak.

### 2. Bounded Contexts (Sınırlı Bağlamlar)

Büyük ve karmaşık domainleri daha küçük, yönetilebilir parçalara bölmek, her bir parçanın kendi modelini ve sınırlarını tanımlamasını sağlar.

- **Uygulama**: Projemiz için şu bounded context'ler tanımlanacak:
  - Kullanıcı Yönetimi
  - Stok Yönetimi
  - Sipariş İşleme
  - Raporlama
- **Örnek**: "Kullanıcı" kavramı, Kullanıcı Yönetimi bağlamında tam profil bilgileriyle, Sipariş İşleme bağlamında ise sadece temel bilgilerle temsil edilebilir.

### 3. Entities (Varlıklar)

Kimliği olan ve yaşam döngüsü boyunca değişebilen nesnelerdir.

- **Uygulama**: Tüm entity sınıfları için aşağıdaki özellikler sağlanacak:
  - Benzersiz kimlik (ID)
  - Değişmezlik garantisi (immutability) için özel metotlar
  - İş kurallarının uygulanması için domain mantığı
- **Örnek**: `User`, `Product`, `Order` gibi sınıflar entity olarak modellenecek.

### 4. Value Objects (Değer Nesneleri)

Kimliği olmayan, değerleriyle tanımlanan ve değişmez (immutable) olan nesnelerdir.

- **Uygulama**: Aşağıdaki kavramlar value object olarak modellenecek:
  - Adres
  - Para Birimi ve Miktar
  - Tarih Aralığı
  - Ölçü Birimleri
- **Örnek**: `Money`, `DateRange`, `Address` gibi sınıflar value object olarak modellenecek.

### 5. Aggregates (Kümeler)

Bir kök entity (root entity) etrafında gruplanmış, birlikte tutarlılık sınırını oluşturan nesneler kümesidir.

- **Uygulama**: Aşağıdaki aggregate'ler tanımlanacak:
  - Order Aggregate (Order root entity, OrderItem child entities)
  - Product Aggregate (Product root entity, ProductVariant child entities)
  - User Aggregate (User root entity, UserPreferences value object)
- **Örnek**: Bir sipariş (Order) ve sipariş kalemleri (OrderItems) bir aggregate oluşturur, ve sipariş kalemleri doğrudan erişilemez, sadece sipariş üzerinden erişilebilir.

### 6. Repositories (Depolar)

Aggregate'lerin kalıcı depolanması ve alınması için kullanılan soyutlamalardır.

- **Uygulama**: Her aggregate için bir repository tanımlanacak:
  - IOrderRepository
  - IProductRepository
  - IUserRepository
- **Örnek**: `IProductRepository` sadece `Product` aggregate'ini yönetir, içindeki `ProductVariant`'ları doğrudan yönetmez.

### 7. Domain Services (Domain Servisleri)

Doğal olarak bir entity veya value object'e ait olmayan domain mantığını içeren servislerdir.

- **Uygulama**: Aşağıdaki domain servisleri oluşturulacak:
  - StockAllocationService
  - PricingService
  - AuthorizationService
- **Örnek**: `PricingService`, ürün fiyatlandırması için karmaşık iş kurallarını uygular.

### 8. Application Services (Uygulama Servisleri)

Domain servislerini ve repository'leri kullanarak use case'leri uygulayan servislerdir.

- **Uygulama**: CQRS pattern ile entegre edilecek:
  - Command Handler'lar
  - Query Handler'lar
- **Örnek**: `CreateOrderCommandHandler`, bir sipariş oluşturma use case'ini uygular.

### 9. Domain Events (Domain Olayları)

Domain'de meydana gelen önemli olayları temsil eden ve diğer bounded context'lerin bu olaylara tepki vermesini sağlayan nesnelerdir.

- **Uygulama**: Aşağıdaki domain olayları tanımlanacak:
  - OrderCreatedEvent
  - ProductStockChangedEvent
  - UserRegisteredEvent
- **Örnek**: Bir sipariş oluşturulduğunda, `OrderCreatedEvent` yayınlanır ve stok yönetimi bu olaya tepki vererek stok miktarlarını günceller.

## Uygulama Adımları

### 1. Domain Modelinin Oluşturulması

```csharp
// src/Stock.Domain/Entities/Product.cs
namespace Stock.Domain.Entities
{
    /// <summary>
    /// Ürün entity'si
    /// </summary>
    public class Product : BaseEntity
    {
        private List<ProductVariant> _variants = new List<ProductVariant>();
        
        /// <summary>
        /// Ürün adı
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Ürün kodu
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// Ürün açıklaması
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Kategori ID
        /// </summary>
        public int CategoryId { get; private set; }
        
        /// <summary>
        /// Stok miktarı
        /// </summary>
        public int StockQuantity { get; private set; }
        
        /// <summary>
        /// Birim fiyatı
        /// </summary>
        public Money UnitPrice { get; private set; }
        
        /// <summary>
        /// Ürün varyantları
        /// </summary>
        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();
        
        /// <summary>
        /// Kategori
        /// </summary>
        public virtual Category Category { get; private set; }
        
        // Private constructor for EF Core
        private Product() { }
        
        /// <summary>
        /// Yeni bir ürün oluşturur
        /// </summary>
        public Product(string name, string code, string description, int categoryId, int stockQuantity, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Ürün adı boş olamaz.");
                
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Ürün kodu boş olamaz.");
                
            if (stockQuantity < 0)
                throw new DomainException("Stok miktarı negatif olamaz.");
                
            Name = name;
            Code = code;
            Description = description;
            CategoryId = categoryId;
            StockQuantity = stockQuantity;
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// Ürün bilgilerini günceller
        /// </summary>
        public void Update(string name, string description, int categoryId, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Ürün adı boş olamaz.");
                
            Name = name;
            Description = description;
            CategoryId = categoryId;
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// Stok miktarını günceller
        /// </summary>
        public void UpdateStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                throw new DomainException("Stok miktarı yetersiz.");
                
            StockQuantity += quantity;
            
            // Domain event yayınla
            AddDomainEvent(new ProductStockChangedEvent(Id, StockQuantity));
        }
        
        /// <summary>
        /// Varyant ekler
        /// </summary>
        public void AddVariant(ProductVariant variant)
        {
            if (_variants.Any(v => v.VariantCode == variant.VariantCode))
                throw new DomainException($"'{variant.VariantCode}' kodlu varyant zaten mevcut.");
                
            _variants.Add(variant);
        }
        
        /// <summary>
        /// Varyant siler
        /// </summary>
        public void RemoveVariant(string variantCode)
        {
            var variant = _variants.FirstOrDefault(v => v.VariantCode == variantCode);
            if (variant == null)
                throw new DomainException($"'{variantCode}' kodlu varyant bulunamadı.");
                
            _variants.Remove(variant);
        }
    }
}
```

### 2. Value Objects Oluşturulması

```csharp
// src/Stock.Domain/ValueObjects/Money.cs
namespace Stock.Domain.ValueObjects
{
    /// <summary>
    /// Para birimi ve miktar value object'i
    /// </summary>
    public class Money : ValueObject
    {
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; private set; }
        
        /// <summary>
        /// Para birimi
        /// </summary>
        public string Currency { get; private set; }
        
        // Private constructor for EF Core
        private Money() { }
        
        /// <summary>
        /// Yeni bir Money nesnesi oluşturur
        /// </summary>
        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Para birimi boş olamaz.", nameof(currency));
                
            Amount = amount;
            Currency = currency;
        }
        
        /// <summary>
        /// Para birimlerini toplar
        /// </summary>
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Farklı para birimleri toplanamaz.");
                
            return new Money(left.Amount + right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Para birimlerini çıkarır
        /// </summary>
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Farklı para birimleri çıkarılamaz.");
                
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Para birimini bir sayı ile çarpar
        /// </summary>
        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }
        
        /// <summary>
        /// Value object karşılaştırması için kullanılır
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
        
        /// <summary>
        /// String temsilini döndürür
        /// </summary>
        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }
}
```

### 3. Domain Events Oluşturulması

```csharp
// src/Stock.Domain/Events/ProductStockChangedEvent.cs
namespace Stock.Domain.Events
{
    /// <summary>
    /// Ürün stok değişikliği domain olayı
    /// </summary>
    public class ProductStockChangedEvent : DomainEvent
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public int ProductId { get; }
        
        /// <summary>
        /// Yeni stok miktarı
        /// </summary>
        public int NewStockQuantity { get; }
        
        /// <summary>
        /// Yeni bir ProductStockChangedEvent oluşturur
        /// </summary>
        public ProductStockChangedEvent(int productId, int newStockQuantity)
        {
            ProductId = productId;
            NewStockQuantity = newStockQuantity;
        }
    }
}
```

### 4. Repository Interfaces Oluşturulması

```csharp
// src/Stock.Domain/Interfaces/IProductRepository.cs
namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Ürün repository arayüzü
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Ürün koduna göre ürün getirir
        /// </summary>
        Task<Product> GetByCodeAsync(string code);
        
        /// <summary>
        /// Kategori ID'sine göre ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        
        /// <summary>
        /// Stok miktarı belirtilen değerin altında olan ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
    }
}
```

### 5. Domain Services Oluşturulması

```csharp
// src/Stock.Domain/Services/StockAllocationService.cs
namespace Stock.Domain.Services
{
    /// <summary>
    /// Stok tahsisi domain servisi
    /// </summary>
    public class StockAllocationService
    {
        /// <summary>
        /// Sipariş için stok tahsisi yapar
        /// </summary>
        public bool AllocateStock(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            if (quantity <= 0)
                throw new ArgumentException("Miktar pozitif olmalıdır.", nameof(quantity));
                
            if (product.StockQuantity < quantity)
                return false;
                
            product.UpdateStock(-quantity);
            return true;
        }
        
        /// <summary>
        /// Stok tahsisini iptal eder
        /// </summary>
        public void DeallocateStock(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            if (quantity <= 0)
                throw new ArgumentException("Miktar pozitif olmalıdır.", nameof(quantity));
                
            product.UpdateStock(quantity);
        }
    }
}
```

### 6. Application Services (CQRS ile Entegrasyon)

```csharp
// src/Stock.Application/Features/Products/Commands/CreateProduct/CreateProductCommand.cs
namespace Stock.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Yeni ürün oluşturma komutu
    /// </summary>
    public class CreateProductCommand : IRequest<int>
    {
        /// <summary>
        /// Ürün adı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Ürün kodu
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// Ürün açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Kategori ID
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// Stok miktarı
        /// </summary>
        public int StockQuantity { get; set; }
        
        /// <summary>
        /// Birim fiyat miktarı
        /// </summary>
        public decimal UnitPriceAmount { get; set; }
        
        /// <summary>
        /// Birim fiyat para birimi
        /// </summary>
        public string UnitPriceCurrency { get; set; }
    }
}
```

```csharp
// src/Stock.Application/Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs
namespace Stock.Application.Features.Products.Commands.CreateProduct
{
    /// <summary>
    /// Yeni ürün oluşturma komut işleyicisi
    /// </summary>
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        
        /// <summary>
        /// Yeni bir CreateProductCommandHandler örneği oluşturur
        /// </summary>
        public CreateProductCommandHandler(IUnitOfWork unitOfWork, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        
        /// <summary>
        /// Komutu işler
        /// </summary>
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInfo($"Yeni ürün oluşturuluyor: {request.Code}");
            
            // Ürün kodunun benzersiz olduğunu kontrol et
            var existingProduct = await _unitOfWork.Products.GetByCodeAsync(request.Code);
            if (existingProduct != null)
            {
                _logger.LogWarn($"Ürün kodu zaten mevcut: {request.Code}");
                throw new ApplicationException($"'{request.Code}' ürün kodu zaten kullanılıyor.");
            }
            
            // Money value object'i oluştur
            var unitPrice = new Money(request.UnitPriceAmount, request.UnitPriceCurrency);
            
            // Yeni ürün domain entity'si oluştur
            var product = new Product(
                request.Name,
                request.Code,
                request.Description,
                request.CategoryId,
                request.StockQuantity,
                unitPrice);
            
            // Ürünü veritabanına ekle
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInfo($"Ürün başarıyla oluşturuldu. ID: {product.Id}");
            
            return product.Id;
        }
    }
}
```

## Sonuç ve Faydalar

Domain-Driven Design prensiplerinin uygulanması, aşağıdaki faydaları sağlayacaktır:

1. **İş Mantığının Netleşmesi**: Domain modelleri, iş mantığını açık ve anlaşılır bir şekilde temsil eder.
2. **Bakım Kolaylığı**: Kodun organizasyonu, değişikliklerin daha kolay yapılmasını sağlar.
3. **Ölçeklenebilirlik**: Bounded context'ler, sistemin bağımsız olarak ölçeklendirilebilir parçalara bölünmesini sağlar.
4. **Test Edilebilirlik**: Domain mantığı, altyapı detaylarından ayrıldığı için daha kolay test edilebilir.
5. **İletişim İyileştirmesi**: Ubiquitous language, teknik ekip ve domain uzmanları arasındaki iletişimi iyileştirir.

Bu yaklaşım, CQRS ve Mediator pattern ile birlikte kullanıldığında, sistemin modülerliğini, bakım yapılabilirliğini ve ölçeklenebilirliğini önemli ölçüde artıracaktır. 