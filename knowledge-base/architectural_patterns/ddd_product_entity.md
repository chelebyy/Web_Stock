# Product Entity için Domain-Driven Design (DDD) Uygulaması

Bu belge, `Product` entity'si üzerinde Domain-Driven Design (DDD) prensiplerinin nasıl uygulandığını detaylandırmaktadır. Amaç, domain modelini zenginleştirmek, iş kurallarını zorunlu kılmak ve kodun sürdürülebilirliğini artırmaktır.

## 1. Value Objects (Değer Nesneleri)

Domain'deki belirli kavramları temsil eden ve kimlik yerine değerleri ile tanımlanan küçük, değişmez nesnelerdir. `Product` entity'si için aşağıdaki Value Object'ler tanımlanmıştır:

### 1.1. `ProductName`

- **Amaç:** Ürün adını temsil eder. Boş olamaz kuralını içerir.
- **Özellikler:**
    - `Value` (string): Ürün adı.
- **Davranış:**
    - Boş veya null kontrolü yapar.
    - Eşitlik karşılaştırması (`Equals`, `==`, `!=`) değer üzerinden yapılır.

```csharp
// Stock.Domain/ValueObjects/ProductName.cs
public class ProductName : ValueObject
{
    public string Value { get; }

    private ProductName(string value)
    {
        Value = value;
    }

    public static Result<ProductName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<ProductName>(DomainErrors.Product.NameEmpty);
        }
        // Gerekirse başka kurallar (uzunluk vb.) eklenebilir.
        return new ProductName(name);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    // Implicit conversion for convenience (optional)
    public static implicit operator string(ProductName name) => name.Value;
}
```

### 1.2. `ProductDescription`

- **Amaç:** Ürün açıklamasını temsil eder. Null olabilir ancak boş string olamaz.
- **Özellikler:**
    - `Value` (string): Ürün açıklaması.
- **Davranış:**
    - Boş string kontrolü yapar.
    - Eşitlik karşılaştırması değer üzerinden yapılır.

```csharp
// Stock.Domain/ValueObjects/ProductDescription.cs
public class ProductDescription : ValueObject
{
    public string Value { get; }

    private ProductDescription(string value)
    {
        // Null kabul edilebilir, ancak boş string olmamalıdır.
        // Eğer null da istenmiyorsa, ProductName gibi kontrol eklenir.
        Value = value;
    }

     public static Result<ProductDescription> Create(string description)
     {
         if (description == string.Empty) // Sadece boş string kontrolü
         {
             // DomainErrors.Product.DescriptionEmpty gibi bir hata tanımlanabilir.
             return Result.Failure<ProductDescription>("Product description cannot be empty.");
         }
         // Gerekirse başka kurallar (uzunluk vb.) eklenebilir.
         return new ProductDescription(description);
     }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

     public override string ToString() => Value;

    // Implicit conversion for convenience (optional)
    public static implicit operator string(ProductDescription description) => description.Value;
}
```

### 1.3. `StockLevel`

- **Amaç:** Stok seviyesini temsil eder. Negatif olamaz.
- **Özellikler:**
    - `Value` (int): Stok miktarı.
- **Davranış:**
    - Negatif değer kontrolü yapar.
    - Stok artırma/azaltma işlemleri için metotlar içerebilir.
    - Eşitlik karşılaştırması değer üzerinden yapılır.

```csharp
// Stock.Domain/ValueObjects/StockLevel.cs
public class StockLevel : ValueObject
{
    public int Value { get; }

    private StockLevel(int value)
    {
        Value = value;
    }

    public static Result<StockLevel> Create(int stock)
    {
        if (stock < 0)
        {
            return Result.Failure<StockLevel>(DomainErrors.Product.StockCannotBeNegative);
        }
        return new StockLevel(stock);
    }

    // Örnek stok işlemleri
    public Result<StockLevel> AddStock(int amount)
    {
        if (amount <= 0)
        {
            return Result.Failure<StockLevel>(DomainErrors.Product.StockToAddMustBePositive);
        }
        return Create(Value + amount);
    }

    public Result<StockLevel> RemoveStock(int amount)
    {
        if (amount <= 0)
        {
            return Result.Failure<StockLevel>(DomainErrors.Product.StockToRemoveMustBePositive);
        }
        if (Value < amount)
        {
            return Result.Failure<StockLevel>(DomainErrors.Product.InsufficientStock);
        }
        return Create(Value - amount);
    }


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

     // Implicit conversion for convenience (optional)
    public static implicit operator int(StockLevel stockLevel) => stockLevel.Value;
}
```

## 2. Entity Refactoring (`Product`)

`Product` entity'si, Value Object'leri kullanacak ve değişmezlerini (invariants) koruyacak şekilde yeniden düzenlenmiştir.

