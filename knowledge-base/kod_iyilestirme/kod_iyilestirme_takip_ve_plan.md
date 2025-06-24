# Kod İyileştirme Takip ve Planı

**Not:** Bu belge, Stock projesinin kod kalitesini artırmak için yürütülen iyileştirme çalışmalarının genel hedeflerini, güncel durumunu, gelecek planını ve teknik detaylarını tek bir yerde toplamaktadır. Daha önceki (`kod_iyilestirme_plani.md`, `kod_iyilestirme_guncel_durum_ve_takip.md`, `kod_iyilestirme_yeni_plan.md`) belgelerin yerini alır.

## 1. Genel Bakış ve Hedefler

Bu iyileştirme çalışmasının temel hedefleri şunlardır:

1.  Kod kalitesinin artırılması
2.  Bakım maliyetlerinin düşürülmesi
3.  Hata ayıklama süreçlerinin iyileştirilmesi
4.  Performans optimizasyonu
5.  Güvenlik önlemlerinin güçlendirilmesi

İyileştirmeler, risk seviyelerine göre üç ana fazda ele alınmaktadır.

## 2. Mevcut Durum ve Sonraki Adımlar

Kod iyileştirme çalışmalarının **güncel ve gerçek durumu** aşağıda detaylandırılmıştır.

**Önemli Not:** Para birimi (Currency) özellikleri ve ilgili tüm kodlar projeden kaldırıldı, bu nedenle bu belgedeki Currency ile ilgili tüm planlamalar iptal edilmiştir.

### Gerçek Tamamlanma Oranları (Tahmini):

*   **Faz 1 (Düşük Riskli İyileştirmeler)**: ~100% tamamlandı
*   **Faz 2 (Orta Riskli İyileştirmeler)**: ~100% tamamlandı (Frontend Hata/Yükleme Yönetimi İyileştirmesi, Global Exception Handling, Merkezi Loglama, Shared Components ve Base Service oluşturma tamamlandı)
*   **Faz 3 (Yüksek Riskli İyileştirmeler)**: ~75% tamamlandı (Specification, UoW, Product/Category/Role/User/Permission CQRS ve test temizliği ile)

### Faz 1: Düşük Riskli İyileştirmeler (~75% Tamamlandı)

#### 1.1. Frontend Temizliği

| Görev                                                       | Durum         | Öncelik |
| :---------------------------------------------------------- | :------------ | :------ |
| Gereksiz console.log ifadelerinin kaldırılması              | ✅ Tamamlandı | -       |
| Magic string ve number'ların sabit değişkenlere dönüştürülmesi | ✅ Tamamlandı | -       |
| Kullanılmayan import ifadelerinin kaldırılması                | ✅ Tamamlandı | -       |
| API endpoint uyumluluğu sorunlarının çözülmesi              | ✅ Tamamlandı | -       |
| Hata yönetiminin iyileştirilmesi                            | ✅ Tamamlandı | -       |
| API yanıt işleme iyileştirmesi                              | ✅ Tamamlandı | -       |
| HTTP isteklerinin standartlaştırılması                      | ✅ Tamamlandı | -       |
| Import ifadelerinin düzenlenmesi                            | ✅ Tamamlandı | -       |
| Yorum satırlarının iyileştirilmesi                 | ✅ Tamamlandı | Düşük   |
| Kod formatının düzenlenmesi                       | ✅ Tamamlandı | Düşük   |
| Değişken isimlerinin standardizasyonu             | ✅ Tamamlandı | Düşük   |

#### 1.2. Backend Temizliği

| Görev                                                       | Durum         | Öncelik |
| :---------------------------------------------------------- | :------------ | :------ |
| Gereksiz log ifadelerinin kaldırılması                      | ✅ Tamamlandı | -       |
| Magic string ve number'ların sabit değişkenlere dönüştürülmesi | ✅ Tamamlandı | -       |
| Kullanılmayan using ifadelerinin kaldırılması                | ✅ Tamamlandı | -       |
| Hata yönetiminin iyileştirilmesi                            | ✅ Tamamlandı | -       |
| Yorum satırlarının iyileştirilmesi                 | ✅ Tamamlandı | Düşük   |
| Kod formatının düzenlenmesi                       | ✅ Tamamlandı | Düşük   |
| Değişken isimlerinin standardizasyonu             | ✅ Tamamlandı | Düşük   |

