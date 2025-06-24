# CQRS Pattern: Ürün Yönetimi

Bu belge, Stock projesindeki Ürün Yönetimi (Product Management) özelliği için Command Query Responsibility Segregation (CQRS) deseninin nasıl uygulandığını detaylandırır.

## 1. Genel Bakış

Ürün yönetimi işlemleri (oluşturma, güncelleme, silme, listeleme, detay görme) için CQRS deseni kullanılmıştır. Bu sayede yazma (Command) ve okuma (Query) operasyonları birbirinden ayrılmış, kodun okunabilirliği ve bakımı kolaylaştırılmıştır. MediatR kütüphanesi, komut ve sorguların ilgili işleyicilere (handler) yönlendirilmesi için kullanılmıştır.

## 2. Bileşenler

### 2.1. Komutlar (Commands)

Yazma operasyonlarını temsil ederler.

-   **`CreateProductCommand`**: Yeni bir ürün oluşturma isteğini temsil eder.
    -   **Alanlar:** `Name`, `Description`, `CategoryId`
    -   **Dönüş Tipi:** `ProductDto` (Oluşturulan ürünün DTO'su)
    -   **İşleyici:** `CreateProductCommandHandler`
-   **`UpdateProductCommand`**: Mevcut bir ürünü güncelleme isteğini temsil eder.
    -   **Alanlar:** `Id`, `Name`, `Description`, `CategoryId`
    -   **Dönüş Tipi:** `Unit` (Başarı durumu)
    -   **İşleyici:** `UpdateProductCommandHandler`
-   **`DeleteProductCommand`**: Bir ürünü silme isteğini temsil eder.
    -   **Alanlar:** `Id`
    -   **Dönüş Tipi:** `Unit` (Başarı durumu)
    *   **İşleyici:** `DeleteProductCommandHandler`

### 2.2. Sorgular (Queries)

Okuma operasyonlarını temsil ederler.

-   **`GetAllProductsQuery`**: Tüm ürünleri listeleme isteğini temsil eder.
    -   **Alanlar:** Yok (ileride filtreleme/sayfalama eklenebilir)
    -   **Dönüş Tipi:** `IEnumerable<ProductDto>`
    -   **İşleyici:** `GetAllProductsQueryHandler`
-   **`GetProductByIdQuery`**: Belirli bir ID'ye sahip ürünü getirme isteğini temsil eder.
    -   **Alanlar:** `Id`
    -   **Dönüş Tipi:** `ProductDto?` (Nullable, ürün bulunamazsa null döner)
    -   **İşleyici:** `GetProductByIdQueryHandler`

### 2.3. İşleyiciler (Handlers)

Komutları ve sorguları işleyen sınıflardır. `IUnitOfWork`, ilgili Repository arayüzleri (`IProductRepository`, `ICategoryRepository` vb.) ve `IMapper` gibi bağımlılıkları kullanarak iş mantığını uygularlar.

-   **`CreateProductCommandHandler`**: `CreateProductCommand`'ı işler. Kategori varlığını kontrol eder, ürünü oluşturur, veritabanına kaydeder ve DTO'sunu döndürür.
-   **`UpdateProductCommandHandler`**: `UpdateProductCommand`'ı işler. Ürünün varlığını ve kategori varlığını kontrol eder, AutoMapper ile güncellemeyi uygular, `LastModifiedDate`'i ayarlar ve değişiklikleri kaydeder.
-   **`DeleteProductCommandHandler`**: `DeleteProductCommand`'ı işler. Ürünün varlığını kontrol eder ve veritabanından siler.
-   **`GetAllProductsQueryHandler`**: `GetAllProductsQuery`'yi işler. `ProductsWithCategorySpecification` kullanarak ürünleri ve ilişkili kategori bilgilerini çeker, AutoMapper ile DTO listesine dönüştürür.
-   **`GetProductByIdQueryHandler`**: `GetProductByIdQuery`'yi işler. `ProductByIdWithCategorySpecification` kullanarak ürünü ve ilişkili kategori bilgisini çeker, AutoMapper ile DTO'ya dönüştürür veya null döndürür.

### 2.4. Veri Transfer Nesneleri (DTOs)

API katmanı ile Application katmanı arasında veri taşımak için kullanılırlar.

-   **`ProductDto`**: Ürün bilgilerini (ID, Name, Description, CategoryId, CategoryName, CreatedDate, LastModifiedDate) içerir.
-   **`CreateProductDto`**: Yeni ürün oluşturma isteği için gerekli alanları (Name, Description, CategoryId) ve validasyonları içerir.
-   **`UpdateProductDto`**: Ürün güncelleme isteği için gerekli alanları (Id, Name, Description, CategoryId) içerir.

### 2.5. Spesifikasyonlar (Specifications)

Repository katmanında sorgu kriterlerini merkezi ve yeniden kullanılabilir hale getirmek için kullanılırlar.

-   **`ProductsWithCategorySpecification`**: Tüm ürünleri getirirken ilişkili `Category` bilgisini de dahil eder.
-   **`ProductByIdWithCategorySpecification`**: Belirli bir ID'ye sahip ürünü getirirken ilişkili `Category` bilgisini de dahil eder.

## 3. API Entegrasyonu (`ProductsController`)

`Stock.API` projesindeki `ProductsController`, HTTP isteklerini alır ve ilgili komut veya sorguları `IMediator` aracılığıyla Application katmanındaki işleyicilere gönderir. Sonuçları uygun HTTP yanıtlarına (Ok, NotFound, CreatedAtAction, NoContent, BadRequest) dönüştürür.

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet] // GET: api/products
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        var query = new GetAllProductsQuery();
        return Ok(await _mediator.Send(query));
    }

    [HttpGet("{id}")] // GET: api/products/5
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost] // POST: api/products
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var createdProduct = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")] // PUT: api/products/5
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE: api/products/5
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}
```

## 4. Sonuç

Ürün yönetimi için CQRS deseninin uygulanması, kodun daha modüler ve sürdürülebilir olmasını sağlamıştır. Yazma ve okuma sorumluluklarının ayrılması, her bir operasyonun bağımsız olarak geliştirilmesine ve optimize edilmesine olanak tanır. 