- **Constructor:** `private` yapıldı. Entity oluşturma işlemi Factory Metodu üzerinden zorunlu kılındı.
- **Factory Metodu (`Create`):** Entity'nin geçerli bir durumda oluşturulmasını sağlar. Value Object oluşturma adımlarını içerir ve başarısızlık durumunda `Result` döner.
- **Özellikler (Properties):** Value Object türlerini kullanır. `private set;` ile dışarıdan doğrudan değiştirilmesi engellenmiştir. Entity'nin state'i, kendi metotları aracılığıyla değiştirilir.
- **Metotlar (`UpdateName`, `UpdateDescription`, `UpdateStock` vb.):** Entity'nin durumunu değiştiren operasyonlar için özel metotlar tanımlanmıştır. Bu metotlar gerekli iş kurallarını uygular ve Value Object'leri günceller.

```csharp
// Stock.Domain/Entities/Product.cs
public class Product : BaseAuditableEntity // veya BaseEntity
{
    public ProductName Name { get; private set; }
    public ProductDescription Description { get; private set; }
    public StockLevel Stock { get; private set; }
    public bool IsActive { get; private set; }

    // İlişkili entity'ler (varsa)
    // public int CategoryId { get; private set; }
    // public Category Category { get; private set; }

    // EF Core için private constructor
    private Product() { }

    private Product(ProductName name, ProductDescription description, StockLevel stock, bool isActive)
    {
        Name = name;
        Description = description;
        Stock = stock;
        IsActive = isActive;
    }

    // Factory Metodu
    public static Result<Product> Create(string name, string description, int stock, bool isActive = true)
    {
        var nameResult = ProductName.Create(name);
        if (nameResult.IsFailure) return Result.Failure<Product>(nameResult.Error);

        var descriptionResult = ProductDescription.Create(description);
        // Description null olabileceğinden, başarısızlık durumu farklı ele alınabilir
        // veya null ise varsayılan bir değer atanabilir. Şimdilik başarılı kabul edelim.
        // if (descriptionResult.IsFailure) return Result.Failure<Product>(descriptionResult.Error);

        var stockResult = StockLevel.Create(stock);
        if (stockResult.IsFailure) return Result.Failure<Product>(stockResult.Error);

        // Null description durumunda varsayılan davranış (örneğin null atama)
        var descValue = descriptionResult.IsSuccess ? descriptionResult.Value : null;


        var product = new Product(nameResult.Value, descValue, stockResult.Value, isActive);

        // Domain Event eklenebilir: product.AddDomainEvent(new ProductCreatedEvent(product));

        return Result.Success(product);
    }

    // Entity davranışlarını yöneten metotlar
    public Result UpdateName(string newName)
    {
        var nameResult = ProductName.Create(newName);
        if (nameResult.IsFailure) return Result.Failure(nameResult.Error);
        Name = nameResult.Value;
        return Result.Success();
    }

     public Result UpdateDescription(string newDescription)
    {
        var descriptionResult = ProductDescription.Create(newDescription);
        // if (descriptionResult.IsFailure) return Result.Failure(descriptionResult.Error); // Hata yönetimi eklenebilir
        Description = descriptionResult.IsSuccess ? descriptionResult.Value : null;
        return Result.Success();
    }

    public Result UpdateStock(int newStock)
    {
        var stockResult = StockLevel.Create(newStock);
        if (stockResult.IsFailure) return Result.Failure(stockResult.Error);
        Stock = stockResult.Value;
        return Result.Success();
    }

     public Result AddStock(int amount)
    {
        var updatedStockResult = Stock.AddStock(amount);
        if (updatedStockResult.IsFailure) return Result.Failure(updatedStockResult.Error);
        Stock = updatedStockResult.Value;
        return Result.Success();
    }

    public Result RemoveStock(int amount)
    {
         var updatedStockResult = Stock.RemoveStock(amount);
        if (updatedStockResult.IsFailure) return Result.Failure(updatedStockResult.Error);
        Stock = updatedStockResult.Value;
        return Result.Success();
    }


    public void Activate()
    {
        IsActive = true;
        // Domain Event: product.AddDomainEvent(new ProductActivatedEvent(this.Id));
    }

    public void Deactivate()
    {
        IsActive = false;
         // Domain Event: product.AddDomainEvent(new ProductDeactivatedEvent(this.Id));
    }
}
```

## 3. Entity Framework Core Yapılandırması (`ProductConfiguration`)

Value Object'lerin veritabanına düzgün bir şekilde eşlenmesi için EF Core'un `IEntityTypeConfiguration` arayüzü kullanılmıştır. Value Object'ler genellikle **Complex Types** (EF Core 8+) veya **Owned Types** (önceki sürümler) olarak eşlenir. Burada Complex Types yaklaşımı gösterilmiştir.

