## SpecStory

## Veritabanı Yapısı İncelemesi (2/18/2025, 10:32:20 PM)

_**User**_

tüm projeyi bir tara bakalım ve biz veritabanında ilişkisel veritabanı yapısı kullanıyor muyuz ? 

---

_**Assistant**_

Projeyi tarayıp veritabanı yapısını kontrol edeceğim.

İlk olarak backend tarafındaki DbContext ve model sınıflarını inceleyeceğim.

---

_****_

<details>
            <summary>Searched codebase "DbContext, entity model classes, database relationships" • **0** results</summary>
        
No results found
</details>

---

_****_



Şimdi doğrudan StockContext.cs dosyasını inceleyelim:

---

_****_

Read file: backend/StockAPI/Data/StockContext.cs

---

_****_



Şimdi de model sınıflarını kontrol edelim:

---

_****_

<details>
            <summary>Searched codebase "class User, class Product, class Category, class Role in Models folder" • **15** results</summary>
        
| File | Lines |
|------|-------|
| `backend/StockAPI/Models/Role.cs` | L1-15 |
| `backend/StockAPI/Models/User.cs` | L1-23 |
| `backend/StockAPI/Models/DTOs/UserDto.cs` | L1-34 |
| `backend/StockAPI/Models/Category.cs` | L1-12 |
| `backend/StockAPI/Models/Product.cs` | L1-14 |
| `OneDrive/Masaüstü/Stock/backend/StockAPI/Models/User.cs` | L1-23 |
| `OneDrive/Masaüstü/Stock/backend/StockAPI/Models/Category.cs` | L1-12 |
| `OneDrive/Masaüstü/Stock/backend/StockAPI/Models/Product.cs` | L1-14 |
| `backend/StockAPI/Models/Base/BaseEntity.cs` | L1-19 |
| `backend/StockAPI/Models/BaseEntity.cs` | L1-16 |
| `backend/StockAPI/Models/AuditLog.cs` | L1-34 |
| `OneDrive/Masaüstü/Stock/backend/StockAPI/Models/BaseEntity.cs` | L1-10 |
| `backend/StockAPI/Models/ITEquipment.cs` | L1-51 |
| `backend/StockAPI/Models/JwtSettings.cs` | L1-12 |
| `backend/StockAPI/Models/DTOs/ITEquipmentDto.cs` | L1-48 |

</details>

---

_****_



Evet, projede ilişkisel veritabanı yapısı kullanılıyor. İncelediğim kodlara göre Entity Framework Core ile PostgreSQL kullanılarak ilişkisel bir veritabanı yapısı kurulmuş. İşte tespit ettiğim ilişkiler:

1. **User - Role İlişkisi**:
   - Bir kullanıcının bir rolü olabilir (One-to-One)
   - Bir rolün birden fazla kullanıcısı olabilir (One-to-Many)
   - `User` sınıfında `RoleId` ve `Role` navigation property'si var
   - `Role` sınıfında `Users` collection property'si var

2. **Category - Product İlişkisi**:
   - Bir kategorinin birden fazla ürünü olabilir (One-to-Many)
   - Bir ürün sadece bir kategoriye ait olabilir
   - `Product` sınıfında `CategoryId` ve `Category` navigation property'si var
   - `Category` sınıfında `Products` collection property'si var

3. **Category - SubCategory İlişkisi (Self-Referencing)**:
   - Kategoriler hiyerarşik bir yapıda
   - Bir kategorinin alt kategorileri olabilir
   - Bir kategorinin bir üst kategorisi olabilir
   - `ParentCategoryId` ve `ParentCategory` navigation property'si var
   - `SubCategories` collection property'si var

4. **Audit Logging**:
   - Sistem işlemlerinin loglanması için `AuditLog` tablosu mevcut
   - Entity'lerin değişiklik geçmişini tutuyor

5. **Base Entity Yapısı**:
   - Tüm entity'ler `BaseEntity` sınıfından türüyor
   - Ortak alanlar: `Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`

Veritabanı ilişkileri `StockContext.cs` dosyasında `OnModelCreating` metodunda Fluent API kullanılarak konfigüre edilmiş. Örneğin:

- Benzersiz indeksler tanımlanmış (Username, CategoryName, ProductName, SerialNumber gibi)
- Foreign key ilişkileri tanımlanmış
- Silme davranışları belirlenmiş (örn: `OnDelete(DeleteBehavior.SetNull)`)

Bu yapı, tam anlamıyla ilişkisel bir veritabanı modeli kullanıldığını gösteriyor ve ACID prensiplerini destekleyen, veri bütünlüğünü koruyan bir sistem kurulmuş.

---



---