#### 1.3. Belgelendirme İyileştirmeleri

| Görev                                                 | Durum         | Öncelik |
| :---------------------------------------------------- | :------------ | :------ |
| Frontend servis iyileştirmelerinin belgelendirilmesi  | ✅ Tamamlandı | -       |
| Backend iyileştirmelerinin belgelendirilmesi          | ✅ Tamamlandı | Orta    |
| **[Kalan]** Tüm iyileştirmelerin bu dosyada birleşimi | ✅ Tamamlandı | -       |

### Faz 2: Orta Riskli İyileştirmeler (~60% Tamamlandı)

#### 2.1. Global Exception Handling (Backend)

| Görev                                                      | Durum         | Öncelik |
| :--------------------------------------------------------- | :------------ | :------ |
| ExceptionMiddleware oluşturulması                          | ✅ Tamamlandı | -       |
| Hata mesajlarının standartlaştırılması                     | ✅ Tamamlandı | -       |
| Loglama entegrasyonu                                       | ✅ Tamamlandı | -       |
| Farklı ortamlar için farklı hata yanıtları yapılandırılması | ✅ Tamamlandı | -       |

#### 2.2. Merkezi Loglama Mekanizması (Backend)

| Görev                                                          | Durum         | Öncelik |
| :------------------------------------------------------------- | :------------ | :------ |
| NLog kütüphanesi entegrasyonu                                  | ✅ Tamamlandı | -       |
| ILoggerManager arayüzü ve LoggerManager sınıfı oluşturulması | ✅ Tamamlandı | -       |
| Log rotasyonu ve arşivleme yapılandırılması                    | ✅ Tamamlandı | -       |
| Farklı log seviyeleri için metotlar eklenmesi                  | ✅ Tamamlandı | -       |

#### 2.3. Shared Components (Frontend)

| Görev                                              | Durum         | Öncelik |
| :------------------------------------------------- | :------------ | :------ |
| LoadingSpinner bileşeni oluşturulması              | ✅ Tamamlandı | -       |
| ErrorMessage bileşeni oluşturulması                | ✅ Tamamlandı | Yüksek  |
| ConfirmationDialog bileşeni oluşturulması          | ✅ Tamamlandı | Orta    |
| SharedModule oluşturulması                         | ✅ Tamamlandı | -       |
| PrimeNG bileşenlerinin SharedModule'e eklenmesi    | ✅ Tamamlandı | -       |

#### 2.4. Base Service Oluşturma (Frontend)

| Görev                                                              | Durum         | Öncelik |
| :----------------------------------------------------------------- | :------------ | :------ |
| BaseHttpService sınıfı oluşturulması                               | ✅ Tamamlandı | -       |
| HTTP istekleri için ortak metotlar eklenmesi                       | ✅ Tamamlandı | -       |
| Hata yönetiminin merkezileştirilmesi                               | ✅ Tamamlandı | -       |
| API yanıtlarını normalleştirme işlevi eklenmesi                    | ✅ Tamamlandı | -       |
| Tüm servislerin BaseHttpService'i kullanacak şekilde güncellenmesi | ✅ Tamamlandı | Orta    |
| HTTP durum kodları için sabitlerin eklenmesi                       | ✅ Tamamlandı | -       |
| Dosya yükleme işlevi eklenmesi                                     | ✅ Tamamlandı | Düşük   |

#### 2.5. Kalan Orta Riskli Görevler

