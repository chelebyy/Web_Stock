# Update/Delete Command Handler'larında Specification Kullanımı

**Tarih:** 25.08.2024

**Amaç:** `UpdateProductCommandHandler` ve `DeleteProductCommandHandler` sınıflarında, entity'leri almak için kullanılan doğrudan `GetByIdAsync` metodunu Specification Pattern ile değiştirmek.

## Yapılan Değişiklikler

1.  **`UpdateProductCommandHandler.cs` Güncellemesi:**
    *   Ürün entity'sini almak için `_unitOfWork.Products.GetByIdAsync(request.Id)` yerine aşağıdaki kod kullanıldı:
        ```csharp
        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken);
        ```
    *   Kategori varlığını kontrol etmek için de `CategoryByIdSpecification` kullanıldı.

2.  **`DeleteProductCommandHandler.cs` Güncellemesi:**
    *   Ürün entity'sini almak için `productRepo.GetByIdAsync(request.Id)` yerine aşağıdaki kod kullanıldı:
        ```csharp
        var productRepo = _unitOfWork.GetRepository<Product>();
        var product = await productRepo.FirstOrDefaultAsync(new ProductByIdSpecification(request.Id), cancellationToken);
        ```
    *   Tutarlılık için `DeleteAsync` metodu kullanıldı (soft delete işlemi için).

3.  **Gerekli Specification Sınıflarının Oluşturulması:**
    *   `CategoryByIdSpecification`: Kategori ID'sine göre filtreleme yapan specification.
    *   `ProductByIdSpecification`: Ürün ID'sine göre filtreleme yapan specification.
        ```csharp
        public class ProductByIdSpecification : BaseSpecification<Product>
        {
            public ProductByIdSpecification(int productId)
                : base(p => p.Id == productId)
            {
                // Simple specification that filters products by their ID
            }
        }
        ```

## Karşılaşılan Sorunlar ve Çözümleri

1.  **Derleme Hataları:**
    *   Projedeki `ProductByIdSpecification` sınıfı bulunamadığından CS0246 hatası alındı.
    *   Çözüm: `src/Stock.Domain/Specifications/Products/` klasörü altına `ProductByIdSpecification.cs` dosyası oluşturularak bu hata çözüldü.

## Elde Edilen Faydalar

1.  **Daha Temiz Kod:** Repository deseni daha anlamlı ve okunabilir hale geldi.
2.  **Sorgu Mantığının Merkezi Yönetimi:** Filtreleme kriterleri specification sınıflarında toplandı.
3.  **Kod Tekrarını Önleme:** Aynı filtreleme kriterleri farklı yerlerde tekrar tekrar yazılmak yerine, specification sınıflarından yeniden kullanılıyor.
4.  **Genişletilebilirlik:** İleride daha karmaşık filtreleme ihtiyaçları olduğunda, specification'lar kolayca güncellenebilir veya genişletilebilir.

## Sonraki Adımlar

1.  Diğer entity'ler için de benzer specification'ların oluşturulması değerlendirilebilir.
2.  `FirstOrDefaultAsync` metodunun yerine, bulunmadığında NotFoundException fırlatan bir extension method oluşturulabilir (örneğin, `FindOrThrowAsync`). 