```csharp
// Stock.Infrastructure/Data/Config/ProductConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects; // Value Objects namespace'i

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        // ProductName Value Object'inin eşlenmesi (Complex Type)
        builder.ComplexProperty(p => p.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .HasColumnName("Name") // Veritabanındaki kolon adı
                .HasMaxLength(200) // Maksimum uzunluk (varsa)
                .IsRequired();
        });

        // ProductDescription Value Object'inin eşlenmesi (Complex Type)
        builder.ComplexProperty(p => p.Description, descriptionBuilder =>
        {
            descriptionBuilder.Property(d => d.Value)
                .HasColumnName("Description")
                .HasMaxLength(1000) // Maksimum uzunluk (varsa)
                .IsRequired(false); // Açıklama null olabilir
        });

         // StockLevel Value Object'inin eşlenmesi (Complex Type)
        builder.ComplexProperty(p => p.Stock, stockBuilder =>
        {
             stockBuilder.Property(s => s.Value)
                .HasColumnName("Stock")
                .IsRequired();
        });

        builder.Property(p => p.IsActive)
            .IsRequired();

        // İlişkiler (varsa)
        // builder.HasOne(p => p.Category)
        //     .WithMany() // Veya Category'de Products koleksiyonu varsa WithMany(c => c.Products)
        //     .HasForeignKey(p => p.CategoryId)
        //     .IsRequired(false); // Veya IsRequired()

        // Denetim alanları (BaseAuditableEntity'den geliyorsa Base konfigürasyonunda olabilir)
        builder.Property(p => p.CreatedBy).HasMaxLength(100);
        builder.Property(p => p.LastModifiedBy).HasMaxLength(100);

        // Tablo adı (isteğe bağlı)
        builder.ToTable("Products");
    }
}
```
**Önemli Not:** EF Core 8 ile birlikte gelen **Complex Types**, Value Object'leri eşlemek için daha modern ve esnek bir yol sunar. Owned Types'a göre bazı avantajları vardır (örneğin, complex type'ın kendisi null olabilir). Eğer EF Core 8 veya üzerini kullanıyorsanız Complex Types tercih edilebilir.

## 4. Application Katmanı Üzerindeki Etkisi

DDD prensiplerinin uygulanması Application katmanını da etkiler:

- **DTO'lar (`ProductDto`, `CreateProductDto`, `UpdateProductDto` vb.):** Genellikle primitive tipleri (string, int vb.) kullanmaya devam ederler. API sınırlarında Value Object'leri doğrudan kullanmak genellikle önerilmez.
- **Mapping (`ProductProfile`):** AutoMapper veya benzeri bir kütüphane kullanılıyorsa, Entity'deki Value Object'ler ile DTO'lardaki primitive tipler arasında eşleştirme yapılması gerekir.
    ```csharp
    // Örnek AutoMapper Profili
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Entity -> DTO
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Value))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock.Value));

            // Create DTO -> Product (Bu eşleme genellikle Command Handler içinde yapılır)
            // Update DTO -> Product (Bu eşleme genellikle Command Handler içinde yapılır)
        }
    }
    ```
- **Command/Query Handler'lar:**
    - **Create/Update Handler'lar:** DTO'dan gelen primitive değerleri kullanarak Entity'nin `Create` factory metodunu veya `UpdateXyz` metotlarını çağırırlar. Value Object oluşturma ve doğrulama mantığı Entity içinde kalır.
    - **Query Handler'lar:** Repository'den alınan Entity'leri DTO'lara maplerler.

```csharp
// Örnek CreateProductCommandHandler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    // Constructor...

    public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Entity oluşturma işlemi Entity'nin factory metodu ile yapılır
        var productResult = Product.Create(
            request.Name,
            request.Description,
            request.Stock);

        if (productResult.IsFailure)
        {
            // Handler seviyesinde loglama yapılabilir
            return Result.Failure<ProductDto>(productResult.Error);
        }

        var product = productResult.Value;

        await _unitOfWork.Products.AddAsync(product); // IRepository<Product> üzerinden
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var productDto = _mapper.Map<ProductDto>(product);
        return Result.Success(productDto);
    }
}

// Örnek UpdateProductCommandHandler
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
{
     private readonly IUnitOfWork _unitOfWork;
     // Constructor...

    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id); // Specification ile de alınabilir
        if (product == null)
        {
            return Result.Failure(DomainErrors.Product.NotFound(request.Id));
        }

        // Entity'nin kendi metotları ile güncelleme yapılır
        var nameResult = product.UpdateName(request.Name);
        if (nameResult.IsFailure) return Result.Failure(nameResult.Error);

        var descResult = product.UpdateDescription(request.Description);
        // if (descResult.IsFailure) return Result.Failure(descResult.Error); // Gerekirse

        var stockResult = product.UpdateStock(request.Stock);
        if (stockResult.IsFailure) return Result.Failure(stockResult.Error);

        // _unitOfWork.Products.UpdateAsync(product); // EF Core tracking ile gerek kalmayabilir
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
```

## 5. Sonuç

`Product` entity'si üzerinde DDD prensiplerinin (Value Objects, Entity Refactoring) uygulanması, domain modelini daha anlamlı hale getirmiş, iş kurallarının uygulanmasını sağlamış ve kodun okunabilirliğini ve sürdürülebilirliğini artırmıştır. EF Core yapılandırması ve Application katmanı da bu değişikliklere uyum sağlayacak şekilde güncellenmiştir. Bu yaklaşım, projenin diğer entity'leri için de benzer şekilde uygulanabilir. 