| Görev                                           | Durum                 | Öncelik | Notlar                                                                                 |
| :---------------------------------------------- | :-------------------- | :------ | :------------------------------------------------------------------------------------- |
| Dependency Injection kullanımının iyileştirilmesi | ✅ Tamamlandı         | Yüksek  | Kontrol edildi, mevcut yapı uygun bulundu.                                           |
| Performans iyileştirmeleri                      | ⬜ Kalan              | Orta    |                                                                                        |

### Faz 3: Yüksek Riskli İyileştirmeler (~75% Tamamlandı)

#### 3.1. Veritabanı Sorgu Optimizasyonları (Backend)

| Görev                              | Durum                 | Öncelik | Notlar                                                                                                                               |
| :--------------------------------- | :-------------------- | :------ | :----------------------------------------------------------------------------------------------------------------------------------- |
| UserRepository Optimizasyonu       | ✅ Tamamlandı         | Yüksek  | Specification Pattern implemente edildi. IRepository, GenericRepository, IUserRepository, UserRepository ve ilgili Handler'lar güncellendi. |
| RoleRepository Optimizasyonu       | ✅ Tamamlandı         | Orta    | Specification Pattern uygulandı. IRoleRepository, RoleRepository ve ilgili Handler'lar güncellendi/oluşturuldu.                      |
| PermissionRepository Optimizasyonu | ✅ Tamamlandı         | Orta    | Specification Pattern uygulandı. IPermissionRepository, PermissionRepository ve ilgili Handler'lar güncellendi/oluşturuldu.            |
| **ProductRepository Optimizasyonu** | ✅ Tamamlandı         | Orta    | Specification Pattern uygulandı. IProductRepository, ProductRepository ve ilgili Handler'lar güncellendi/oluşturuldu. Price özelliği projeden tamamen kaldırıldı.          |
| **Repository Performans Optimizasyonu** | ✅ Tamamlandı         | Yüksek  | GenericRepository sınıfında AsNoTracking kullanımı yaygınlaştırıldı. ListAsync, CountAsync gibi metotlar optimize edildi. BaseSpecification'a SetCriteria metodu eklendi.            |
| Önbellek (Cache) Mekanizması       | ❌ Tamamlanmadı       | Düşük   | İleride değerlendirilecek.                                                                                                          |
| **Performans iyileştirmeleri**     | ⚠️ Kısmen Tamamlandı              | Orta    | Sayfalama ve filtreleme iyileştirmeleri PagedResponse uygulanarak tamamlandı. ProductsWithCategorySpecification güncellendi.                                      |

#### 3.2. Mimari İyileştirmeler (Backend)

