# İzin Yönetimi (Permission Management) CQRS Uygulaması

**Son Güncelleme:** 24.06.2025

Bu belge, `Permission` özelliğinin temel CRUD (Create, Read, Update, Delete) operasyonlarının **CQRS (Command Query Responsibility Segregation)** ve **MediatR** desenleri kullanılarak nasıl yeniden yapılandırıldığını açıklar.

## 1. Genel Bakış

Bu çalışma, `PermissionsController`'ın doğrudan servis ve DbContext bağımlılıklarını azaltarak daha temiz, test edilebilir ve sürdürülebilir bir yapıya kavuşmasını amaçlamıştır. İş mantığı, `Application` katmanındaki `Command` ve `Query` işleyicilerine (Handler) taşınmıştır.

## 2. Uygulanan Değişiklikler

### 2.1. Query (Sorgu) Tarafı

#### 2.1.1. `GetAllPermissionsQuery`
- Bu sorgu ve işleyicisi zaten mevcuttu ve `AllPermissionsSpecification` kullanarak tüm izinleri getiriyordu. Yapısı doğru olduğu için üzerinde bir değişiklik yapılmadı.

#### 2.1.2. `GetPermissionByIdQuery` (Yeni)
- **Amaç:** Tek bir izni ID'sine göre getirmek.
- **Dosyalar:**
    - `src/Stock.Application/Features/Permissions/Queries/GetPermissionById/GetPermissionByIdQuery.cs`: ID parametresini taşıyan ve `IRequest<PermissionDto>` arayüzünü implemente eden sorgu sınıfı.
    - `src/Stock.Application/Features/Permissions/Queries/GetPermissionById/GetPermissionByIdQueryHandler.cs`: `IUnitOfWork` kullanarak `Permission`'ı `PermissionByIdSpecification` ile getiren ve sonucu `PermissionDto`'ya map eden işleyici.
- **Specification:**
    - `src/Stock.Domain/Specifications/Permissions/PermissionByIdSpecification.cs`: Bir izni ID'sine göre filtrelemek için oluşturulan yeni spesifikasyon sınıfı.

### 2.2. Command (Komut) Tarafı

Tüm komut işleyicileri (`Create`, `Update`, `Delete`), doğrudan `IPermissionRepository` bağımlılığı yerine `IUnitOfWork` üzerinden repository'ye erişecek şekilde yeniden düzenlenmiştir. Bu, veritabanı işlemlerinde tutarlılık ve tek bir işlem kapsamı (transaction scope) sağlar.

#### 2.2.1. `CreatePermissionCommand`
- **Handler (`CreatePermissionCommandHandler.cs`):**
    - `IPermissionRepository` bağımlılığı kaldırıldı.
    - `_unitOfWork.GetRepository<Permission>()` ile repository'ye erişim sağlandı.
    - `Permission.Create` factory metodu ile domain katmanındaki iş kuralları korundu.

#### 2.2.2. `UpdatePermissionCommand`
- **Handler (`UpdatePermissionCommandHandler.cs`):**
    - `IPermissionRepository` bağımlılığı kaldırıldı.
    - `PermissionByIdSpecification` kullanılarak güncellenecek entity, `_unitOfWork` üzerinden çekildi.
    - Entity'nin `UpdateDescription`, `UpdateGroup` gibi kendi içindeki domain metotları kullanılarak güncelleme yapıldı.
    - Entity'nin adı (`Name`) birincil anahtar gibi kabul edildiğinden güncellenmesi mantığı kaldırıldı.

#### 2.2.3. `DeletePermissionCommand`
- **Handler (`DeletePermissionCommandHandler.cs`):**
    - `IPermissionRepository` bağımlılığı kaldırıldı.
    - `PermissionByIdSpecification` kullanılarak silinecek entity, `_unitOfWork` üzerinden bulundu.
    - `_unitOfWork.GetRepository<Permission>().DeleteAsync()` metodu ile silme işlemi gerçekleştirildi.

### 2.3. API Katmanı (`PermissionsController.cs`)

- `GetPermissionById` adında yeni bir `GET` endpoint'i eklendi. Bu endpoint, `GetPermissionByIdQuery`'yi `IMediator` aracılığıyla gönderir.
- Mevcut `Create`, `Update`, `Delete` endpoint'leri zaten `IMediator` kullandığı için bu kısımlarda bir değişiklik yapılmadı.
- Controller'daki `IPermissionService` ve `ApplicationDbContext` bağımlılıkları, henüz CQRS'e taşınmamış diğer metotlar (örn: `AssignPermissionsToRole`) tarafından kullanıldığı için şimdilik korunmuştur.

## 3. Sonraki Adımlar

- `PermissionsController`'da kalan ve hala `IPermissionService` kullanan diğer endpoint'lerin (izin atama, gruplama vb.) de CQRS yapısına taşınması planlanmaktadır.
- Bu işlemler tamamlandıktan sonra `IPermissionService` ve controller'daki `DbContext` bağımlılığı tamamen kaldırılabilecektir. 