| Görev                                                              | Durum                 | Öncelik | Notlar                                                                                                                                                                                                                            |
| :----------------------------------------------------------------- | :-------------------- | :------ | :---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Repository pattern implementasyonu/iyileştirilmesi                 | ✅ Tamamlandı         | Yüksek  | Kullanıcı, Rol, İzin, Ürün, Kategori için Repository arayüzleri ve implementasyonları oluşturuldu/güncellendi. GenericRepository kullanılıyor. **Not:** Currency entity'si ve ilgili repository'ler projeden kaldırıldı.                                                                               |
| Unit of Work pattern implementasyonu/iyileştirilmesi               | ✅ Tamamlandı         | Orta    | `IUnitOfWork` ve `UnitOfWork` tüm repository'leri içeriyor. `PermissionService` ve `UserPermissionService` DbContext bağımlılığından kurtarıldı. DI kaydı kontrol edildi, doğru implementasyon (`Data.UnitOfWork`) aktif edildi, kullanılmayan `UnitOfWork.cs` silindi. |
| Specification pattern implementasyonu                              | ✅ Tamamlandı         | Yüksek  | Temel yapı kuruldu ve Repository katmanında aktif olarak kullanılıyor (Kullanıcı, Rol, İzin, Ürün için spesifikasyonlar mevcut). **Category** entity'si için de spesifikasyon oluşturuldu.                       |
| CQRS (Command Query Responsibility Segregation) deseni uygulanması | ⚠️ Kısmen Tamamlandı  | Orta    | Kullanıcı, Rol, Ürün ve Kategori yönetimi tamamlandı. **İzin Yönetimi** için temel CRUD operasyonları (GetAll, GetById, Create, Update, Delete) implemente edildi. **Rol ve kullanıcılar için izin atama/güncelleme işlemleri de CQRS ile tamamlandı.** |
| Mediator pattern implementasyonu                                   | ⚠️ Kısmen Tamamlandı  | Orta    | MediatR kullanılarak Kullanıcı, Rol, Ürün ve Kategori yönetimi implemente edildi. **İzin Yönetimi** için temel CRUD handler'ları oluşturuldu. Yeni özellikler için handler'lar oluşturulacak.                     |
| Domain-Driven Design prensiplerinin uygulanması                    | ⚠️ Kısmen Tamamlandı | Orta    | **Product**, **User**, **Role**, **Permission**, **Category** entity'leri için temel prensipler (Factory, Behavior Methods, Encapsulation) uygulandı. Value Object kullanımı (`Product` için `.OwnsOne()` ile yeniden yapılandırıldı, diğerleri pragmatik) sınırlı/pragmatik tutuldu. **Not:** Para birimi (Currency) entity'si ve ilgili DDD yapılandırması projeden kaldırıldı. |
| **Update/Delete Command Handler'larında Specification Kullanımı** | ✅ Tamamlandı         | Orta    | `UpdateProductCommandHandler` ve `DeleteProductCommandHandler` sınıfları, entity'leri almak için `ProductByIdSpecification` kullandı. `CategoryByIdSpecification` oluşturuldu. `DeleteAsync` (soft delete) tutarlılığı sağlandı. Detaylar için: [Specification in Update/Delete Handlers Belgesi](mdc:./specification_in_update_delete_handlers.md) |
| Mikroservis mimarisine geçiş hazırlığı                             | ❌ Tamamlanmadı       | Düşük   |                                                                                                                                                                                                                                   |
| **[Kalan]** API versiyonlama                                       | ⬜ Kalan              | Düşük   |                                                                                                                                                                                                                                   |
| **Güvenlik iyileştirmeleri**                                       | ✅ Tamamlandı         | Orta    | API Rate Limiting, Security Headers Middleware, hassas veriler için Secret Manager kullanımı ve bağımlılık güvenliği taraması tamamlandı.                                                                                        |
| **[Kalan]** Kod tabanının yeniden yapılandırılması                 | ⬜ Kalan              | Düşük   |                                                                                                                                                                                                                                   |

#### 3.3. Asenkron İşlem Optimizasyonları (Backend)

| Görev                                                          | Durum                 | Öncelik |
| :------------------------------------------------------------- | :-------------------- | :------ |
| Uzun süren işlemlerin arka planda çalıştırılması                 | ❌ Tamamlanmadı       | Orta    |
| İş parçacığı havuzu (thread pool) optimizasyonu                  | ❌ Tamamlanmadı       | Düşük   |
| Task-based asenkron programlama iyileştirmeleri                  | ⚠️ Kısmen Tamamlandı | Orta    |
| Rapor oluşturma işlemleri için asenkron yapı kurulması           | ❌ Tamamlanmadı       | Düşük   |
| ConfigureAwait(false) kullanımı ile performans iyileştirilmesi | ❌ Tamamlanmadı       | Düşük   |
| API yanıt boyutlarının küçültülmesi              | ⚠️ Kısmen Tamamlandı | Orta    | Kullanıcı, Ürün ve Rol listeleri için hafif DTO'lar oluşturuldu.    |
| API rate limiting uygulanması                  | ✅ Tamamlandı        | Düşük   | Global bir kural ile tüm endpoint'lere temel koruma sağlandı.       |
| API versiyonlama stratejisinin iyileştirilmesi | ❌ Tamamlanmadı      | Düşük   |                                                                     |

#### 3.4. API Endpoint Optimizasyonları (Backend)

| Görev                                          | Durum                | Öncelik | Notlar                                                              |
| :--------------------------------------------- | :------------------- | :------ | :------------------------------------------------------------------ |
| API yanıt boyutlarının küçültülmesi              | ⚠️ Kısmen Tamamlandı | Orta    | Kullanıcı, Ürün ve Rol listeleri için hafif DTO'lar oluşturuldu.    |
| API rate limiting uygulanması                  | ❌ Tamamlanmadı      | Düşük   |                                                                     |
| API versiyonlama stratejisinin iyileştirilmesi | ❌ Tamamlanmadı      | Düşük   |                                                                     |

#### 3.5. Frontend Performans Optimizasyonları

| Görev                                                  | Durum                | Öncelik | Notlar                                                                    |
| :----------------------------------------------------- | :------------------- | :------ | :------------------------------------------------------------------------ |
| State management optimizasyonu                         | ⚠️ Kısmen Tamamlandı | Orta    | Kullanıcı, Ürün ve Rol yönetimi için Signal tabanlı state servisleri oluşturuldu. |
| Lazy loading implementasyonu                           | ⚠️ Kısmen Tamamlandı | Orta    | Kullanıcı, Ürün ve Rol yönetimi için tamamlandı.                          |
| Route guard'ların iyileştirilmesi                        | ⬜ Kalan             | Orta    |                                                                           |
| Change detection stratejisinin optimizasyonu           | ⚠️ Kısmen Tamamlandı | Orta    | Kullanıcı, Ürün ve Rol yönetimi bileşenleri `OnPush` stratejisine geçirildi.  |
| Bundle boyutlarının küçültülmesi                       | ⬜ Kalan             | Orta    |                                                                           |
| Web worker kullanımı ile arayüzün kilitlenmesini önleme  | ⬜ Kalan             | Düşük   |                                                                           |

## 3. Detaylı Teknik Plan

Bu bölüm, yukarıda belirtilen her bir iyileştirme adımının teknik detaylarını içerir.

### 3.1. Frontend Temizliği ve İyileştirmeler

-   **Magic String/Number**: `src/app/core/constants/` altında ilgili sabitler tanımlandı ve bileşenlerde kullanıldı.
-   **API Endpoint Uyumluluğu**: Backend controller (`/api/role`) ile frontend servisi (`/api/roles`) arasındaki uyumsuzluk giderildi.
-   **Hata Yönetimi**: `HttpInterceptor` yerine `BaseHttpService` içinde merkezi bir hata yönetimi yapıldı. HTTP durum kodlarına göre kullanıcıya bildirimler gösterildi.
-   **API Yanıt İşleme**: Yanıtlar `BaseHttpService` içinde standart bir formata (`{ success: boolean, data: T | null, error: any }`) dönüştürüldü.
-   **BaseHttpService**: Tüm servisler `BaseHttpService`'i miras alacak şekilde yeniden yapılandırıldı.

### 3.2. Backend Mimarisi ve Optimizasyonlar

#### 3.2.1. Global Exception Handling

-   **Middleware**: `ExceptionMiddleware.cs` oluşturuldu. `try-catch` blokları ile tüm pipeline'daki hatalar yakalanır.
-   **Hata Yanıtı**: Hatanın türüne göre (NotFound, Validation, etc.) standart bir JSON yanıtı (`{ StatusCode, Message, Details }`) oluşturulur. `Details` sadece geliştirme ortamında doldurulur.
-   **Loglama**: Yakalanan her hata `ILoggerManager` aracılığıyla loglanır.

#### 3.2.2. Merkezi Loglama

-   **NLog Yapılandırması**: `nlog.config` dosyası ile logların nereye (dosya, konsol vb.), hangi formatta ve hangi kurallara göre (rotasyon, arşivleme) yazılacağı belirlendi.
-   **LoggerManager**: `ILoggerManager` arayüzü ve `LoggerManager` sınıfı, NLog'u soyutlayarak uygulama içinde tek bir noktadan loglama yapılmasını sağlar.

#### 3.2.3. Repository, Unit of Work ve Specification Pattern

-   **IRepository/GenericRepository**: `AddAsync`, `GetByIdAsync`, `GetAllAsync`, `UpdateAsync`, `DeleteAsync` gibi temel CRUD operasyonlarını tanımlar. `FirstOrDefaultAsync` ve `CountAsync` gibi metotlar `ISpecification` alır.
-   **IUnitOfWork**: Tüm repository'leri tek bir scope altında toplar. `SaveChangesAsync` metodu ile tüm değişiklikleri tek bir transaction'da veritabanına yazar.
-   **ISpecification**: Sorgu kriterlerini (`Criteria`), dahil edilecek ilişkileri (`Includes`), sıralama (`OrderBy`) ve sayfalama (`Paging`) bilgilerini encapsule eder.
-   **Uygulama**: `UserRepository`, `RoleRepository` gibi spesifik repository'ler `GenericRepository`'den türer. Servisler ve Handler'lar, `IUnitOfWork` üzerinden repository'lere erişir ve sorgularını `Specification` nesneleri ile yapar. Bu sayede sorgu mantığı IQueryable sızıntısı olmadan Repository katmanında kalır.

#### 3.2.4. CQRS ve MediatR

-   **Commands/Queries**: Her bir yazma (Create, Update, Delete) ve okuma (GetById, GetAll) operasyonu için ayrı sınıflar (`CreateUserCommand`, `GetAllUsersQuery` vb.) oluşturuldu. Bu sınıflar `IRequest<TResponse>` arayüzünü implemente eder.
-   **Handlers**: Her komut/sorgu için ayrı bir handler sınıfı (`CreateUserCommandHandler`, `GetAllUsersQueryHandler` vb.) oluşturuldu. Bu sınıflar `IRequestHandler<TRequest, TResponse>` arayüzünü implemente eder ve asıl iş mantığını içerir.
-   **MediatR Pipeline**: `IMediator` arayüzü, `Controller` katmanından inject edilir. `_mediator.Send(command)` çağrısı ile ilgili komut/sorgu, doğru handler'a yönlendirilir.
-   **Avantajlar**: Controller'lar çok daha sade hale geldi. İş mantığı, yeniden kullanılabilir handler'lara taşındı.

#### 3.2.5. Domain-Driven Design (DDD)

-   **Entity Encapsulation**: Entity'lerin property'leri `private set` yapılarak dışarıdan değiştirilmesi engellendi.
-   **Factory Metotları**: `public static Result<Entity> Create(...)` metotları ile entity'lerin sadece geçerli bir state ile oluşturulması sağlandı. Constructor `private` yapıldı.
-   **Davranış Metotları**: `UpdateName()`, `AddStock()` gibi metotlar ile state değişiklikleri entity'nin kendi içinde, iş kuralları kontrol edilerek yapılır.
-   **Value Objects**: `ProductName` gibi basit ve değişmez değerler için ayrı sınıflar oluşturuldu. `Product` entity'sinde EF Core'un `.OwnsOne()` metodu ile veritabanı eşlemesi yapıldı. Bu, `Product` tablosuna `Name_Value` gibi bir kolon ekler. Diğer entity'ler için (User, Role vb.) daha pragmatik bir yaklaşımla primitive tipler korundu.
-   **Result Pattern**: İşlem sonuçları (başarılı/başarısız) ve hatalar, `Result<T>` sınıfı ile yönetildi. Bu, handler'larda `try-catch` bloklarına olan bağımlılığı azaltır ve iş kurallarından dönen hataların daha kontrollü yönetilmesini sağlar.

## 4. Kaynaklar ve Referanslar

-   [Clean Architecture - Jason Taylor](https://jasontaylor.dev/clean-architecture-a-net-template/)
-   [NLog Dokümantasyonu](https://nlog-project.org/documentation/)
-   [MediatR Dokümantasyonu](https://github.com/jbogard/MediatR)
-   [Specification Pattern - Ardalis](https://ardalis.com/specification-pattern-c-razor-pages/)
-   [DDD ve EF Core - Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/net-core-microservice-domain-model) 