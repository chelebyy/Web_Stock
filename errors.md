# Hatalar ve Çözümler

Bu dosya, proje geliştirme sürecinde karşılaşılan hataları ve çözümlerini içermektedir.

## İçindekiler
- [Clean Architecture Geçişi Hataları](#clean-architecture-geçişi-hataları)
- [Infrastructure Katmanı Hataları](#infrastructure-katmanı-hataları)
- [API Katmanı Hataları](#api-katmanı-hataları)
- [Null Değer Uyarıları](#null-değer-uyarıları)
- [Backend ve Frontend Başlatma Sorunları](#backend-ve-frontend-başlatma-sorunları)
- [Genel Öneriler](#genel-öneriler)
- [Veritabanı Yapılandırma Hataları](#veritabanı-yapılandırma-hataları)
- [API İstek Hataları](#api-i̇stek-hataları)
- [Şifre Hashleme ve Doğrulama Sorunları](#şifre-hashleme-ve-doğrulama-sorunları)
- [Bağımlılık Enjeksiyon Hataları](#bağımlılık-enjeksiyon-hataları)
- [Login Hataları](#login-hataları)
- [PowerShell Komut Çalıştırma Sorunları](#powershell-komut-çalıştırma-sorunları)
- [Profil Resmi Ekleme Sorunları](#profil-resmi-ekleme-sorunları)
- [Şifre Değiştirme Formu Sorunları](#şifre-değiştirme-formu-sorunları)
- [Frontend Rol Yönetimi Hataları](#frontend-rol-yönetimi-hataları)
- [Login Sicil Sorunu](#login-sicil-sorunu)
- [Login 401 Unauthorized Hatası](#login-401-unauthorized-hatası)
- [Kullanıcı İzinleri Yönetimi Entegrasyonu](#kullanıcı-izinleri-yönetimi-entegrasyonu)
- [PrimeNG 19 Uyumluluk Sorunları](#primeNG-19-uyumluluk-sorunları)
- [User Model Uyumsuzluğu Hatası](#user-model-uyumsuzluğu-hatası)
- [Backend Model Uyumsuzluğu ve Migration Hataları](#backend-model-uyumsuzluğu-ve-migration-hataları)
- [Veritabanı Temizliği Sonrası Login Hatası](#veritabanı-temizliği-sonrası-login-hatası)
- [Kullanıcı Dashboard Yeniden Tasarımı Hataları](#kullanıcı-dashboard-yeniden-tasarımı-hataları)
- [Profil Resmi Placeholder Hatası (06.03.2025)](#profil-resmi-placeholder-hatası-06032025)
- [Profil Resmi ve API Endpoint Hataları (06.03.2025)](#profil-resmi-ve-api-endpoint-hataları-06032025)
- [Profil Resmi Endpoint Hatası - Geçici Çözüm (06.03.2025)](#profil-resmi-endpoint-hatası-geçici-çözüm-06032025)
- [Revir ve BilgiIslem İzinlerinin Görünmeme Sorunu](#revir-ve-bilgiislem-izinlerinin-görünmeme-sorunu)
- [Angular 19 Geçiş Süreci Hataları ve Çözümleri](#angular-19-geçiş-süreci-hataları-ve-çözümleri)
- [Angular 19 ve PrimeNG 19 Güncelleme Hataları](#angular-19-ve-primeNG-19-güncelleme-hataları)
- [Login Sayfası Tasarım Sorunları ve Çözümleri](#login-sayfası-tasarım-sorunları-ve-çözümleri)

## Kullanıcı Oluşturma Hataları

### Hata: Sicil Numarası Çakışması

**Hata Mesajı:**
```
500 (Internal Server Error)
```

**Nedeni:**
Kullanıcı oluşturulurken, sistemde zaten var olan bir sicil numarası kullanılmaya çalışılıyor. SeedData sınıfında "U001" sicil numarasıyla oluşturulan bir kullanıcı bulunduğu için, aynı sicil numarasıyla yeni bir kullanıcı oluşturulamıyor.

**Çözüm:**
1. Farklı bir sicil numarası kullanılarak yeni kullanıcı oluşturulabilir.
2. Hata mesajlarının daha açıklayıcı olması için API ve frontend tarafında aşağıdaki değişiklikler yapıldı:

AuthController.cs dosyasında:
```csharp
if (errorMessage.Contains("sicil numarası zaten kullanılmaktadır"))
{
    return BadRequest(new { error = $"Sicil numarası çakışması: {errorMessage}", code = "DuplicateSicil" });
}
```

UserService.ts dosyasında:
```typescript
if (error.error.code === 'DuplicateSicil') {
  errorMessage = `Sicil numarası '${user.sicil}' zaten kullanımda. Lütfen farklı bir sicil numarası girin.`;
}
```

**Öğrenilen Ders:**
1. Kullanıcı dostu hata mesajları oluşturmak için hem backend hem de frontend tarafında uygun hata işleme mekanizmaları kurulmalı.
2. Benzersiz olması gereken alanlar (sicil numarası gibi) için kullanıcı arayüzünde ön kontrol yapılmalı.
3. HTTP 500 hataları yerine uygun HTTP durum kodları ile anlamlı hata mesajları döndürülmeli.

### Hata: Kullanıcı Oluşturma API Hatası 

**Hata Mesajı:**
```
POST http://localhost:5037/api/auth/create-user 500 (Internal Server Error)
```

**Nedeni:**
1. Kullanıcı oluşturma API'si gelen isteği yeterince kontrol etmiyor
2. Sicil numarası çakışması hatası dışındaki hata durumları için yeterli loglama yok
3. API'nin çıktısı yeterince detaylı değil, hata mesajları yeterince açıklayıcı değil

**Çözüm:**
1. CreateUserCommandHandler sınıfında daha kapsamlı hata işleme ve loglama eklendi:
   ```csharp
   try
   {
       _logger.LogInformation("Kullanıcı oluşturma işlemi başlatıldı: {Username}, Sicil: {Sicil}", request.Username, request.Sicil);
       // ... İşlem kodları ...
   }
   catch (Exception ex)
   {
       _logger.LogError(ex, "Kullanıcı oluşturma işlemi sırasında hata oluştu: {ErrorMessage}", ex.Message);
       throw;
   }
   ```

2. API endpoint'i olan AuthController.CreateUser metodunda daha iyi bir hata işleme mekanizması kuruldu:
   ```csharp
   try
   {
       // ... İşlem kodları ...
   }
   catch (InvalidOperationException ex)
   {
       // ... Doğrulama hataları ...
   }
   catch (Exception ex)
   {
       _logger.LogError(ex, "Kullanıcı oluşturma sırasında bir hata meydana geldi: {Message}", ex.Message);
       return StatusCode(500, new { 
           error = "Kullanıcı oluşturulurken bir hata oluştu", 
           details = ex.Message,
           innerException = ex.InnerException?.Message,
           stackTrace = ex.StackTrace
       });
   }
   ```

3. Frontend tarafında daha iyi hata işleme:
   ```typescript
   createUser(user: User): Observable<any> {
     // ... İşlem kodları ...
     return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest, options)
       .pipe(
         catchError(error => {
           // ... Hata işleme kodları ...
         })
       );
   }
   ```

**Öğrenilen Ders:**
1. API'lerde her zaman yeterli doğrulama, loglama ve hata işleme olmalı
2. Frontend ve backend arasında tutarlı bir hata iletişim stratejisi olmalı
3. HTTP durum kodları doğru kullanılmalı (BadRequest, NotFound vs)
4. Kullanıcılara anlamlı hata mesajları gösterilmeli
5. Geliştirme ortamında detaylı hata mesajları, production ortamında genel hata mesajları verilmeli

### Hata: Kullanıcı Oluşturma Rol Seçimi Hatası

**Hata Mesajı**
```
Geçerli bir rol seçilmelidir
```

**Hata Detayı**
Yeni kullanıcı oluşturma işlemi sırasında, backend'e gönderilen veride roleId değeri eksik olduğu için "Geçerli bir rol seçilmelidir" hatası alınıyordu.

**Çözüm**
1. `frontend/src/app/models/auth.model.ts` dosyasındaki CreateUserRequest arayüzüne roleId alanı eklendi.
2. `frontend/src/app/services/user.service.ts` dosyasındaki createUser metodunda, CreateUserRequest nesnesine roleId değeri eklendi.

Detaylı bilgi için: [Kullanıcı Oluşturma Rol Seçimi Hatası Çözümü](knowledge-base/user-creation-role-fix.md)

## Sınıf Belirsizliği Hataları

**Hata:** Stock.Infrastructure projesinde derleme hatası: 'RolePermission', 'Stock.Domain.Entities.RolePermission' ile 'Stock.Domain.Entities.Permissions.RolePermission' arasında belirsiz bir başvuru.

**Hata Mesajı:**
```
error CS0104: 'RolePermission', 'Stock.Domain.Entities.RolePermission' ile 'Stock.Domain.Entities.Permissions.RolePermission' arasında belirsiz bir başvuru
```

**Nedeni:**
Projede aynı isimli sınıf (RolePermission) iki farklı namespace'te (`Stock.Domain.Entities` ve `Stock.Domain.Entities.Permissions`) tanımlanmış. Derleyici, kodda sadece sınıf adı kullanıldığında hangisini kullanacağını bilemiyor.

**Çözüm:**
Sınıfı kullanırken tam nitelikli adını (fully qualified name) kullanmak. İlgili dosyada (`src/Stock.Infrastructure/Data/Repositories/PermissionRepository.cs`) RolePermission sınıfını şu şekilde kullandık:

```csharp
// Önceki kod:
var rolePermission = new RolePermission { ... };

// Düzeltilmiş kod:
var rolePermission = new Stock.Domain.Entities.RolePermission { ... };
```

**Alternatif Çözümler:**
1. Using direktifi ile birini özellikle belirtmek:
```csharp
using RolePermission = Stock.Domain.Entities.RolePermission;
```

2. Namespace yeniden düzenlemesi yaparak sınıf çakışmalarını önlemek.

**Öğrenilen Ders:**
Aynı isimli sınıflar farklı namespace'lerde olduğunda tam nitelikli ad kullanmak gerekir. Uzun vadede, sınıf isim çakışmalarını önlemek için proje yapısını düzenlemek daha iyi bir çözümdür.

## Kullanıcı Admin Yetkisi Sorunları

**Hata:** Yeni oluşturulan kullanıcı (U003 sicil numaralı Test3) admin paneline yönlendirildi. Bu, kullanıcının isAdmin değerinin yanlışlıkla true olarak ayarlandığını gösteriyordu.

**Nedeni:**
1. `CreateUserCommandHandler` sınıfında frontend'den gelen isAdmin değeri doğrudan kullanıcıya atanıyordu
2. `UpdateUserCommandHandler` ve `AuthService.RegisterAsync` metodunda da benzer sorunlar vardı
3. Frontend'den gönderilen isAdmin değeri güvenilir değildi ve manipüle edilebilirdi

**Çözüm:**
1. Kullanıcı oluşturma ve güncelleme işlemlerinde isAdmin değerini RoleId'ye göre belirledik:
```csharp
// IsAdmin değerini sadece RoleId=1 (Admin rolü) olduğunda true yap
bool isAdmin = request.RoleId.HasValue && request.RoleId.Value == 1;
```

2. `CreateUserCommandHandler`, `UpdateUserCommandHandler` ve `AuthService.RegisterAsync` metodlarını güncelledik
3. IsAdmin değeri artık frontend'den gönderilen değere bakılmaksızın, RoleId'ye göre otomatik belirleniyor
4. Daha detaylı loglama ekledik

**Öğrenilen Ders:**
Frontend'den gelen güvenlik ile ilgili parametreleri asla doğrudan kullanmamalıyız. Güvenlik ile ilgili tüm değerler sunucu tarafında kontrol edilmeli ve atanmalıdır.

## Clean Architecture Geçişi Hataları

### Solution Yapısı Sorunları

**Hata:** Proje referansları ve bağımlılıklar düzgün ayarlanmamış

**Nedeni:**
1. Katmanlar arası referanslar eksik
2. NuGet paketleri yanlış projelerde
3. Circular dependency riski

**Çözüm:**
1. Proje referanslarını düzenle:
```xml
<!-- Stock.Application -->
<ProjectReference Include="..\Stock.Domain\Stock.Domain.csproj" />

<!-- Stock.Infrastructure -->
<ProjectReference Include="..\Stock.Domain\Stock.Domain.csproj" />
<ProjectReference Include="..\Stock.Application\Stock.Application.csproj" />

<!-- Stock.API -->
<ProjectReference Include="..\Stock.Infrastructure\Stock.Infrastructure.csproj" />
```

2. NuGet paketlerini doğru projelere taşı:
- Domain: Sadece temel .NET paketleri
- Application: AutoMapper, FluentValidation
- Infrastructure: Entity Framework Core, Npgsql
- API: ASP.NET Core paketleri, Swagger

**Önemli Notlar:**
- Domain katmanı hiçbir katmana bağımlı olmamalı
- Application katmanı sadece Domain'e bağımlı olmalı
- Infrastructure hem Domain hem Application'a bağımlı olabilir
- API katmanı Infrastructure'a bağımlı olabilir

### Eski Yapı Temizliği

**Sorun:** Clean Architecture geçişi sonrası eski yapı hala projede bulunuyordu

**Nedeni:**
1. Geçiş sırasında eski klasörler silinmemiş
2. Kök dizinde ve src klasöründe aynı isimli projeler vardı
3. Karışıklığa neden olabilecek eski dosyalar mevcuttu

**Çözüm:**
1. Eski yapıyı yedekle (backup_old_structure klasörüne)
2. Aşağıdaki eski klasörleri ve dosyalarını sil:
   - backend/StockAPI
   - Stock.API (kök dizindeki)
   - Stock.Infrastructure (kök dizindeki)
   - src/Stock.sln (boş solution dosyası)
   - CreateAdminUser.cs (kök dizindeki boş dosya)

**Önemli Notlar:**
- Temizlik öncesi mutlaka yedek alınmalı
- Silme işlemi öncesi dosyaların içeriği kontrol edilmeli
- Yeni yapının çalıştığından emin olunmalı

### CQRS Pattern Implementasyon Sorunları

**Hata:** Generic tip parametreleri ve constraint'ler eksik

**Nedeni:**
1. ICommand ve IQuery interface'leri eksik tanımlanmış
2. Handler'lar için constraint'ler eksik
3. Return tipleri belirsiz

**Çözüm:**
Interface'leri düzelt:
```csharp
public interface ICommand<TResult> { }
public interface ICommand { }
public interface IQuery<TResult> { }

public interface ICommandHandler<in TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
```

**Önemli Notlar:**
- Generic tip parametreleri doğru kullanılmalı
- Constraint'ler eksiksiz tanımlanmalı
- Async/await pattern'i kullanılmalı
- CancellationToken desteği eklenmeli

### Entity İlişkileri Sorunları

**Hata:** Entity ilişkileri düzgün tanımlanmamış

**Nedeni:**
1. Virtual keyword'ü eksik
2. Collection tipleri yanlış
3. Foreign key tanımlamaları eksik

**Çözüm:**
Entity'leri düzelt:
```csharp
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }
}

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<User> Users { get; set; }

    public Role()
    {
        Users = new HashSet<User>();
    }
}
```

**Önemli Notlar:**
- Virtual keyword'ü lazy loading için gerekli
- Collection'lar için HashSet<T> tercih edilmeli
- Foreign key'ler nullable olabilir
- Constructor'da collection'lar initialize edilmeli

## Infrastructure Katmanı Hataları

### IAuthService Uygulaması Hatası

**Hata:**
```
'AuthService', 'IAuthService.GenerateJwtToken(UserDto)' arabirim üyesini uygulamaz
```

**Çözüm:**
AuthService sınıfına GenerateJwtToken metodu ekle:
```csharp
public string GenerateJwtToken(UserDto user)
{
    return _tokenGenerator.GenerateToken(new User
    {
        Id = user.Id,
        Username = user.Username,
        IsAdmin = user.IsAdmin,
        RoleId = user.RoleId,
        Role = user.RoleName != null ? new Role { Id = user.RoleId ?? 0, Name = user.RoleName } : null
    });
}
```

### Repository Uygulaması Hataları

**Hata:**
```
'GenericRepository<T>, 'IRepository<T>.AddAsync(T)' arabirim üyesini uygulamıyor. 'Task' eşleşen dönüş türüne sahip olmadığından 'GenericRepository<T>.AddAsync(T)' 'IRepository<T>.AddAsync(T)' öğesini uygulayamaz.
```

**Çözüm:**
GenericRepository sınıfındaki AddAsync metodunun dönüş tipini Task olarak değiştir ve SaveChangesAsync metodunu ekle:
```csharp
public async Task AddAsync(T entity)
{
    await _dbSet.AddAsync(entity);
}

public async Task SaveChangesAsync()
{
    await _context.SaveChangesAsync();
}
```

### UnitOfWork Uygulaması Hataları

**Hata:**
```
'UnitOfWork', 'IUnitOfWork.CommitTransactionAsync()' arabirim üyesini uygulamaz
'UnitOfWork', 'IUnitOfWork.RollbackTransactionAsync()' arabirim üyesini uygulamaz
```

**Çözüm:**
UnitOfWork sınıfına CommitTransactionAsync ve RollbackTransactionAsync metotlarını ekle:
```csharp
public async Task CommitTransactionAsync()
{
    await _context.Database.CommitTransactionAsync();
}

public async Task RollbackTransactionAsync()
{
    await _context.Database.RollbackTransactionAsync();
}
```

### Repository Dönüş Tipi Uyumsuzluğu

**Hata:**
```
'UnitOfWork, 'IUnitOfWork.Users' arabirim üyesini uygulamıyor. 'IUserRepository' eşleşen dönüş türüne sahip olmadığından 'UnitOfWork.Users' 'IUnitOfWork.Users' öğesini uygulayamaz.
```

**Çözüm:**
UnitOfWork sınıfındaki Users ve Roles özelliklerinin tiplerini düzelt:
```csharp
private readonly IUserRepository _users;
private readonly IRoleRepository _roles;

public IUserRepository Users => _users;
public IRoleRepository Roles => _roles;
```

## API Katmanı Hataları

### Paket Sürüm Uyumsuzluğu

**Hata:**
```
System.IdentityModel.Tokens.Jwt paketi için 8.6.0 sürümünden 8.5.0 sürümüne düşürme algılandı.
```

**Çözüm:**
API projesinde System.IdentityModel.Tokens.Jwt paketinin sürümünü yükselt:
```
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.6.0
```

### FluentValidation Entegrasyonu

**Hata:**
```
'IServiceCollection' bir 'AddValidatorsFromAssembly' tanımı içermiyor
```

**Çözüm:**
FluentValidation.DependencyInjection paketi yerine manuel kayıt kullan:
```csharp
var assembly = Assembly.GetExecutingAssembly();
var validatorType = typeof(IValidator<>);
var validatorTypes = assembly.GetTypes()
    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType))
    .ToList();

foreach (var validator in validatorTypes)
{
    var validatorInterface = validator.GetInterfaces()
        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType);
    services.AddTransient(validatorInterface, validator);
}
```

## Null Değer Uyarıları

**Uyarı:**
```
Null atanamaz özellik 'Username', oluşturucudan çıkış yaparken null olmayan bir değer içermelidir.
```

**Çözüm:**
Bu uyarılar, C# 9.0'ın null referans tipleri özelliğinden kaynaklanmaktadır. Çözüm için aşağıdaki yaklaşımlar kullanılabilir:

1. Özellikleri nullable olarak işaretlemek: 
   ```csharp
   public string? Username { get; set; }
   ```

2. Özelliklere varsayılan değer atamak: 
   ```csharp
   public string Username { get; set; } = string.Empty;
   ```

3. Yapıcı metotta değer atamak:
   ```csharp
   public User()
   {
       Username = string.Empty;
       PasswordHash = string.Empty;
   }
   ```

4. [Required] özniteliğini kullanmak:
   ```csharp
   [Required]
   public string Username { get; set; }
   ```

## Backend ve Frontend Başlatma Sorunları

### Backend Başlatma Sorunu

**Hata:**
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> dotnet run
Çalıştırılacak bir proje bulunamadı. C:\Users\muham\OneDrive\Masaüstü\Stock içinde bir projenin bulunduğundan emin olun veya --project kullanarak yolu projeye geçirin.
```

**Nedeni:**
1. Proje ana dizininde çalıştırıldığında dotnet run komutu çalıştırılacak projeyi bulamıyor
2. PowerShell'de && operatörü desteklenmiyor
3. Doğru dizinde olmadan komut çalıştırılıyor

**Çözüm:**
Backend için:
```powershell
cd Stock.API
dotnet run
```

### Frontend Başlatma Sorunları (03.03.2025)

### Hata: Angular CLI Tanınmama Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
ng : The term 'ng' is not recognized as the name of a cmdlet, function, script file, or operable program.
```

**Nedeni:**
1. Angular CLI global olarak yüklü değil
2. PATH değişkeninde Angular CLI yolu tanımlı değil
3. PowerShell'de komut çalıştırma yetkileri kısıtlı olabilir

**Çözüm:**
1. Angular CLI'ı global olarak yüklemek:
```powershell
npm install -g @angular/cli
```

2. Yerel Angular CLI'ı kullanmak:
```powershell
cd frontend
npx ng serve --port=4202
```

3. npm run start komutunu kullanmak:
```powershell
cd frontend
npm run start -- --port=4202
```

**Önemli Notlar:**
- PowerShell'de && operatörü yerine ; kullanılmalıdır
- Port çakışması durumunda farklı bir port belirtilmelidir
- Çalışan node işlemleri durdurulmalıdır
- Komutlar doğru dizinde çalıştırılmalıdır (frontend klasöründe)

### Hata: Port Çakışması Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
Port 4200 is already in use.
Would you like to use a different port? No
An unhandled exception occurred: Port 4200 is already in use. Use '--port' to specify a different port.
```

**Nedeni:**
1. 4200 portu başka bir uygulama tarafından kullanılıyor
2. Önceki Angular uygulaması düzgün kapatılmamış
3. Port serbest bırakılmamış

**Çözüm:**
1. Çalışan tüm node işlemlerini durdurmak:
```powershell
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
```

2. 4200 portunu kullanan işlemleri bulmak ve durdurmak:
```powershell
netstat -ano | findstr :4200
Get-Process -Id <PID> -ErrorAction SilentlyContinue | Stop-Process -Force
```

3. Farklı bir port belirterek uygulamayı başlatmak:
```powershell
cd frontend
npm run start -- --port=4202
```

**Önemli Notlar:**
- Port parametresi doğru şekilde geçirilmelidir: `--port=4202`
- Komut doğru dizinde çalıştırılmalıdır (frontend klasöründe)
- Çalışan işlemler düzgün şekilde durdurulmalıdır
- Uygulama başlatıldıktan sonra http://localhost:4202 adresinden erişilebilir

### Hata: npm Komut Çalıştırma Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
npm error code ENOENT
npm error syscall open
npm error path C:\Users\IT\Documents\Cz_Web\Web_Stock\package.json
npm error errno -4058
npm error enoent Could not read package.json: Error: ENOENT
```

**Nedeni:**
1. Komut yanlış dizinde çalıştırılıyor
2. package.json dosyası bulunamıyor
3. npm komutları frontend klasöründe çalıştırılmalı

**Çözüm:**
1. Doğru dizine geçmek:
```powershell
cd frontend
```

2. npm komutlarını frontend klasöründe çalıştırmak:
```powershell
npm run start -- --port=4202
```

**Önemli Notlar:**
- npm komutları package.json dosyasının bulunduğu dizinde çalıştırılmalıdır
- Frontend uygulaması için komutlar frontend klasöründe çalıştırılmalıdır
- Komut çalıştırılmadan önce dizin kontrol edilmelidir

## Genel Öneriler

1. **Arayüz ve Uygulama Geliştirme**
   - Önce arayüzleri tam olarak tanımla, sonra uygulamaları geliştir
   - Arayüz ve uygulama arasındaki uyumu sürekli kontrol et

2. **Null Referans Tipleri**
   - Projenin başında bir strateji belirle ve tutarlı şekilde uygula
   - Nullable referans tiplerini doğru kullan (?, string.Empty, vb.)

3. **Paket Yönetimi**
   - Paket sürümlerini projenin tamamında tutarlı tut
   - Paket bağımlılıklarını düzenli olarak kontrol et
   - Paket çakışmalarını hızlıca çöz

4. **Hata Yönetimi**
   - Hata mesajlarını dikkatlice oku ve adım adım çöz
   - Hataları kategorilere ayır ve sistematik olarak ele al
   - Çözümleri belgelendir ve benzer hataları önle

## Veritabanı Yapılandırma Hataları

### Hata: Veritabanı Bağlantı Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Unable to create a 'DbContext' of type 'ApplicationDbContext'. The exception 'The configuration file 'appsettings.json' was not found and is not optional. The expected physical path was 'C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\appsettings.json'.' was thrown while attempting to create an instance.
```

**Çözüm:**
Migration komutları, appsettings.json dosyasının bulunduğu dizinde çalıştırılmalıdır. Infrastructure dizininde değil, API dizininde çalıştırılmalıdır.

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Hata: PowerShell Komut Çalıştırma Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
The token '&&' is not a valid statement separator in this version.
```

**Çözüm:**
PowerShell'de `&&` operatörü kullanılamaz. Komutlar ayrı ayrı çalıştırılmalıdır.

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Hata: Veritabanı Bağlantı Bilgileri Tutarsızlığı
**Tarih:** 2025-03-02
**Sorun:** 
Projede birden fazla appsettings.json dosyası bulunduğu için, veritabanı bağlantı bilgileri tutarsız olabilir.

**Çözüm:**
Tüm appsettings.json dosyalarındaki veritabanı bağlantı bilgileri güncellenmelidir:
- `Stock.API/appsettings.json`
- `src/Stock.API/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=czDb;Username=postgres;Password=19909687"
}
```

## API İstek Hataları

### Hata: API Bağlantı Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Invoke-RestMethod : Uzak sunucuya bağlanılamıyor
```

**Nedeni:**
1. API servisi çalışmıyor olabilir
2. API farklı bir portta çalışıyor olabilir
3. Güvenlik duvarı engellemiş olabilir

**Çözüm:**
1. API servisinin çalıştığından emin olun:
```powershell
cd src/Stock.API
dotnet run
```

2. API'nin doğru portta çalıştığını kontrol edin (varsayılan 5126)
3. Güvenlik duvarı ayarlarını kontrol edin

### Hata: PowerShell Komut Çalıştırma Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
The token '&&' is not a valid statement separator in this version.
```

**Çözüm:**
PowerShell'de `&&` operatörü kullanılamaz. Komutlar ayrı ayrı çalıştırılmalıdır.

```powershell
cd Stock.API
dotnet run
```

### Hata: Kullanıcı Kaydı Hatası
**Tarih:** 2025-03-02
**Sorun:** 
API'ye gönderilen kayıt isteği başarısız olabilir.

**Çözüm:**
1. İstek gövdesinin doğru formatta olduğundan emin olun:
```json
{
  "username": "Admin",
  "password": "Admin123",
  "confirmPassword": "Admin123",
  "isAdmin": true
}
```

2. API'nin çalıştığından emin olun
3. İstek URL'sinin doğru olduğundan emin olun: `http://localhost:5126/api/auth/register`

### API Bağlantı Hataları

### Hata: API Bağlantı Reddi Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
POST http://localhost:5037/api/auth/login 500 (Internal Server Error)
```

**Nedeni:**
1. Backend API'si çalışmıyor veya hatalı çalışıyor olabilir.
2. API'nin doğru portta çalıştığını kontrol edin (5037).
3. API'nin CORS ayarlarının frontend'in çalıştığı porta (4200) izin verdiğinden emin olun.

### Hata: HTTP 0 Unknown Error
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Http failure response for http://localhost:5126/api/auth/login: 0 Unknown Error
```

**Nedeni:**
Bu hata genellikle tarayıcının API'ye bağlanamadığını gösterir. Nedenleri:
1. API servisi çalışmıyor
2. API farklı bir portta çalışıyor
3. Ağ bağlantısı sorunları
4. CORS politikası engelleri

**Çözüm:**
1. API servisinin çalıştığını doğrulayın
2. API'nin doğru portta çalıştığını kontrol edin
3. Tarayıcı konsolunda ağ sekmesini kontrol edin
4. API'nin CORS yapılandırmasını kontrol edin
5. Güvenlik duvarı ayarlarını kontrol edin
6. Tarayıcıyı yeniden başlatın

**Önemli Notlar:**
- API ve frontend uygulamaları aynı anda çalışıyor olmalıdır
- API'nin 5126 portunda, frontend'in 4200 portunda çalıştığından emin olun
- Tarayıcı konsolunda ağ isteklerini ve hataları kontrol edin
- API'nin CORS yapılandırmasının frontend'in çalıştığı adresi (http://localhost:4200) izin verdiğinden emin olun

### Hata: Swagger Paket Eksikliği Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(64,18): error CS1061: 'IServiceCollection' bir 'AddSwaggerGen' tanımı içermiyor ve 'IServiceCollection' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'AddSwaggerGen' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(99,9): error CS1061: 'WebApplication' bir 'UseSwagger' tanımı içermiyor ve 'WebApplication' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'UseSwagger' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(100,9): error CS1061: 'WebApplication' bir 'UseSwaggerUI' tanımı içermiyor ve 'WebApplication' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'UseSwaggerUI' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
```

**Nedeni:**
1. Swagger NuGet paketleri eksik veya yanlış sürümde
2. Program.cs dosyasında gerekli using direktifleri eksik
3. Proje referansları düzgün ayarlanmamış

**Çözüm:**
1. Gerekli Swagger paketlerini projeye ekleyin:
```powershell
cd Stock.API
dotnet add package Swashbuckle.AspNetCore
```

2. Program.cs dosyasında gerekli using direktiflerinin eklendiğinden emin olun:
```csharp
using Microsoft.OpenApi.Models;
```

3. Program.cs dosyasında Swagger yapılandırmasının doğru şekilde yapıldığından emin olun:
```csharp
// Program.cs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
    
    // JWT için yetkilendirme yapılandırması
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ...

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API v1"));
}
```

4. Paket sürüm çakışmalarını çözün:
```powershell
dotnet restore --force
```

**Önemli Notlar:**
- Swagger paketleri API projesine eklenmelidir, diğer katmanlara değil
- Paket sürüm çakışmaları projenin derlenmesini engelleyebilir
- Program.cs dosyasında Swagger yapılandırması Development ortamında etkinleştirilmelidir
- Swagger UI'a erişmek için `/swagger/index.html` uzantısını kullanın

### Hata: Entity Framework Core Paket Çakışması
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
C:\Program Files\dotnet\sdk\9.0.100-rc.2.24474.11\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3277:
  Farklı "Microsoft.EntityFrameworkCore.Relational" sürümleri arasında çözümlenemeyen çakışmalar bulundu.
  "Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" ve "Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" arasında çakışma vardı.
```

**Nedeni:**
1. Projede farklı sürümlerde Entity Framework Core paketleri kullanılıyor
2. Farklı projelerde farklı sürümlerde paketler referans edilmiş
3. Paket sürümleri arasında tutarsızlık var

**Çözüm:**
1. Tüm projelerde aynı sürümde Entity Framework Core paketlerini kullanın:
```powershell
cd Stock.API
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
cd ../src/Stock.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
```

2. Tüm projeleri temizleyip yeniden derleyin:
```powershell
dotnet clean
dotnet build
```

3. Paket referanslarını global.json dosyası ile yönetin:
```json
{
  "msbuild-sdks": {
    "Microsoft.Build.Traversal": "3.2.0"
  }
}
```

**Önemli Notlar:**
- Tüm projelerde aynı sürümde paketler kullanılmalıdır
- Paket sürüm çakışmaları projenin derlenmesini engelleyebilir
- Paket sürümlerini güncellerken tüm bağımlı paketleri de güncellemeyi unutmayın
- Paket sürüm çakışmalarını çözmek için `dotnet restore --force` komutunu kullanabilirsiniz

## Entity Framework Core PendingModelChangesWarning Hatası

### Hata Mesajı
```
An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDbContext' changes each time it is built. This is usually caused by dynamic values used in a 'HasData' call (e.g. `new DateTime()`, `Guid.NewGuid()`). Add a new migration and examine its contents to locate the cause, and replace the dynamic call with a static, hardcoded value.
```

### Nedeni
Seed data içinde dinamik değerler kullanmak (örn. `BCrypt.Net.BCrypt.HashPassword("Admin123")`) her seferinde modelin değişmesine neden olur.

### Çözüm
1. Seed data için sabit değerler kullanın:
```csharp
// Dinamik değer (KULLANMAYIN):
PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123")

// Sabit değer (KULLANIN):
PasswordHash = "$2a$11$ij4DH2oHJIgakH3QZS94Uu1/jandJqMjWZtQB8B5.JoK4zrBd2Lhu"
```

2. Yeni bir migrasyon oluşturun ve veritabanını güncelleyin:
```bash
dotnet ef migrations add FixedPasswordHash
dotnet ef database update
```

### Hata: Dashboard Yönlendirme Hatası
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
ERROR RuntimeError: NG04002: Cannot match any routes. URL Segment: 'dashboard'
```

**Nedeni:**
1. Login component'inde `/dashboard` yoluna yönlendirme yapılıyor ancak bu rota tanımlı değil
2. Kullanıcı rolüne göre farklı dashboard'lara yönlendirme yapılması gerekiyor
3. AuthService'den gelen kullanıcı bilgisi doğru kullanılmıyor

**Çözüm:**
1. Login component'inde yönlendirme mantığını güncelle:
```typescript
// Kullanıcı rolüne göre yönlendirme
const user = this.authService.getCurrentUser();
const targetRoute = user?.isAdmin ? '/admin-dashboard' : '/user-dashboard';
console.log('Yönlendiriliyor:', targetRoute);

setTimeout(() => {
  this.router.navigate([targetRoute])
    .then(() => console.log('Yönlendirme başarılı'))
    .catch(err => console.error('Yönlendirme hatası:', err));
}, 1000);
```

2. app.routes.ts dosyasına `/admin/users` rotası eklendi ve navigateToUserManagement metodu güncellendi.

## Kullanıcı Yönetimi Entegrasyonu

### Admin Dashboard'dan Kullanıcı Yönetimine Yönlendirme
- **Sorun**: Admin dashboard'dan Kullanıcı Yönetimi sayfasına yönlendirme yapılırken rota hatası oluşuyordu.
- **Çözüm**: app.routes.ts dosyasına `/admin/users` rotası eklendi ve navigateToUserManagement metodu güncellendi.

### Kullanıcı Yönetimi Sayfasından Geri Dönüş
- **Sorun**: Kullanıcı yönetimi sayfasından admin dashboard'a geri dönüş için bir buton gerekiyordu.
- **Çözüm**: Kullanıcı yönetimi bileşenine geri dönüş butonu eklendi ve goBack metodu oluşturuldu.
  ```typescript
  goBack() {
    this.router.navigate(['/admin-dashboard']);
  }
  ```

### Kullanıcı Yönetimi Bileşeni Entegrasyonu
- **Sorun**: Mevcut UserManagementComponent'in admin dashboard ile entegrasyonu gerekiyordu.
- **Çözüm**: Admin dashboard içerisinde Kullanıcı Yönetimi bölümüne erişim sağlandı ve ilgili rota yapılandırması yapıldı.

# Login Hataları

## BCrypt Şifre Doğrulama Hatası (03.03.2025)

**Hata:** 
```
401 Unauthorized - Login işleminde şifre doğrulama hatası
```

**Nedeni:**
1. BCrypt hash'i ile şifre doğrulama işlemi başarısız
2. Veritabanındaki hash değeri ile girilen şifrenin hash'i uyuşmuyor

**Çözüm:**
1. `FixPasswordController` endpoint'i kullanılarak admin şifresi yeniden ayarlandı:
```
GET /api/FixPassword/fix-admin
```

2. Yeni hash değeri:
```
$2a$11$jLGT8mYtwgJ/U6VpQPhxFuLfnVAMm7qLsnfim33OehCyeieqVms8q
```

**Önemli Notlar:**
- BCrypt work factor: 11
- Hash uzunluğu: 60 karakter
- Hash formatı: BCrypt ($2a$ prefix)
- Şifre: Admin123

**Log Kayıtları:**
```
info: Stock.Infrastructure.Services.AuthService[0]
      Kullanıcı bulundu: admin, ID: 1, IsAdmin: True
info: Stock.Infrastructure.Services.PasswordHasher[0]
      Şifre doğrulama sonucu: True
info: Stock.Infrastructure.Services.AuthService[0]
      Başarılı giriş: admin, Token üretildi
```

### Şifre Değiştirme Endpoint Hatası (04.03.2025)

**Hata:**
```
POST http://localhost:5037/api/auth/change-password 404 (Not Found)
```

**Nedeni:**
1. Clean Architecture geçişi sırasında şifre değiştirme endpoint'i uygulanmamış
2. IAuthService arayüzünde şifre değiştirme metodu eksik
3. AuthController'da şifre değiştirme endpoint'i eksik
4. ChangePasswordDto sınıfı oluşturulmamış

**Çözüm:**
1. IAuthService arayüzüne ChangePasswordAsync metodu ekle:
```csharp
Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId);
```

2. ChangePasswordDto sınıfını oluştur:
```csharp
public class ChangePasswordDto
{
    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır")]
    public string NewPassword { get; set; } = string.Empty;
}
```

3. AuthResponseDto sınıfına Message özelliği ekle:
```csharp
public string? Message { get; set; }
```

4. AuthService sınıfına ChangePasswordAsync metodunu uygula:
```csharp
public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
{
    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var user = await _unitOfWork.Users.GetByIdAsync(userId);
    if (user == null)
    {
        _logger.LogWarning($"Kullanıcı bulunamadı - ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Kullanıcı bulunamadı"
        };
    }

    bool isCurrentPasswordValid = _passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash);
    _logger.LogInformation($"Mevcut şifre doğrulama sonucu: {isCurrentPasswordValid}");

    if (!isCurrentPasswordValid)
    {
        _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Mevcut şifre hatalı"
        };
    }

    try
    {
        user.PasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = true,
            Message = "Şifre başarıyla değiştirildi"
        };
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifre değiştirme hatası: {ex.Message}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = $"Şifre değiştirme hatası: {ex.Message}"
        };
    }
}
```

5. AuthController'a şifre değiştirme endpoint'ini ekle:
```csharp
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
{
    _logger.LogInformation("Şifre değiştirme isteği başlatılıyor...");
    
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
        _logger.LogWarning("Token'da kullanıcı ID'si bulunamadı veya geçersiz");
        return Unauthorized(new { message = "Geçersiz kullanıcı kimliği" });
    }

    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var result = await _authService.ChangePasswordAsync(changePasswordDto, userId);
    
    if (!result.Success)
    {
        _logger.LogWarning($"Şifre değiştirme başarısız - Kullanıcı ID: {userId}, Hata: {result.ErrorMessage}");
        return BadRequest(result);
    }
    
    _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
    return Ok(result);
}
```

**Önemli Notlar:**
- Şifre değiştirme işlemi için her zaman mevcut şifre doğrulaması yapılmalıdır
- Yeni şifre ve şifre tekrarı alanları eşleşmelidir
- Şifre değiştirme işlemi başarılı olduğunda kullanıcıya bildirim gösterilmelidir
- Hata durumlarında kullanıcıya anlamlı hata mesajları gösterilmelidir

## Şifre Değiştirme Formu Güncellemesi

### Şifre Değiştirme Formu Tasarım Güncellemesi

**Yapılan Değişiklikler:**
1. Şifre değiştirme formunun arka plan rengi ve stilini güncelledik
2. Input alanlarını daha modern bir görünüme kavuşturduk
3. Şifre göster/gizle işlevselliği ekledik
4. Butonların tasarımını iyileştirdik

**Olası Hatalar ve Çözümleri:**

1. **Şifre göster/gizle işlevselliği çalışmıyor**
   - **Neden:** Angular'da event binding sorunları olabilir
   - **Çözüm:** HTML'de (click) event binding'in doğru şekilde uygulandığından emin olun

2. **Şifre değiştirme API çağrısı başarısız oluyor**
   - **Neden:** Backend API endpoint'i değişmiş olabilir veya token doğrulama sorunu olabilir
   - **Çözüm:** Network isteklerini kontrol edin, API endpoint'in doğru olduğundan emin olun ve token'ın geçerli olduğunu doğrulayın

3. **Form stillerinde bozulmalar**
   - **Neden:** CSS override sorunları veya PrimeNG bileşenleriyle çakışmalar
   - **Çözüm:** CSS seçicilerinin özgüllüğünü artırın veya !important kullanın

4. **Şifre değiştirme işlemi sonrası form kapanmıyor**
   - **Neden:** togglePasswordChange() metodu çağrılmamış olabilir
   - **Çözüm:** API çağrısı başarılı olduğunda togglePasswordChange() metodunun çağrıldığından emin olun

**Genel Notlar:**
- Şifre değiştirme işlemi için her zaman mevcut şifre doğrulaması yapılmalıdır
- Yeni şifre ve şifre tekrarı alanları eşleşmelidir
- Şifre değiştirme işlemi başarılı olduğunda kullanıcıya bildirim gösterilmelidir
- Hata durumlarında kullanıcıya anlamlı hata mesajları gösterilmelidir

## Kullanıcı Silme İşlemi Hataları

### Hata 1: DeleteUserCommandHandler'da Remove Metodu Hatası

**Tarih:** 2025-03-08

**Hata Mesajı:** DeleteUserCommandHandler.cs dosyasında derleme hatası: Remove metodu için gerekli argüman eksik.

**Nedeni:** DeleteUserCommandHandler içinde `_unitOfWork.Users.Remove(user)` şeklinde bir metot çağrısı yapılıyor.

**Çözüm:**
```csharp
// Hatalı kod
_unitOfWork.Users.Remove(user);

// Düzeltilmiş kod
await _unitOfWork.Users.DeleteAsync(user);
```

**Önemli Notlar:**
- Repository arayüzlerinde metot isimleri tutarlı olmalı
- Asenkron metotlar için await kullanılmalı
- Metot isimleri ve imzaları arayüz ile uyumlu olmalı

### Hata 2: API Endpoint Tutarsızlıkları ve Karışıklık

**Tarih:** 2025-03-08

**Sorun:** Hem UserController (tekil) hem de UsersController (çoğul) olması, API endpoint'lerinde tutarsızlıklara ve frontend uygulamasında karışıklığa neden oldu.

**Nedeni:**
1. Clean Architecture'a geçiş sırasında eski yapı tamamen kaldırılmamış
2. Frontend hala eski endpoint'i referans alıyordu
3. Tekil (User) ve çoğul (Users) controller'lar arasında karışıklık vardı

**Çözüm:**
1. Eski proje dosyalarını yedekleyip kaldırdık:
   - Stock.API
   - Stock.Infrastructure
   - UserController.cs
2. Yeni UsersController.cs dosyasında Delete metodu oluşturduk
3. Frontend'deki UserService içindeki endpoint'leri düzelttik:
   ```typescript
   // Hatalı
   return this.http.delete<void>(`${this.apiUrl}/User/${id}`);
   
   // Düzeltilmiş
   return this.http.delete<void>(`${this.apiUrl}/Users/${id}`);
   ```

**Önemli Notlar:**
- Clean Architecture'da çoğul isimlendirme tercih edilmeli (UsersController)
- Frontend ve backend arasındaki API endpoint'leri tutarlı olmalı
- Yapısal değişiklikler sırasında eski kod tamamen kaldırılmalı veya açıkça işaretlenmeli

### Hata 3: Çoklu Command Sınıfları Çakışması

**Tarih:** 2025-03-08

**Hata Mesajı:** CS0104: 'DeleteUserCommand' adı iki farklı ad alanında bulundu.

**Nedeni:** Projede iki farklı namespace'te DeleteUserCommand sınıfı tanımlanmıştı:
1. `Stock.Application.Features.Users.Commands.DeleteUserCommand`
2. `Stock.Application.Features.Users.Commands.DeleteUser.DeleteUserCommand`

Aynı şekilde, CreateUserCommand ve UpdateUserCommand için de benzer çakışmalar vardı.

**Çözüm:**
UsersController.cs dosyasında tam nitelikli tip adları kullanıldı:

```csharp
// Hatalı kod
using Stock.Application.Features.Users.Commands.DeleteUser;

// Düzeltilmiş kod
using Stock.Application.Features.Users.Commands;
```

**Önemli Notlar:**
- Namespace'lerde tutarlı bir yapı kullanılmalı
- Aynı isimli sınıflar farklı namespace'lerde olmamalı
- Çakışma durumunda tam nitelikli tip adları kullanılabilir
- Uzun vadede, proje yapısı düzenlenerek çakışmalar giderilmeli

## Kullanıcı Güncelleme Hatası

**Hata:**
```
System.InvalidOperationException: No service for type 'MediatR.IRequestHandler`2[Stock.Application.Features.Users.Commands.UpdateUserCommand,Stock.Application.Models.DTOs.UserDto]' has been registered.
```

**Nedeni:**
1. UpdateUserCommand için bir handler sınıfı (UpdateUserCommandHandler) oluşturulmamış
2. MediatR, UpdateUserCommand için bir handler bulamıyor

**Çözüm:**
1. UpdateUserCommandHandler sınıfını oluştur:
```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user == null)
                return null;

            user.Username = request.Username;
            user.IsAdmin = request.IsAdmin;
            user.RoleId = request.RoleId;
            user.Sicil = request.Sicil;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            return _mapper.Map<UserDto>(updatedUser);
        }
    }
}
```

2. Ayrıca CreateUserCommandHandler sınıfını da oluştur (eksikse):
```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = command.Username,
                PasswordHash = _passwordHasher.HashPassword(command.Password),
                IsAdmin = command.IsAdmin,
                RoleId = command.RoleId,
                Sicil = command.Sicil
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
```

3. UpdateUserCommand'e sicil alanını ekledik:
```csharp
public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public string Sicil { get; set; } = string.Empty;
}
```

4. User entity'sine sicil alanını ekle:
```csharp
[StringLength(50)]
public string Sicil { get; set; } = string.Empty;
```

5. UserDto'ya sicil alanını ekledik:
```csharp
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string Sicil { get; set; } = string.Empty;
}
```

6. Veritabanı şemasını güncellemek için migration oluşturduk ve uyguladık:
```bash
dotnet ef migrations add AddSicilFieldToUser
dotnet ef database update
```

### Clean Architecture Perspektifinden Değerlendirme
Bu hata, Clean Architecture'ın temel prensiplerinden biri olan "iç katmanlar dış katmanlara bağımlı olmamalıdır" ilkesini vurgulamaktadır. Frontend (dış katman) tarafından beklenen bir alan (sicil), Domain ve Application katmanlarında (iç katmanlar) tanımlanmamıştı.

Çözüm süreci, Clean Architecture'ın aşağıdaki katmanlarında değişiklik gerektirdi:
- **Domain Layer**: User entity'sinin güncellenmesi
- **Application Layer**: Command ve DTO modellerinin güncellenmesi
- **Infrastructure Layer**: Veritabanı şemasının güncellenmesi
- **API Layer**: Controller'ın düzgün yapılandırılması

Bu çözüm, tüm katmanların uyumlu çalışmasını sağlayarak Clean Architecture prensiplerini korumaktadır.

### Öğrenilen Dersler
1. Veritabanı kısıtlamaları eklemeden önce mevcut verilerin bu kısıtlamalara uygun olup olmadığı kontrol edilmelidir.
2. Mevcut veriler kısıtlamalara uymuyorsa, önce veri temizliği yapılmalı veya uygulama seviyesinde kontrollerle ilerlenmelidir.
3. Hata durumlarını kullanıcıya net ve anlaşılır mesajlarla iletmek, kullanıcı memnuniyetini artırır.
4. Farklı katmanlar arasında hata mesajlarının iletimi, iyi tasarlanmış bir API sözleşmesi ile sağlanmalıdır.
5. Frontend uygulamasında API hata mesajlarını doğrudan göstermek, sorunların hızlı teşhisine olanak tanır.
6. Loglama stratejisi, hem geliştirme hem de canlı ortamlarda sorunların tespit edilmesine yardımcı olur.

## Kullanıcı Ekleme Hatası (500 Internal Server Error)

**Hata:** Kullanıcı ekleme işlemi sırasında 500 Internal Server Error alınıyor.

**Nedeni:**
1. Frontend'den gönderilen istekte `passwordHash` parametresi kullanılıyordu, ancak backend `password` parametresi bekliyordu.
2. `userToCreate` nesnesi frontend'de `passwordHash` alanına sahipti, backend'de ise bu alan `password` olarak tanımlanmıştı.
3. `CreateUserCommand` sınıfında `password` alanı varken, frontend'den `passwordHash` gönderiliyordu.

**Çözüm:**
1. `user-management.component.ts` dosyasında `passwordHash` alanı `password` olarak değiştirildi.
2. `user.service.ts` dosyasında `createUser` metodu değiştirildi:
```typescript
password: user.password || user.passwordHash || '', // Önce password, yoksa passwordHash kullan
```

3. Form değerleri konsola yazdırılarak hata ayıklama yapıldı.

**Önemli Notlar:**
- Frontend ve backend arasındaki model uyumsuzlukları API hatalarına neden olabilir
- Veri modeli değişikliklerinde hem frontend hem de backend güncellenmelidir
- Şifre gibi hassas bilgilerin üretim ortamında loglanmasından kaçınılmalıdır
- HTTP durum kodları (500, 400 vb.) sorunların kaynağını belirlemeye yardımcı olur

## Sicil Alanı Benzersizlik Kontrolü

**Hata:** Sicil alanı için benzersizlik kontrolü uygulanırken veritabanı güncelleme hatası alındı.

**Nedeni:**
1. Entity Framework Core migrasyonu, sicil alanı için veritabanında benzersiz indeks oluşturmaya çalışıyordu.
2. Veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğu için bu işlem başarısız oldu.
3. Bu durum, "The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database." hatası ile sonuçlandı.

**Çözüm:**
1. `src/Stock.Infrastructure/Data/Configurations/UserConfiguration.cs` dosyasından `Sicil` alanı için tanımlanmış benzersizlik indeksi kaldırıldı:
   ```csharp
   // Kaldırılan kod
   builder.HasIndex(u => u.Sicil)
       .IsUnique();
   ```

2. Yeni bir "RemoveSicilUniquenessConstraint" migrasyonu oluşturuldu.
3. Veritabanı güncellendi.
4. Benzersizlik kontrolü, uygulama seviyesinde (Command Handler sınıflarında) uygulandı:
   ```csharp
   // Eklenen veya Güncellenen kullanıcının sicil numarası kontrolü
   var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil && u.Id != request.Id);
   if (existingUsers.Any())
   {
       throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten başka bir kullanıcı tarafından kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
   }
   ```

5. Controller sınıflarında hata yönetimi iyileştirildi:
   ```csharp
   catch (InvalidOperationException ex)
   {
       // Sicil benzersizlik hatası veya diğer doğrulama hataları
       string errorMessage = ex.Message;
       _logger.LogWarning("Kullanıcı işlemi sırasında doğrulama hatası: {Message}", errorMessage);
       
       // Özel olarak sicil numarası hatası için kontrol et
       if (errorMessage.Contains("sicil numarası zaten"))
       {
           return BadRequest(new { error = errorMessage });
       }
       
       return BadRequest(new { error = errorMessage });
   }
   ```

6. Frontend bileşenlerinde API'dan dönen hata mesajları doğrudan kullanıcıya gösterildi:
   ```typescript
   error: (error) => {
     console.error('Kullanıcı ekleme hatası:', error);
     
     // API'dan dönen hata mesajını göster (varsa)
     const errorMessage = error.error?.error || 'Kullanıcı eklenirken bir hata oluştu.';
     
     this.messageService.add({
       severity: 'error',
       summary: 'Hata',
       detail: errorMessage,
       life: 5000
     });
   }
   ```

**Kullanıcı Deneyimi İyileştirmeleri:**
1. Hata mesajları daha açıklayıcı ve kullanıcı dostu hale getirildi.
2. Spesifik hata durumlarına göre özelleştirilmiş mesajlar sunuldu.
3. Sicil benzersizlik hatası için özel kontrol eklenip kullanıcıya net bir mesaj gösterildi.
4. Hata mesajları daha uzun süre ekranda tutuldu (5 saniye).
5. Loglama iyileştirildi ve hata ayıklama bilgileri zenginleştirildi.

**Öğrenilen Dersler:**
1. Veritabanı kısıtlamaları eklemeden önce mevcut verilerin bu kısıtlamalara uygun olup olmadığı kontrol edilmelidir.
2. Mevcut veriler kısıtlamalara uymuyorsa, önce veri temizliği yapılmalı veya uygulama seviyesinde kontrollerle ilerlenmelidir.
3. Hata durumlarını kullanıcıya net ve anlaşılır mesajlarla iletmek, kullanıcı memnuniyetini artırır.
4. Farklı katmanlar arasında hata mesajlarının iletimi, iyi tasarlanmış bir API sözleşmesi ile sağlanmalıdır.
5. Frontend uygulamasında API hata mesajlarını doğrudan göstermek, sorunların hızlı teşhisine olanak tanır.
6. Loglama stratejisi, hem geliştirme hem de canlı ortamlarda sorunların tespit edilmesine yardımcı olur.

## GitHub Senkronizasyonu ve Kullanıcı Şifre/Sicil Sorunları

**Hata:** GitHub'dan son değişiklikleri çektikten sonra kullanıcı girişi yapılamıyor. Veritabanında sadece admin kullanıcısı kalıyor ve sicil alanı boş oluyor.

**Tarih:** 2025-03-05

**Nedeni:**
1. Son migration'lar veritabanında kritik değişiklikler yapıyor:
   - Normal kullanıcı (user) siliniyor
   - User rolü siliniyor
   - Admin kullanıcısının şifresi değişiyor ve sicil alanı boşaltılıyor
2. SeedData mekanizması, veritabanında zaten bir admin kullanıcısı olduğu için çalışmıyor ve yeni kullanıcıları eklemiyor.
3. Farklı ortamlarda (ev ve iş) aynı GitHub repo kullanıldığında, her ortamda migration'lar farklı durumda olabiliyor.

**İncelenen Migration'lar:**
```sql
-- 20250305173319_PendingChanges migration'ından
DELETE FROM "Users" WHERE "Id" = 2;
DELETE FROM "Roles" WHERE "Id" = 2;
UPDATE "Users" SET "PasswordHash" = '...', "Sicil" = '' WHERE "Id" = 1;
```

**Çözüm:**
1. FixPasswordController adında yeni bir controller oluşturuldu:
   ```csharp
   [HttpGet("fix-passwords")]
   public async Task<IActionResult> FixPasswords()
   {
       // Admin kullanıcısının şifresini ve sicil alanını düzelt
       var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
       if (adminUser != null)
       {
           adminUser.PasswordHash = _passwordHasher.HashPassword("admin123");
           adminUser.Sicil = "A001";
           await _context.SaveChangesAsync();
       }
       
       // Normal kullanıcı yoksa oluştur veya güncelle
       var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
       if (normalUser == null)
       {
           // User kullanıcısı yoksa oluştur
           var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
           if (userRole == null)
           {
               // User rolü yoksa oluştur
               userRole = new Role { Name = "User", Description = "Normal kullanıcı" };
               await _context.Roles.AddAsync(userRole);
               await _context.SaveChangesAsync();
           }
           
           var newUser = new User
           {
               Username = "user",
               PasswordHash = _passwordHasher.HashPassword("user123"),
               IsAdmin = false,
               RoleId = userRole.Id,
               Sicil = "U001"
           };
           
           await _context.Users.AddAsync(newUser);
           await _context.SaveChangesAsync();
       }
       else
       {
           normalUser.PasswordHash = _passwordHasher.HashPassword("user123");
           normalUser.Sicil = "U001";
           await _context.SaveChangesAsync();
       }
   }
   ```

2. SeedData mekanizması güncellendi, artık kullanıcılar varsa da güncelleniyor:
   ```csharp
   private static async Task SeedUsersAsync(ApplicationDbContext context, IServiceProvider services)
   {
       var passwordHasher = services.GetRequiredService<IPasswordHasher>();
       
       // Admin kullanıcısını kontrol et ve güncelle
       var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
       if (adminUser == null)
       {
           // Admin kullanıcısı yoksa oluştur
           adminUser = new User
           {
               Username = "admin",
               PasswordHash = passwordHasher.HashPassword("admin123"),
               IsAdmin = true,
               Sicil = "A001"
           };
           
           await context.Users.AddAsync(adminUser);
       }
       else
       {
           // Admin kullanıcısı varsa kontrol et ve güncelle
           if (string.IsNullOrEmpty(adminUser.Sicil))
           {
               adminUser.Sicil = "A001";
           }
           
           adminUser.PasswordHash = passwordHasher.HashPassword("admin123");
           context.Users.Update(adminUser);
       }
       
       // Normal kullanıcı için benzer kontroller ve güncellemeler...
   }
   ```

**Kullanımı:**
GitHub'dan son değişiklikleri çektikten sonra, eğer kullanıcı girişi yapılamıyorsa:
1. API uygulamasını başlatın
2. `/api/FixPassword/fix-passwords` endpoint'ine istek atın
3. Bu işlem hem admin hem de normal kullanıcı hesaplarını düzeltecek

**Öğrenilen Dersler:**
1. Veritabanı şemasında kritik değişiklikler yapan migration'lar dikkatli planlanmalı
2. Seed data mekanizmaları mevcut verilerin kontrolünü ve gerektiğinde güncellemesini içermeli
3. Farklı ortamlar arasında geçiş yapıldığında veritabanı durumunu senkronize tutmak için özel mekanizmalar gerekebilir
4. Critical fix endpoint'leri gibi acil durum onarım mekanizmaları oluşturulmalı
5. Migration'ların içeriğini commit etmeden önce olası etkileri değerlendirilmeli
6. Farklı ortamlar arasındaki senkronizasyon sorunları için dokümantasyon sağlanmalı

**Önleyici Önlemler:**
1. Migration'lar, mevcut verileri silmek yerine güncellemeyi tercih etmeli
2. Seed data, var olan verilerin kontrolünü ve gerektiğinde güncellemesini içermeli
3. Kritik veritabanı değişiklikleri için rollback stratejisi planlanmalı
4. Önemli alanlar için varsayılan değerler atanmalı
5. Farklı ortamlar arasında geçiş rehberi oluşturulmalı

## Rol Terminolojisi Değişikliği

**Hata:** Kullanıcı yönetimi arayüzünde "Katkıda Bulunan" terimi kullanılıyordu, ancak bu terim proje genelindeki rol terminolojisine uygun değildi. Ayrıca, sistemde "İzleyici" rolü bulunmasına rağmen bu rol aktif olarak kullanılmıyordu.

**Nedeni:**
- Kullanıcı izinleri tablosunda "Contributor" teknik değeri için "Katkıda Bulunan" çevirisi kullanılmıştı.
- Projenin standardizasyonu için tüm arayüzlerde "Yönetici" ve "Kullanıcı" şeklinde iki ana rol kullanılması gerekiyor.
- "İzleyici" rolü kullanılmadığı halde kullanıcı arayüzünde görüntüleniyordu, bu da kullanıcıların kafasını karıştırabilirdi.

**Çözüm:**
1. İlk güncelleme: "Katkıda Bulunan" ifadesi "Kullanıcı" olarak güncellendi
   ```diff
   {{ user.permissions === 'Admin' ? 'Yönetici' : 
   -  user.permissions === 'Contributor' ? 'Katkıda Bulunan' : 
   +  user.permissions === 'Contributor' ? 'Kullanıcı' : 
      user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
   ```

2. İkinci güncelleme: "İzleyici" rolü tamamen kaldırıldı
   ```diff
   roleFilterOptions: any[] = [
     { label: 'Tümü', value: null },
     { label: 'Yönetici', value: 'Admin' },
   -  { label: 'Kullanıcı', value: 'Contributor' },
   -  { label: 'İzleyici', value: 'Viewer' }
   +  { label: 'Kullanıcı', value: 'Contributor' }
   ];
   ```

3. Kullanıcı izinleri görüntüleme kısmında "İzleyici" kontrolü kaldırıldı
   ```diff
   <span class="permission-badge" [ngClass]="{
     'admin-badge': user.permissions === 'Admin',
   -  'contributor-badge': user.permissions === 'Contributor',
   -  'viewer-badge': user.permissions === 'Viewer'
   +  'contributor-badge': user.permissions === 'Contributor'
   }">
     {{ user.permissions === 'Admin' ? 'Yönetici' : 
        user.permissions === 'Contributor' ? 'Kullanıcı' : 
   -    user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
   +    user.permissions }}
   </span>
   ```

**Önemli Notlar:**
- Teknik değerler (arka planda kullanılan değerler) değiştirilmedi, sadece kullanıcıya gösterilen etiketler güncellendi.
- Rol tabanlı erişim kontrol sistemi bu değişiklikten etkilenmedi.
- Sistem, artık sadece "Yönetici" ve "Kullanıcı" rollerini destekliyor.
- İlgili detaylı bilgi için bkz: `knowledge-base/user_role_terminology_update.md`

## Sayfa Erişim İzinleri Geliştirmesi

### Hata 1: PrimeNG Bileşenleri Görüntülenemedi
**Sorun:** Rol detay sayfasında p-accordion ve p-toggleButton bileşenleri görüntülenemedi.
**Çözüm:** Standalone bileşenlerde gerekli PrimeNG modüllerinin imports dizisine eklenmesi gerekiyor:
```typescript
@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AccordionModule,
    ToggleButtonModule,
    CheckboxModule,
    ButtonModule,
    ProgressSpinnerModule
  ]
})
```

### Hata 2: İzin Gruplarına Erişilemedi
**Sorun:** Şablonda belirli gruplar için izinlere erişirken hata alındı.
**Çözüm:** Permission servisinden izinleri grup bazlı döndürmek için yardımcı metotlar eklendi:
```typescript
getPermissionsByGroup(groupName: string): Permission[] {
  const group = this.permissionGroups.find(g => g.group === groupName);
  return group ? group.permissions : [];
}
```

### Hata 3: İzinlerin Kaydedilmesi Sırasında 401 Hatası
**Sorun:** İzin güncelleme işlemi yapıldığında 401 Unauthorized hatası alındı.
**Çözüm:** Kimlik doğrulama token'ının HTTP isteklerinde gönderildiğinden emin olmak ve API izin kontrolünü düzeltmek:
```typescript
// HTTP isteğine token ekleme
const headers = {
  'Authorization': `Bearer ${this.authService.getToken()}`
};
return this.http.post<boolean>(url, { permissionIds }, { headers });
```

### Hata 4: ToggleButton [checked] ve [value] Özelliği Hatası

**Sorun:** Rol detay sayfasında p-toggleButton bileşenleri için şu hatalar alındı:
```
Can't bind to 'checked' since it isn't a known property of 'p-toggleButton'.
Can't bind to 'value' since it isn't a known property of 'p-toggleButton'.
```

**Nedeni:**
1. ToggleButton bileşeni, standart HTML input bileşenleri gibi `checked` ve `value` özelliklerini desteklemiyor.
2. PrimeNG ToggleButton, bir dizi içinde değer varlığı kontrolü için doğrudan kullanılamıyor.

**Çözüm:**
1. `[checked]` ve `[value]` özellikleri yerine tek yönlü veri bağlama kullan:
```html
<!-- Hatalı kod -->
<p-toggleButton 
    [checked]="isPermissionSelected(permission.id)" 
    [(ngModel)]="selectedPermissions"
    [value]="permission.id">
</p-toggleButton>

<!-- Doğru kod -->
<p-toggleButton 
    [ngModel]="isPermissionSelected(permission.id)"
    (onChange)="togglePermission(permission.id)">
</p-toggleButton>
```

2. isPermissionSelected metodu, izin ID'sinin seçili izinler listesinde olup olmadığını kontrol eder:
```typescript
isPermissionSelected(permissionId: number): boolean {
  return this.selectedPermissions.includes(permissionId);
}
```

3. togglePermission metodu, izin ID'sini izin listesinden ekler veya çıkarır:
```typescript
togglePermission(permissionId: number): void {
  const index = this.selectedPermissions.indexOf(permissionId);
  if (index > -1) {
    this.selectedPermissions.splice(index, 1);
  } else {
    this.selectedPermissions.push(permissionId);
  }
}
```

**Öğrenilen Dersler:**
- PrimeNG bileşenleri, standart HTML bileşenlerinden farklı şekilde çalışır ve farklı özellikler kullanır.
- ToggleButton için değer listesi bağlama doğrudan desteklenmez, özel bir mantıkla yönetilmelidir.
- Bileşenlerde sorun yaşandığında, PrimeNG dokümantasyonunu kontrol etmek faydalıdır.

### Hata 5: Admin/Roles Rota Hatası

**Sorun:** Admin dashboard'dan rol yönetimi sayfasına gitmeye çalışıldığında aşağıdaki hata alınıyor:
```
ERROR RuntimeError: NG04002: Cannot match any routes. URL Segment: 'admin/roles'
```

**Nedeni:**
1. Admin dashboard bileşeninde `navigateToRoleManagement()` metodu `/admin/roles` rotasına yönlendiriyor
2. Ancak app.routes.ts dosyasında `admin/roles` rotası tanımlanmamış
3. Sadece `roles/:id` ve `role-management` rotaları tanımlanmış

**Çözüm:**
1. app.routes.ts dosyasına eksik olan `admin/roles` rotasını ekle:
```typescript
{ 
  path: 'admin/roles', 
  component: RoleManagementComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.RoleManagement'
  }
}
```

2. Ayrıca tutarlılık için `roles` rotasını da ekle:
```typescript
{ 
  path: 'roles', 
  component: RoleManagementComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.RoleManagement'
  }
}
```

**Öğrenilen Dersler:**
- Bir sayfaya yönlendirme yapmadan önce, hedef rotanın app.routes.ts dosyasında tanımlanmış olduğundan emin olunmalı
- Admin dashboard gibi merkezi bir komponenten yapılan yönlendirmeler kontrol edilmeli
- Angular'da rotaların tutarlı bir şekilde tanımlanması önemli
- URL yolları ve rota tanımları arasında uyumsuzluk olmamalı

### Hata 6: API Endpoint Uyumsuzluğu (Rol Yönetimi)

**Sorun:** Rol yönetimi sayfasında roller yüklenirken aşağıdaki hata alınıyor:
```
GET http://localhost:5037/api/roles 404 (Not Found)
```

**Nedeni:**
1. Frontend'de RoleService, `api/roles` endpoint'ine istek yapıyor
2. Backend'de controller `api/Role` endpoint'ini sunuyor (controller adı RoleController)
3. ASP.NET Core'da controller adı rota olarak kullanılıyor ve büyük/küçük harf duyarlı

**Çözüm:**
Frontend'deki RoleService'de API URL'sini değiştir:
```typescript
// Hatalı
private apiUrl = `${environment.apiUrl}/api/roles`;

// Doğru
private apiUrl = `${environment.apiUrl}/api/Role`;
```

**Alternatif Çözüm:**
Backend'de controller adını değiştirmek:
```csharp
// Hatalı
public class RoleController : ControllerBase

// Doğru
public class RolesController : ControllerBase
```

**Öğrenilen Dersler:**
- API endpoint'leri frontend ve backend arasında tam olarak eşleşmeli
- ASP.NET Core'da controller adları rota olarak kullanılır ve büyük/küçük harf duyarlıdır
- Frontend ve backend arasındaki API sözleşmesi tutarlı olmalı
- API endpoint'lerini test etmek için Swagger veya Postman kullanılabilir

### Hata 7: PrimeNG Table Responsive Özelliği Uyarısı

**Sorun:** Rol yönetimi sayfasında PrimeNG Table bileşeni için aşağıdaki uyarı alınıyor:
```
responsive property is deprecated as table is always responsive with scrollable behavior.
```

**Nedeni:**
PrimeNG'nin güncel sürümlerinde Table bileşeni her zaman responsive olarak çalışıyor ve `responsive` özelliği kullanımdan kaldırılmış.

**Çözüm:**
Table bileşeninden `[responsive]="true"` özelliğini kaldır:
```html
<!-- Hatalı -->
<p-table [value]="roles" [paginator]="true" [rows]="10" [responsive]="true" [loading]="loading">

<!-- Doğru -->
<p-table [value]="roles" [paginator]="true" [rows]="10" [loading]="loading">
```

**Öğrenilen Dersler:**
- Kütüphane güncellemeleriyle bazı özellikler kullanımdan kaldırılabilir
- Konsol uyarılarını düzenli olarak kontrol etmek ve çözmek önemli
- PrimeNG gibi UI kütüphanelerinin dokümantasyonunu takip etmek gerekli

### Hata 8: 500 Internal Server Error (Rol Yönetimi) - Döngüsel Referans

**Sorun:** Rol yönetimi sayfasında roller yüklenirken aşağıdaki hata alınıyor:
```
GET http://localhost:5037/api/Role 500 (Internal Server Error)

System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
```

**Nedeni:**
1. RoleController'da `Include(r => r.Users)` kullanılması döngüsel referans sorununa neden oluyor
2. JSON serialization sırasında Role -> Users -> Role -> Users -> ... şeklinde sonsuz döngü oluşuyor
3. Döngüsel referanslar için uygun JSON serialization ayarları kullanılmamış
4. Entity Framework ile ilişkisel verileri döndürürken projection kullanılmamış

**Çözüm 1: RoleController'da döngüsel referans sorununu önlemek için Select ile projection kullanımı:**
```csharp
// Hatalı
return await _context.Roles.Include(r => r.Users).ToListAsync();

// Doğru
var roles = await _context.Roles
    .AsNoTracking() // Performans için
    .Select(r => new 
    {
        r.Id,
        r.Name,
        r.Description,
        r.CreatedAt,
        r.UpdatedAt,
        UserCount = r.Users.Count,
        // İlişkili kullanıcıları Role referansı olmadan seçici şekilde al
        Users = r.Users.Select(u => new { u.Id, u.Username }).ToList()
    })
    .ToListAsync();

return Ok(roles);
```

**Çözüm 2: Program.cs'de JSON serialization ayarlarını Preserve olarak güncelleme:**
```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Döngüsel referansları yoksaymak (IgnoreCycles) yerine özel formatta (Preserve) koru
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        // Null değerleri dahil etme
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
```

**Çözüm 3: Tüm Controller'larda hata yönetimi ve loglama geliştirmeleri:**
```csharp
try
{
    _logger.LogInformation("İşlem başlatılıyor...");
    // İşlemler...
    _logger.LogInformation("İşlem başarılı.");
    return Ok(result);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Hata mesajı");
    return StatusCode(500, "Kullanıcı dostu hata mesajı: " + ex.Message);
}
```

**Çözüm 4: Özel güvenlik kontrollerinin eklenmesi:**
```csharp
// Rol silme işleminde ilişkili kullanıcı kontrolü
var usersWithRole = await _context.Users.AnyAsync(u => u.RoleId == id);
if (usersWithRole)
{
    _logger.LogWarning($"ID: {id} olan rol, kullanıcılar tarafından kullanılıyor ve silinemez.");
    return BadRequest("Bu rol, bir veya daha fazla kullanıcı tarafından kullanılıyor ve silinemez.");
}
```

**Öğrenilen Dersler:**
1. **Entity Framework İlişkilerde Dikkat:**
   - Entity Framework ile ilişkisel verileri döndürürken döngüsel referanslar oluşabilir
   - Select ile projection kullanarak sadece ihtiyaç duyulan verileri seçmek önemlidir
   - AsNoTracking() ile sorgu performansı iyileştirilebilir

2. **JSON Serialization Stratejileri:**
   - Döngüsel referanslar için `ReferenceHandler.IgnoreCycles` (referansları yoksay) veya `ReferenceHandler.Preserve` (referansları özel formatta koru) kullanılabilir
   - Preserve, referans ilişkilerini koruyarak daha güvenilir sonuçlar verir
   - Null değerler için `DefaultIgnoreCondition` ayarları önemlidir

3. **Hata Yönetimi ve Güvenlik:**
   - Controller'larda tüm işlemler try-catch bloklarına alınmalıdır
   - Detaylı loglama yapılmalıdır
   - İlişkisel verilerin bütünlüğü için güvenlik kontrolleri eklenmelidir
   - Kullanıcı dostu hata mesajları döndürülmelidir

4. **Optimizasyon Teknikleri:**
   - Sadece ihtiyaç duyulan verileri getirmek (Select kullanarak)
   - Değişiklik takibi gerektirmeyen sorgularda AsNoTracking() kullanmak
   - Performans için uygun indekslerin oluşturulması

## PrimeNG Table ve Döngüsel Referans Hatası

**Hata:**
```
ERROR TypeError: _data.slice is not a function
    at _Table.dataToRender (primeng-table.mjs:2725:20)
```

**Nedeni:**
1. Backend API'de `ReferenceHandler.Preserve` ayarı kullanıldığında, JSON yanıtları normal dizi yerine referans yapılarını korumak için özel bir formatta geliyor:
   ```json
   {
     "$id": "1",
     "$values": [
       { "id": 1, "name": "Admin", ... },
       { "id": 2, "name": "User", ... }
     ]
   }
   ```

2. PrimeNG Table veya diğer bileşenler, bu formatı değil düz dizileri bekliyor.

3. Frontend bileşenleri API'den gelen verileri doğrudan kullandığında, `$values` içindeki diziyi değil, üst düzey nesneyi kullanmaya çalışıyor.

**Çözüm:**
1. Frontend'de veri dönüşümü yaparak API'den gelen yanıtları beklenen formata dönüştürme:
```typescript
loadRoles() {
  this.roleService.getRoles().subscribe({
    next: (data: any) => {
      // Döngüsel referans düzeltmesi - ReferenceHandler.Preserve formatını düz diziye dönüştür
      if (data && data.$values) {
        this.roles = data.$values as Role[];
      } else if (data && Array.isArray(data)) {
        this.roles = data as Role[];
      } else if (data) {
        // Herhangi bir şekilde veri varsa bunu dizi olarak al
        this.roles = Object.values(data).filter(item => typeof item === 'object' && item !== null) as Role[];
      } else {
        this.roles = [];
      }
    }
  });
}
```

2. Log verilerini işleme için benzer dönüşüm:
```typescript
loadUserActivityLogs(): void {
  this.logService.getUserActivityLogs().subscribe({
    next: (response: any) => {
      // Farklı veri formatlarını kontrol et
      if (response && response.logs && response.logs.$values) {
        this.userActivityLogs = response.logs.$values;
      } else if (response && response.$values) {
        this.userActivityLogs = response.$values;
      } 
      // ... diğer format kontrolleri ...
    }
  });
}
```

**Öğrenilen Dersler:**
1. API tarafında `ReferenceHandler.Preserve` kullanılırken, frontend'de veri dönüşümü yapılması gerekir
2. API yanıtlarında çeşitli formatlar kontrol edilmeli: `$values`, Array, nesne özellikleri 
3. Type güvenliği için `as` operatörü ile tip dönüşümü yapılmalı
4. PrimeNG Table, düz dizi bekler ve `$values` formatındaki verilerle çalışmaz

**Alternatif Çözüm:**
Backend'de API yanıtı için JSON serileştirme sırasında dönüşüm yapmak:

```csharp
// API Controller'ında
return Ok(roles.Select(r => new 
{
    r.Id,
    r.Name,
    r.Description,
    // ... diğer özellikler
}).ToList());
```

## Angular *ngFor ve ReferenceHandler.Preserve Hatası

### Hata
```
ERROR RuntimeError: NG02200: Cannot find a differ supporting object '[object Object]' of type 'object'. 
NgFor only supports binding to Iterables, such as Arrays. Did you mean to use the keyvalue pipe?
```

### Nedeni
Bu hata, Angular *ngFor direktifi ile bir dizi (array) olmayan bir nesne (object) üzerinde döngü yapmaya çalıştığımızda oluşur. ReferenceHandler.Preserve kullanıldığında, API'den gelen veriler `{ $id: "1", $values: [...] }` formatında olur, bu da *ngFor ile doğrudan kullanılamaz.

### Çözüm

1. **Bileşen Seviyesinde**:
```typescript
// ReferenceHandler.Preserve için ek tip tanımı
interface PreserveFormat<T> {
  $id?: string;
  $values?: T[];
}

// API çağrıları sonrasında, veriyi normalizasyon işlemi
if (data.someArray && Array.isArray(data.someArray)) {
  this.someArray = data.someArray;
} else if (data.someArray && (data.someArray as PreserveFormat<SomeType>).$values) {
  const preserveData = data.someArray as PreserveFormat<SomeType>;
  this.someArray = preserveData.$values || [];
} else {
  console.error('someArray uygun formatta değil:', data.someArray);
  this.someArray = [];
}
```

2. **Servis Seviyesinde**:
```typescript
private normalizeResponse<T>(response: any): T {
  if (!response) return [] as unknown as T;
  
  // $values dizisi varsa
  if (response.$values) {
    return response.$values as T;
  } 
  // Dizi ise doğrudan döndür
  else if (Array.isArray(response)) {
    return response as T;
  } 
  // Son çare: orijinal yanıtı döndür
  return response as T;
}

// Servis metodlarında kullanımı
getItems(): Observable<Item[]> {
  return this.http.get<any>(this.apiUrl)
    .pipe(map(response => this.normalizeResponse<Item[]>(response)));
}
```

3. **Template'te Keyvalue Pipe Kullanımı (Alternatif)**:
```html
<div *ngFor="let item of someObjectData | keyvalue">
  {{ item.key }}: {{ item.value }}
</div>
```

### Öğrenilen Dersler
1. ReferenceHandler.Preserve kullanıldığında, API'den gelen veriler özel bir formatta olur ve normalizasyon gerektirir.
2. Bileşen seviyesinde, veri template'e bağlanmadan önce doğru formata dönüştürülmelidir.
3. *ngFor sadece diziler, Map, Set gibi yinelenebilir (iterable) nesnelerle çalışır.
4. Güvenli bir yaklaşım için hem servis seviyesinde hem de bileşen seviyesinde normalizasyon uygulanmalıdır.
5. Veri yapılarının undefined veya null olma ihtimali her zaman kontrol edilmelidir.

## ReactiveForm Disabled Özelliği Sorunu

**Hata:** Kullanıcı yönetimi sayfasında izinler ve ad soyad bilgileri görünmüyor ve konsola şu hata mesajı düşüyor:

```
It looks like you're using the disabled attribute with a reactive form directive. If you set disabled to true
when you set up this control in your component class, the disabled attribute will actually be set in the DOM for
you. We recommend using this approach to avoid 'changed after checked' errors.
```

**Nedeni:**
1. ReactiveForm içindeki form kontrolünü HTML şablonunda `[disabled]` özelliği ile devre dışı bırakmak yanlıştır
2. Kullanıcı verilerinin dönüşümünde eksiklik vardı - username fullName'e atanmıyordu
3. Permissions (izinler) alanının veri dönüşümü eksikti

**Çözüm:**
1. HTML'den `[disabled]` niteliğini kaldırıp, component sınıfında formun oluşturulması sırasında veya daha sonra `disable()/enable()` metotlarını kullanmak:

```typescript
// Form oluşturma
this.userForm = this.formBuilder.group({
  // ...
  roleId: [{value: 2, disabled: false}] // başlangıçta etkin
});

// Dinamik olarak devre dışı bırakma/etkinleştirme
this.userForm.get('isAdmin')?.valueChanges.subscribe(isAdmin => {
  const roleIdControl = this.userForm.get('roleId');
  
  if (isAdmin) {
    roleIdControl?.patchValue(1, {emitEvent: false});
    roleIdControl?.disable({emitEvent: false});
  } else {
    roleIdControl?.enable({emitEvent: false});
    if (roleIdControl?.value === 1) {
      roleIdControl?.patchValue(2, {emitEvent: false});
    }
  }
});
```

2. Veri dönüşümünü düzeltme:

```typescript
loadUsers() {
  this.userService.getUsers().subscribe({
    next: (data) => {
      this.users = data.map(user => {
        return {
          id: user.id,
          fullName: user.username, // Kullanıcı adını fullName alanına at
          sicil: user.sicil || `N/A-${user.id}`,
          permissions: user.isAdmin ? 'Admin' : (user.roleName || 'Kullanıcı'),
          roleId: user.roleId || (user.isAdmin ? 1 : 2),
          isAdmin: user.isAdmin,
          isActive: true,
          createdAt: user.createdAt || new Date()
        };
      });
      
      // ...
    }
  }
}
```

3. Form gönderiminde disabled alanları da almak için `form.getRawValue()` kullanma:

```typescript
const userData = {...this.userForm.getRawValue()};
```

**Önemli Notlar:**
- Angular'da ReactiveForm kullanırken, disabled durumunu her zaman component sınıfı üzerinden yönetin, HTML şablonu üzerinden değil
- Disabled alanların değerlerini form.value ile alamazsınız, form.getRawValue() kullanmalısınız
- Form alanları arasında bağımlılık varsa, valueChanges ile dinleyip tepki vermeniz gerekir
- Backend'den alınan verilerin frontend'de gösterilmesi için doğru şekilde dönüştürülmesi gerekir

**Referans:** [knowledge-base/user_management_troubleshooting.md](../knowledge-base/user_management_troubleshooting.md)

## Kullanıcı Güncelleme ID Uyuşmazlığı Hatası

**Tarih:** 2025-06-03

**Hata Mesajı:**
```
PUT http://localhost:5037/api/Users/2 400 (Bad Request)
warn: Stock.API.Controllers.UsersController[0]
      ID uyuşmazlığı: URL'deki 2 ile gönderilen 0 eşleşmiyor
```

**Nedeni:**
1. Backend UsersController'da kullanıcı güncellemesi yapılırken, URL'deki ID (örneğin `/api/Users/2`) ile request body'deki ID'nin (örneğin `0` veya eksik) aynı olması beklenir.
2. Frontend'den gönderilen kullanıcı nesnesinde `id` alanı eksik veya `0` olarak gönderiliyor.
3. Backend'de aşağıdaki kontrol yapılıyor:
```csharp
if (id != command.Id)
{
    _logger.LogWarning("ID uyuşmazlığı: URL'deki {UrlId} ile gönderilen {CommandId} eşleşmiyor", id, command.Id);
    return BadRequest(new { error = "URL'deki ID ile request body'deki ID eşleşmiyor." });
}
```

**Çözüm:**
1. Frontend tarafında `saveUser` metodunda, düzenleme modunda gönderilen formData nesnesine `id` alanını eklemek:
```typescript
// Düzenleme modunda id değerini ekleyelim
if (this.editMode) {
  formData.id = this.user.id; // ID'yi request body'ye ekliyoruz
}
```

2. Frontend'de hata mesajını daha açıklayıcı hale getirmek:
```typescript
if (error.error && error.error.error) {
  errorMessage = `Hata: ${error.error.error}`;
}
```

3. Backend'de ID kontrolünü düzgün loglamak (hali hazırda yapılmış durumda)

**Önemli Notlar:**
- Frontend-backend arasındaki model uyumlulukları önemlidir
- Backend API'ye yapılan isteklerde gönderilen verilerin formatına dikkat edilmelidir
- Hata mesajları kullanıcıya anlamlı bir şekilde gösterilmelidir
- HTTP 400 Bad Request hatası, genellikle istek formatının doğru olmadığını gösterir

## Ortamlar Arası Geçiş Sorunları

**Tarih:** 2025-06-03

**Sorun:** 
İş ve ev ortamları arasında geçiş yaparken, GitHub'dan son değişiklikleri çektikten sonra veritabanı ve kullanıcı verileri tutarsızlıkları oluşabiliyor. Özellikle:
1. Kullanıcı giriş bilgileri (şifreler) çalışmayabiliyor
2. Rol tanımlamaları eksik olabiliyor
3. Sicil alanları boş kalabiliyor 
4. Veritabanı migration'ları tutarsız olabiliyor

**Nedeni:**
1. GitHub'dan çekilen migration'lar halihazırda uygulanmış olabiliyor
2. Veritabanında bazı kullanıcı bilgileri (özellikle şifreler ve sicil alanları) sıfırlanmış olabiliyor
3. Farklı ortamlardaki veritabanı durumları arasında tutarsızlıklar olabiliyor
4. SeedData mekanizması, sadece veritabanı boşsa çalışacak şekilde tasarlanmış olabiliyor

**Çözüm:**
1. Ortamlar arası geçiş yaptıktan sonra, kullanıcı verilerini düzeltmek için özel bir endpoint kullanın:
```
http://localhost:5037/api/FixPassword/fix-passwords
```

2. Bu endpoint aşağıdaki işlemleri gerçekleştirir:
   - Admin kullanıcısının şifresini ve sicil alanını düzeltir
   - Normal kullanıcı hesaplarını kontrol eder ve günceller veya oluşturur
   - Rol tanımlamalarını kontrol eder ve günceller veya oluşturur

3. Veritabanı migration sorunları için temiz başlangıç yapın (gerekirse):
```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

4. Sistem Başlatma Rehberi'nde belirtilen adımları takip edin

**Öğrenilen Dersler:**
1. Farklı ortamlar arasında geçiş yaparken, çalışma adımlarını standartlaştırmak önemlidir
2. Veritabanını düzeltmek ve güncellemek için özel mekanizmalar oluşturmak faydalıdır
3. Acil durum onarım endpoint'leri geliştirme sürecinde çok yardımcı olabilir
4. Migration sorunlarında temiz başlangıç yapmak bazen en iyi çözümdür
5. Ortamlar arası geçiş sürecini belgelemek ve sistem başlatma rehberine eklemek, sorunların hızlı çözülmesine yardımcı olur

## Kullanıcı Dashboard Erişim Sorunları

### Sorun
Kullanıcılar giriş yaptıktan sonra, dashboard sayfasına erişmeye çalıştıklarında şu hatalar görülüyordu:

```
Login isteği gönderiliyor: {sicil: 'U001', password: '123456'}
auth.service.ts:67 Yönlendiriliyor: /user-dashboard
login.component.ts:170 Login başarılı: {$id: '1', success: true, token: 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiO…50In0.6mKPqIvHfpwJ1K8_I-IQvaC6dRoN2WUPDV-99JC1gwI', username: 'Test', isAdmin: false}
login.component.ts:181 Yönlendiriliyor: /user-dashboard
2auth.guard.ts:43 Erişim reddedildi: 'Pages.UserDashboard' izni yok
auth.service.ts:69 Yönlendirme başarılı
4112auth.guard.ts:43 Erişim reddedildi: 'Pages.UserDashboard' izni yok
```

Hata, standart kullanıcıların `Pages.UserDashboard` iznine sahip olmamasından kaynaklanıyordu. Bu, backend'deki `SeedRolePermissionsAsync` metodunda standart kullanıcı rolüne bu iznin verilmemesinden dolayı oluşuyordu.

### Analiz
1. Backend'de `SeedRolePermissionsAsync` metodunda standart kullanıcı rolüne `Pages.UserDashboard` izni atanmamıştı.
2. Frontend'de `UserDashboardComponent` için rota tanımında `requiredPermission: 'Pages.UserDashboard'` şartı bulunuyordu.
3. `AuthGuard` sınıfı, bu izni kontrol ediyor ve yoksa erişimi reddediyordu.
4. Kullanıcılar için dashboard erişimi olmadığından, sürekli login sayfasına yönlendiriliyorlardı.

### Çözüm
İki farklı çözüm yaklaşımı değerlendirildi:

1. **Backend çözümü**: `SeedRolePermissionsAsync` metodunu güncelleyerek standart kullanıcı rolüne `Pages.UserDashboard` iznini eklemek.
2. **Frontend çözümü**: `AuthGuard` sınıfını güncelleyerek user-dashboard sayfası için izin kontrolünü bypass etmek.

Frontend çözümü daha hızlı ve yeniden seed işlemi gerektirmediğinden tercih edildi:

```typescript
// AuthGuard sınıfındaki canActivate metoduna eklenen özel durumlar

// Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
if (url === '/user-dashboard') {
  return true;
}

// Route data'sında izin belirtilmişse kontrol et
if (route.data['requiredPermission']) {
  const requiredPermission = route.data['requiredPermission'];
  
  // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
  if (requiredPermission === 'Pages.UserDashboard') {
    return true;
  }
  
  // ... diğer kontroller ...
}

// Sayfa yoluna göre izin kontrolü
const pagePermission = this.pagePermissionMap[url];
if (pagePermission) {
  // Özel durum: user-dashboard sayfası için izin kontrolünü bypass et
  if (pagePermission === 'Pages.UserDashboard') {
    return true;
  }
  
  // ... diğer kontroller ...
}
```

### Ayrıca Yapılan İyileştirmeler
- `AuthService.login()` metodunda, token'dan izinlerin yüklenmesi için kod eklendi:
```typescript
// İzinleri yükle
this.permissions = decodedToken.Permission || [];
```

### İleriye Dönük Öneriler
Kalıcı çözüm olarak, backend'de `SeedRolePermissionsAsync` metodunu güncelleyerek standart kullanıcı rolüne `Pages.UserDashboard` izninin eklenmesi önerilir. Buna ek olarak, tüm kullanıcıların BİLGİ İŞLEM modülüne erişebilmesi için `Pages.BilgiIslem.View` izni de eklenmesi önerilir.

Detaylı bilgiler için bakınız: [knowledge-base/auth_knowledge_base.md](knowledge-base/auth_knowledge_base.md)

### Aktivite Log Hataları (07.03.2025)

#### Hata: Toplu Aktivite Logları Kaydederken 500 Internal Server Error
**Tarih:** 2025-03-07
**Hata Mesajı:** 
```
POST http://localhost:5037/api/logs/bulk-activity 500 (Internal Server Error)
Bekleyen logları senkronize etme hatası: [...] "Toplu aktivite logları kaydedilirken bir hata oluştu."
```

**Nedeni:**
1. Frontend'den gönderilen model yapısı (UserActivityLog) ile backend'de beklenen model yapısı (ActivityLog) arasında uyumsuzluk vardı
2. Frontend'den gönderilen bazı alanlar (status, details, ipAddress) backend modelinde bulunmamaktaydı
3. JSON deserializasyon hatası oluşuyordu çünkü backend doğrudan ActivityLog tipine dönüştürmeye çalışıyordu

**Çözüm:**
1. ActivityLogController'daki metotları güncellendi:
   - Önce object tipinde veri alacak şekilde parametre değiştirildi
   - JSON'ı manuel olarak çözümleyen ve doğru şekilde ActivityLog'a dönüştüren kod eklendi
   - Frontend'den gelen fazla alanlar (status, details, ipAddress) AdditionalInfo alanında JSON olarak saklandı
2. Hata yönetimi ve loglama geliştirildi:
   - Daha detaylı hata mesajlarıyla loglama eklendi
   - Geliştirme ortamında daha açıklayıcı hata yanıtları döndürüldü

**Öğrenilen Dersler:**
1. Frontend ve backend model yapılarının farklı olduğu durumlarda, ya modelleri birbirine uyumlu hale getirmeli ya da manuel dönüştürme işlemleri yapılmalıdır
2. API'lerde her zaman detaylı hata loglaması yapılmalıdır (InnerException ve StackTrace dahil)
3. API yanıtlarında açıklayıcı hata mesajları verilmeli, ancak bunlar sadece geliştirme ortamında detaylı olmalıdır

### ILogger Paketi Eksiklik Hatası (08.03.2025)

#### Hata: Microsoft.Extensions.Logging Referans Eksikliği
**Tarih:** 2025-03-08
**Hata Mesajı:** 
```
Stock.Application 3 hata ile başarısız oldu (1,2sn)
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Application\Features\Users\Handlers\CreateUserCommandHandler.cs(11,28): error CS0234: 'Logging' tür veya ad alanı adı 'Microsoft.Extensions' ad alanında yok (bir derleme başvurunuz mu eksik?)
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Application\Features\Users\Handlers\CreateUserCommandHandler.cs(20,26): error CS0246: 'ILogger<>' türü veya ad alanı adı bulunamadı (bir using yönergeniz veya derleme başvurunuz mu eksik?)
C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Application\Features\Users\Handlers\CreateUserCommandHandler.cs(26,13): error CS0246: 'ILogger<>' türü veya ad alanı adı bulunamadı (bir using yönergeniz veya derleme başvurunuz mu eksik?)
```

**Nedeni:**
1. Stock.Application projesine gerekli Microsoft.Extensions.Logging paket referansı eklenmeden ILogger kullanılmaya çalışıldı
2. Farklı projelerde paket referansları eklenmediğinde derleme sırasında assembly bulunamama hataları oluşabilir

**Çözüm:**
1. Stock.Application projesine Microsoft.Extensions.Logging NuGet paketi eklendi:
```
cd src/Stock.Application
dotnet add package Microsoft.Extensions.Logging
```

**Öğrenilen Dersler:**
1. Kod yeni bir dependency (bağımlılık) kullandığında ilgili projeye NuGet paketini eklemeyi unutmayın
2. Visual Studio IDE kullanıyorsanız, eksik referanslar için otomatik öneriler sunar, ancak komut satırında manuel olarak paketlerin eklenmesi gerekir
3. Her bir projenin kendi .csproj dosyasında kendi paket referanslarını içermesi gerektiğini unutmayın

### Log Senkronizasyon Hatası (08.03.2025)

#### Hata: Aktivite Logları Senkronizasyon Hatası
**Tarih:** 2025-03-08
**Hata Mesajı:** 
```
POST http://localhost:5037/api/logs/bulk-activity 400 (Bad Request)
Bekleyen logları senkronize etme hatası: HttpErrorResponse {headers: _HttpHeaders, status: 400, statusText: 'Bad Request', url: 'http://localhost:5037/api/logs/bulk-activity', ok: false, …}error: "İşlenebilecek log bulunamadı. Gönderilen veriler geçersiz format içeriyor olabilir."
```

**Nedeni:**
1. Frontend'den gönderilen log verilerinin formatı ile backend'de beklenen format arasında uyumsuzluk vardı
2. Frontend'de `userId` alanı opsiyonel olarak tanımlanmışken, backend'de zorunlu alan olarak tanımlanmıştı
3. JSON dönüşüm hataları yeterince iyi yönetilmiyordu

**Çözüm:**
1. ActivityLogController'daki `CreateBulkActivityLogs` ve `CreateActivityLog` metotları güncellendi:
   - `userId` alanı eksik veya null olduğunda varsayılan değer (1) atanacak şekilde düzenlendi
   - JSON dönüşüm hataları daha iyi yönetildi
   - Tarih dönüşüm hataları için try-catch blokları eklendi
   - Eksik alanlar için varsayılan değerler atandı

**Öğrenilen Dersler:**
1. Frontend ve backend arasındaki model uyumsuzluklarını daha esnek bir şekilde yönetmek gerekiyor
2. Zorunlu alanlar için varsayılan değerler atamak, sistemin daha dayanıklı olmasını sağlar
3. JSON dönüşüm hatalarını daha iyi yönetmek için try-catch blokları kullanılmalı
4. Hata mesajları daha açıklayıcı olmalı ve kullanıcıya yardımcı olmalı

## Kullanıcı İzinleri Kaydetme Hatası

**Hata Mesajı:**
```
POST http://localhost:5037/api/permissions/user/20/assign 404 (Not Found)
```

**Hata Detayı:**
Kullanıcı izinleri sayfasında, izinleri kaydetmeye çalışırken 404 Not Found hatası alınıyordu. Frontend uygulaması `http://localhost:5037/api/permissions/user/20/assign` endpoint'ine POST isteği gönderiyor, ancak bu endpoint backend'de tanımlanmamıştı.

**Nedeni:**
PermissionsController'da kullanıcıya birden fazla izin atamak için bir endpoint tanımlanmamıştı. Sadece tek bir izin atamak için `[HttpPost("user/{userId}/assign/{permissionId}")]` endpoint'i vardı.

**Çözüm:**
PermissionsController'a yeni bir endpoint ekledik:

```csharp
[HttpPost("user/{userId}/assign")]
[RequirePermission("Users.Permissions.Assign")]
public async Task<ActionResult<bool>> AssignPermissionsToUser(int userId, [FromBody] AssignPermissionsRequest request)
{
    try
    {
        _logger.LogInformation($"Kullanıcıya izinler atanıyor - Kullanıcı ID: {userId}, İzin sayısı: {request.PermissionIds.Count}");
        
        bool result = true;
        foreach (var permissionId in request.PermissionIds)
        {
            var assignResult = await _permissionService.AssignPermissionToUserAsync(userId, permissionId, true);
            if (!assignResult)
            {
                result = false;
                _logger.LogWarning($"İzin atanamadı - Kullanıcı ID: {userId}, İzin ID: {permissionId}");
            }
        }
        
        return Ok(result);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Kullanıcıya izinler atanırken hata oluştu - Kullanıcı ID: {userId}");
        return StatusCode(500, new { error = "Kullanıcıya izinler atanırken bir hata oluştu: " + ex.Message });
    }
}
```

Bu endpoint, frontend'den gelen izin ID'lerini alarak her biri için `_permissionService.AssignPermissionToUserAsync` metodunu çağırıyor.

**Öğrenilen Ders:**
1. Frontend ve backend arasındaki API endpoint'lerinin uyumlu olması gerekir.
2. Frontend'in kullandığı tüm endpoint'lerin backend'de tanımlanmış olduğundan emin olunmalı.
3. API endpoint'lerini tasarlarken, tek bir kaynak için tekil işlemler yanında toplu işlemler için de endpoint'ler düşünülmeli.
4. Hata durumlarında detaylı loglama yapılmalı ve anlamlı hata mesajları döndürülmeli.

## Clean Architecture Geçişi Hataları

### Solution Yapısı Sorunları

**Hata:** Proje referansları ve bağımlılıklar düzgün ayarlanmamış

**Nedeni:**
1. Katmanlar arası referanslar eksik
2. NuGet paketleri yanlış projelerde
3. Circular dependency riski

**Çözüm:**
1. Proje referanslarını düzenle:
```xml
<!-- Stock.Application -->
<ProjectReference Include="..\Stock.Domain\Stock.Domain.csproj" />

<!-- Stock.Infrastructure -->
<ProjectReference Include="..\Stock.Domain\Stock.Domain.csproj" />
<ProjectReference Include="..\Stock.Application\Stock.Application.csproj" />

<!-- Stock.API -->
<ProjectReference Include="..\Stock.Infrastructure\Stock.Infrastructure.csproj" />
```

2. NuGet paketlerini doğru projelere taşı:
- Domain: Sadece temel .NET paketleri
- Application: AutoMapper, FluentValidation
- Infrastructure: Entity Framework Core, Npgsql
- API: ASP.NET Core paketleri, Swagger

**Önemli Notlar:**
- Domain katmanı hiçbir katmana bağımlı olmamalı
- Application katmanı sadece Domain'e bağımlı olmalı
- Infrastructure hem Domain hem Application'a bağımlı olabilir
- API katmanı Infrastructure'a bağımlı olabilir

### Eski Yapı Temizliği

**Sorun:** Clean Architecture geçişi sonrası eski yapı hala projede bulunuyordu

**Nedeni:**
1. Geçiş sırasında eski klasörler silinmemiş
2. Kök dizinde ve src klasöründe aynı isimli projeler vardı
3. Karışıklığa neden olabilecek eski dosyalar mevcuttu

**Çözüm:**
1. Eski yapıyı yedekle (backup_old_structure klasörüne)
2. Aşağıdaki eski klasörleri ve dosyalarını sil:
   - backend/StockAPI
   - Stock.API (kök dizindeki)
   - Stock.Infrastructure (kök dizindeki)
   - src/Stock.sln (boş solution dosyası)
   - CreateAdminUser.cs (kök dizindeki boş dosya)

**Önemli Notlar:**
- Temizlik öncesi mutlaka yedek alınmalı
- Silme işlemi öncesi dosyaların içeriği kontrol edilmeli
- Yeni yapının çalıştığından emin olunmalı

### CQRS Pattern Implementasyon Sorunları

**Hata:** Generic tip parametreleri ve constraint'ler eksik

**Nedeni:**
1. ICommand ve IQuery interface'leri eksik tanımlanmış
2. Handler'lar için constraint'ler eksik
3. Return tipleri belirsiz

**Çözüm:**
Interface'leri düzelt:
```csharp
public interface ICommand<TResult> { }
public interface ICommand { }
public interface IQuery<TResult> { }

public interface ICommandHandler<in TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
```

**Önemli Notlar:**
- Generic tip parametreleri doğru kullanılmalı
- Constraint'ler eksiksiz tanımlanmalı
- Async/await pattern'i kullanılmalı
- CancellationToken desteği eklenmeli

### Entity İlişkileri Sorunları

**Hata:** Entity ilişkileri düzgün tanımlanmamış

**Nedeni:**
1. Virtual keyword'ü eksik
2. Collection tipleri yanlış
3. Foreign key tanımlamaları eksik

**Çözüm:**
Entity'leri düzelt:
```csharp
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }
}

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public virtual ICollection<User> Users { get; set; }

    public Role()
    {
        Users = new HashSet<User>();
    }
}
```

**Önemli Notlar:**
- Virtual keyword'ü lazy loading için gerekli
- Collection'lar için HashSet<T> tercih edilmeli
- Foreign key'ler nullable olabilir
- Constructor'da collection'lar initialize edilmeli

## Infrastructure Katmanı Hataları

### IAuthService Uygulaması Hatası

**Hata:**
```
'AuthService', 'IAuthService.GenerateJwtToken(UserDto)' arabirim üyesini uygulamaz
```

**Çözüm:**
AuthService sınıfına GenerateJwtToken metodu ekle:
```csharp
public string GenerateJwtToken(UserDto user)
{
    return _tokenGenerator.GenerateToken(new User
    {
        Id = user.Id,
        Username = user.Username,
        IsAdmin = user.IsAdmin,
        RoleId = user.RoleId,
        Role = user.RoleName != null ? new Role { Id = user.RoleId ?? 0, Name = user.RoleName } : null
    });
}
```

### Repository Uygulaması Hataları

**Hata:**
```
'GenericRepository<T>, 'IRepository<T>.AddAsync(T)' arabirim üyesini uygulamıyor. 'Task' eşleşen dönüş türüne sahip olmadığından 'GenericRepository<T>.AddAsync(T)' 'IRepository<T>.AddAsync(T)' öğesini uygulayamaz.
```

**Çözüm:**
GenericRepository sınıfındaki AddAsync metodunun dönüş tipini Task olarak değiştir ve SaveChangesAsync metodunu ekle:
```csharp
public async Task AddAsync(T entity)
{
    await _dbSet.AddAsync(entity);
}

public async Task SaveChangesAsync()
{
    await _context.SaveChangesAsync();
}
```

### UnitOfWork Uygulaması Hataları

**Hata:**
```
'UnitOfWork', 'IUnitOfWork.CommitTransactionAsync()' arabirim üyesini uygulamaz
'UnitOfWork', 'IUnitOfWork.RollbackTransactionAsync()' arabirim üyesini uygulamaz
```

**Çözüm:**
UnitOfWork sınıfına CommitTransactionAsync ve RollbackTransactionAsync metotlarını ekle:
```csharp
public async Task CommitTransactionAsync()
{
    await _context.Database.CommitTransactionAsync();
}

public async Task RollbackTransactionAsync()
{
    await _context.Database.RollbackTransactionAsync();
}
```

### Repository Dönüş Tipi Uyumsuzluğu

**Hata:**
```
'UnitOfWork, 'IUnitOfWork.Users' arabirim üyesini uygulamıyor. 'IUserRepository' eşleşen dönüş türüne sahip olmadığından 'UnitOfWork.Users' 'IUnitOfWork.Users' öğesini uygulayamaz.
```

**Çözüm:**
UnitOfWork sınıfındaki Users ve Roles özelliklerinin tiplerini düzelt:
```csharp
private readonly IUserRepository _users;
private readonly IRoleRepository _roles;

public IUserRepository Users => _users;
public IRoleRepository Roles => _roles;
```

## API Katmanı Hataları

### Paket Sürüm Uyumsuzluğu

**Hata:**
```
System.IdentityModel.Tokens.Jwt paketi için 8.6.0 sürümünden 8.5.0 sürümüne düşürme algılandı.
```

**Çözüm:**
API projesinde System.IdentityModel.Tokens.Jwt paketinin sürümünü yükselt:
```
dotnet add package System.IdentityModel.Tokens.Jwt --version 8.6.0
```

### FluentValidation Entegrasyonu

**Hata:**
```
'IServiceCollection' bir 'AddValidatorsFromAssembly' tanımı içermiyor
```

**Çözüm:**
FluentValidation.DependencyInjection paketi yerine manuel kayıt kullan:
```csharp
var assembly = Assembly.GetExecutingAssembly();
var validatorType = typeof(IValidator<>);
var validatorTypes = assembly.GetTypes()
    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType))
    .ToList();

foreach (var validator in validatorTypes)
{
    var validatorInterface = validator.GetInterfaces()
        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType);
    services.AddTransient(validatorInterface, validator);
}
```

## Null Değer Uyarıları

**Uyarı:**
```
Null atanamaz özellik 'Username', oluşturucudan çıkış yaparken null olmayan bir değer içermelidir.
```

**Çözüm:**
Bu uyarılar, C# 9.0'ın null referans tipleri özelliğinden kaynaklanmaktadır. Çözüm için aşağıdaki yaklaşımlar kullanılabilir:

1. Özellikleri nullable olarak işaretlemek: 
   ```csharp
   public string? Username { get; set; }
   ```

2. Özelliklere varsayılan değer atamak: 
   ```csharp
   public string Username { get; set; } = string.Empty;
   ```

3. Yapıcı metotta değer atamak:
   ```csharp
   public User()
   {
       Username = string.Empty;
       PasswordHash = string.Empty;
   }
   ```

4. [Required] özniteliğini kullanmak:
   ```csharp
   [Required]
   public string Username { get; set; }
   ```

## Backend ve Frontend Başlatma Sorunları

### Backend Başlatma Sorunu

**Hata:**
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> dotnet run
Çalıştırılacak bir proje bulunamadı. C:\Users\muham\OneDrive\Masaüstü\Stock içinde bir projenin bulunduğundan emin olun veya --project kullanarak yolu projeye geçirin.
```

**Nedeni:**
1. Proje ana dizininde çalıştırıldığında dotnet run komutu çalıştırılacak projeyi bulamıyor
2. PowerShell'de && operatörü desteklenmiyor
3. Doğru dizinde olmadan komut çalıştırılıyor

**Çözüm:**
Backend için:
```powershell
cd Stock.API
dotnet run
```

### Frontend Başlatma Sorunları (03.03.2025)

### Hata: Angular CLI Tanınmama Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
ng : The term 'ng' is not recognized as the name of a cmdlet, function, script file, or operable program.
```

**Nedeni:**
1. Angular CLI global olarak yüklü değil
2. PATH değişkeninde Angular CLI yolu tanımlı değil
3. PowerShell'de komut çalıştırma yetkileri kısıtlı olabilir

**Çözüm:**
1. Angular CLI'ı global olarak yüklemek:
```powershell
npm install -g @angular/cli
```

2. Yerel Angular CLI'ı kullanmak:
```powershell
cd frontend
npx ng serve --port=4202
```

3. npm run start komutunu kullanmak:
```powershell
cd frontend
npm run start -- --port=4202
```

**Önemli Notlar:**
- PowerShell'de && operatörü yerine ; kullanılmalıdır
- Port çakışması durumunda farklı bir port belirtilmelidir
- Çalışan node işlemleri durdurulmalıdır
- Komutlar doğru dizinde çalıştırılmalıdır (frontend klasöründe)

### Hata: Port Çakışması Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
Port 4200 is already in use.
Would you like to use a different port? No
An unhandled exception occurred: Port 4200 is already in use. Use '--port' to specify a different port.
```

**Nedeni:**
1. 4200 portu başka bir uygulama tarafından kullanılıyor
2. Önceki Angular uygulaması düzgün kapatılmamış
3. Port serbest bırakılmamış

**Çözüm:**
1. Çalışan tüm node işlemlerini durdurmak:
```powershell
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
```

2. 4200 portunu kullanan işlemleri bulmak ve durdurmak:
```powershell
netstat -ano | findstr :4200
Get-Process -Id <PID> -ErrorAction SilentlyContinue | Stop-Process -Force
```

3. Farklı bir port belirterek uygulamayı başlatmak:
```powershell
cd frontend
npm run start -- --port=4202
```

**Önemli Notlar:**
- Port parametresi doğru şekilde geçirilmelidir: `--port=4202`
- Komut doğru dizinde çalıştırılmalıdır (frontend klasöründe)
- Çalışan işlemler düzgün şekilde durdurulmalıdır
- Uygulama başlatıldıktan sonra http://localhost:4202 adresinden erişilebilir

### Hata: npm Komut Çalıştırma Sorunu
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
npm error code ENOENT
npm error syscall open
npm error path C:\Users\IT\Documents\Cz_Web\Web_Stock\package.json
npm error errno -4058
npm error enoent Could not read package.json: Error: ENOENT
```

**Nedeni:**
1. Komut yanlış dizinde çalıştırılıyor
2. package.json dosyası bulunamıyor
3. npm komutları frontend klasöründe çalıştırılmalı

**Çözüm:**
1. Doğru dizine geçmek:
```powershell
cd frontend
```

2. npm komutlarını frontend klasöründe çalıştırmak:
```powershell
npm run start -- --port=4202
```

**Önemli Notlar:**
- npm komutları package.json dosyasının bulunduğu dizinde çalıştırılmalıdır
- Frontend uygulaması için komutlar frontend klasöründe çalıştırılmalıdır
- Komut çalıştırılmadan önce dizin kontrol edilmelidir

## Genel Öneriler

1. **Arayüz ve Uygulama Geliştirme**
   - Önce arayüzleri tam olarak tanımla, sonra uygulamaları geliştir
   - Arayüz ve uygulama arasındaki uyumu sürekli kontrol et

2. **Null Referans Tipleri**
   - Projenin başında bir strateji belirle ve tutarlı şekilde uygula
   - Nullable referans tiplerini doğru kullan (?, string.Empty, vb.)

3. **Paket Yönetimi**
   - Paket sürümlerini projenin tamamında tutarlı tut
   - Paket bağımlılıklarını düzenli olarak kontrol et
   - Paket çakışmalarını hızlıca çöz

4. **Hata Yönetimi**
   - Hata mesajlarını dikkatlice oku ve adım adım çöz
   - Hataları kategorilere ayır ve sistematik olarak ele al
   - Çözümleri belgelendir ve benzer hataları önle

## Veritabanı Yapılandırma Hataları

### Hata: Veritabanı Bağlantı Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Unable to create a 'DbContext' of type 'ApplicationDbContext'. The exception 'The configuration file 'appsettings.json' was not found and is not optional. The expected physical path was 'C:\Users\muham\OneDrive\Masaüstü\Stock\src\Stock.Infrastructure\appsettings.json'.' was thrown while attempting to create an instance.
```

**Çözüm:**
Migration komutları, appsettings.json dosyasının bulunduğu dizinde çalıştırılmalıdır. Infrastructure dizininde değil, API dizininde çalıştırılmalıdır.

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Hata: PowerShell Komut Çalıştırma Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
The token '&&' is not a valid statement separator in this version.
```

**Çözüm:**
PowerShell'de `&&` operatörü kullanılamaz. Komutlar ayrı ayrı çalıştırılmalıdır.

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Hata: Veritabanı Bağlantı Bilgileri Tutarsızlığı
**Tarih:** 2025-03-02
**Sorun:** 
Projede birden fazla appsettings.json dosyası bulunduğu için, veritabanı bağlantı bilgileri tutarsız olabilir.

**Çözüm:**
Tüm appsettings.json dosyalarındaki veritabanı bağlantı bilgileri güncellenmelidir:
- `Stock.API/appsettings.json`
- `src/Stock.API/appsettings.json`

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=czDb;Username=postgres;Password=19909687"
}
```

## API İstek Hataları

### Hata: API Bağlantı Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Invoke-RestMethod : Uzak sunucuya bağlanılamıyor
```

**Nedeni:**
1. API servisi çalışmıyor olabilir
2. API farklı bir portta çalışıyor olabilir
3. Güvenlik duvarı engellemiş olabilir

**Çözüm:**
1. API servisinin çalıştığından emin olun:
```powershell
cd src/Stock.API
dotnet run
```

2. API'nin doğru portta çalıştığını kontrol edin (varsayılan 5126)
3. Güvenlik duvarı ayarlarını kontrol edin

### Hata: PowerShell Komut Çalıştırma Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
The token '&&' is not a valid statement separator in this version.
```

**Çözüm:**
PowerShell'de `&&` operatörü kullanılamaz. Komutlar ayrı ayrı çalıştırılmalıdır.

```powershell
cd Stock.API
dotnet run
```

### Hata: Kullanıcı Kaydı Hatası
**Tarih:** 2025-03-02
**Sorun:** 
API'ye gönderilen kayıt isteği başarısız olabilir.

**Çözüm:**
1. İstek gövdesinin doğru formatta olduğundan emin olun:
```json
{
  "username": "Admin",
  "password": "Admin123",
  "confirmPassword": "Admin123",
  "isAdmin": true
}
```

2. API'nin çalıştığından emin olun
3. İstek URL'sinin doğru olduğundan emin olun: `http://localhost:5126/api/auth/register`

### API Bağlantı Hataları

### Hata: API Bağlantı Reddi Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
POST http://localhost:5037/api/auth/login 500 (Internal Server Error)
```

**Nedeni:**
1. Backend API'si çalışmıyor veya hatalı çalışıyor olabilir.
2. API'nin doğru portta çalıştığını kontrol edin (5037).
3. API'nin CORS ayarlarının frontend'in çalıştığı porta (4200) izin verdiğinden emin olun.

### Hata: HTTP 0 Unknown Error
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
Http failure response for http://localhost:5126/api/auth/login: 0 Unknown Error
```

**Nedeni:**
Bu hata genellikle tarayıcının API'ye bağlanamadığını gösterir. Nedenleri:
1. API servisi çalışmıyor
2. API farklı bir portta çalışıyor
3. Ağ bağlantısı sorunları
4. CORS politikası engelleri

**Çözüm:**
1. API servisinin çalıştığını doğrulayın
2. API'nin doğru portta çalıştığını kontrol edin
3. Tarayıcı konsolunda ağ sekmesini kontrol edin
4. API'nin CORS yapılandırmasını kontrol edin
5. Güvenlik duvarı ayarlarını kontrol edin
6. Tarayıcıyı yeniden başlatın

**Önemli Notlar:**
- API ve frontend uygulamaları aynı anda çalışıyor olmalıdır
- API'nin 5126 portunda, frontend'in 4200 portunda çalıştığından emin olun
- Tarayıcı konsolunda ağ isteklerini ve hataları kontrol edin
- API'nin CORS yapılandırmasının frontend'in çalıştığı adresi (http://localhost:4200) izin verdiğinden emin olun

### Hata: Swagger Paket Eksikliği Hatası
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(64,18): error CS1061: 'IServiceCollection' bir 'AddSwaggerGen' tanımı içermiyor ve 'IServiceCollection' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'AddSwaggerGen' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(99,9): error CS1061: 'WebApplication' bir 'UseSwagger' tanımı içermiyor ve 'WebApplication' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'UseSwagger' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
C:\Users\muham\OneDrive\Masaüstü\Stock\Stock.API\Program.cs(100,9): error CS1061: 'WebApplication' bir 'UseSwaggerUI' tanımı içermiyor ve 'WebApplication' türünde bir ilk bağımsız değişken kabul eden hiçbir erişilebilir 'UseSwaggerUI' genişletme yöntemi bulunamadı (bir kullanma yönergeniz veya derleme başvurunuz eksik olabilir mi?)
```

**Nedeni:**
1. Swagger NuGet paketleri eksik veya yanlış sürümde
2. Program.cs dosyasında gerekli using direktifleri eksik
3. Proje referansları düzgün ayarlanmamış

**Çözüm:**
1. Gerekli Swagger paketlerini projeye ekleyin:
```powershell
cd Stock.API
dotnet add package Swashbuckle.AspNetCore
```

2. Program.cs dosyasında gerekli using direktiflerinin eklendiğinden emin olun:
```csharp
using Microsoft.OpenApi.Models;
```

3. Program.cs dosyasında Swagger yapılandırmasının doğru şekilde yapıldığından emin olun:
```csharp
// Program.cs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
    
    // JWT için yetkilendirme yapılandırması
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ...

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API v1"));
}
```

4. Paket sürüm çakışmalarını çözün:
```powershell
dotnet restore --force
```

**Önemli Notlar:**
- Swagger paketleri API projesine eklenmelidir, diğer katmanlara değil
- Paket sürüm çakışmaları projenin derlenmesini engelleyebilir
- Program.cs dosyasında Swagger yapılandırması Development ortamında etkinleştirilmelidir
- Swagger UI'a erişmek için `/swagger/index.html` uzantısını kullanın

### Hata: Entity Framework Core Paket Çakışması
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
C:\Program Files\dotnet\sdk\9.0.100-rc.2.24474.11\Microsoft.Common.CurrentVersion.targets(2413,5): warning MSB3277:
  Farklı "Microsoft.EntityFrameworkCore.Relational" sürümleri arasında çözümlenemeyen çakışmalar bulundu.
  "Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" ve "Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" arasında çakışma vardı.
```

**Nedeni:**
1. Projede farklı sürümlerde Entity Framework Core paketleri kullanılıyor
2. Farklı projelerde farklı sürümlerde paketler referans edilmiş
3. Paket sürümleri arasında tutarsızlık var

**Çözüm:**
1. Tüm projelerde aynı sürümde Entity Framework Core paketlerini kullanın:
```powershell
cd Stock.API
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
cd ../src/Stock.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
```

2. Tüm projeleri temizleyip yeniden derleyin:
```powershell
dotnet clean
dotnet build
```

3. Paket referanslarını global.json dosyası ile yönetin:
```json
{
  "msbuild-sdks": {
    "Microsoft.Build.Traversal": "3.2.0"
  }
}
```

**Önemli Notlar:**
- Tüm projelerde aynı sürümde paketler kullanılmalıdır
- Paket sürüm çakışmaları projenin derlenmesini engelleyebilir
- Paket sürümlerini güncellerken tüm bağımlı paketleri de güncellemeyi unutmayın
- Paket sürüm çakışmalarını çözmek için `dotnet restore --force` komutunu kullanabilirsiniz

## Entity Framework Core PendingModelChangesWarning Hatası

### Hata Mesajı
```
An error was generated for warning 'Microsoft.EntityFrameworkCore.Migrations.PendingModelChangesWarning': The model for context 'ApplicationDbContext' changes each time it is built. This is usually caused by dynamic values used in a 'HasData' call (e.g. `new DateTime()`, `Guid.NewGuid()`). Add a new migration and examine its contents to locate the cause, and replace the dynamic call with a static, hardcoded value.
```

### Nedeni
Seed data içinde dinamik değerler kullanmak (örn. `BCrypt.Net.BCrypt.HashPassword("Admin123")`) her seferinde modelin değişmesine neden olur.

### Çözüm
1. Seed data için sabit değerler kullanın:
```csharp
// Dinamik değer (KULLANMAYIN):
PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123")

// Sabit değer (KULLANIN):
PasswordHash = "$2a$11$ij4DH2oHJIgakH3QZS94Uu1/jandJqMjWZtQB8B5.JoK4zrBd2Lhu"
```

2. Yeni bir migrasyon oluşturun ve veritabanını güncelleyin:
```bash
dotnet ef migrations add FixedPasswordHash
dotnet ef database update
```

### Hata: Dashboard Yönlendirme Hatası
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
ERROR RuntimeError: NG04002: Cannot match any routes. URL Segment: 'dashboard'
```

**Nedeni:**
1. Login component'inde `/dashboard` yoluna yönlendirme yapılıyor ancak bu rota tanımlı değil
2. Kullanıcı rolüne göre farklı dashboard'lara yönlendirme yapılması gerekiyor
3. AuthService'den gelen kullanıcı bilgisi doğru kullanılmıyor

**Çözüm:**
1. Login component'inde yönlendirme mantığını güncelle:
```typescript
// Kullanıcı rolüne göre yönlendirme
const user = this.authService.getCurrentUser();
const targetRoute = user?.isAdmin ? '/admin-dashboard' : '/user-dashboard';
console.log('Yönlendiriliyor:', targetRoute);

setTimeout(() => {
  this.router.navigate([targetRoute])
    .then(() => console.log('Yönlendirme başarılı'))
    .catch(err => console.error('Yönlendirme hatası:', err));
}, 1000);
```

2. app.routes.ts dosyasına `/admin/users` rotası eklendi ve navigateToUserManagement metodu güncellendi.

## Kullanıcı Yönetimi Entegrasyonu

### Admin Dashboard'dan Kullanıcı Yönetimine Yönlendirme
- **Sorun**: Admin dashboard'dan Kullanıcı Yönetimi sayfasına yönlendirme yapılırken rota hatası oluşuyordu.
- **Çözüm**: app.routes.ts dosyasına `/admin/users` rotası eklendi ve navigateToUserManagement metodu güncellendi.

### Kullanıcı Yönetimi Sayfasından Geri Dönüş
- **Sorun**: Kullanıcı yönetimi sayfasından admin dashboard'a geri dönüş için bir buton gerekiyordu.
- **Çözüm**: Kullanıcı yönetimi bileşenine geri dönüş butonu eklendi ve goBack metodu oluşturuldu.
  ```typescript
  goBack() {
    this.router.navigate(['/admin-dashboard']);
  }
  ```

### Kullanıcı Yönetimi Bileşeni Entegrasyonu
- **Sorun**: Mevcut UserManagementComponent'in admin dashboard ile entegrasyonu gerekiyordu.
- **Çözüm**: Admin dashboard içerisinde Kullanıcı Yönetimi bölümüne erişim sağlandı ve ilgili rota yapılandırması yapıldı.

# Login Hataları

## BCrypt Şifre Doğrulama Hatası (03.03.2025)

**Hata:** 
```
401 Unauthorized - Login işleminde şifre doğrulama hatası
```

**Nedeni:**
1. BCrypt hash'i ile şifre doğrulama işlemi başarısız
2. Veritabanındaki hash değeri ile girilen şifrenin hash'i uyuşmuyor

**Çözüm:**
1. `FixPasswordController` endpoint'i kullanılarak admin şifresi yeniden ayarlandı:
```
GET /api/FixPassword/fix-admin
```

2. Yeni hash değeri:
```
$2a$11$jLGT8mYtwgJ/U6VpQPhxFuLfnVAMm7qLsnfim33OehCyeieqVms8q
```

**Önemli Notlar:**
- BCrypt work factor: 11
- Hash uzunluğu: 60 karakter
- Hash formatı: BCrypt ($2a$ prefix)
- Şifre: Admin123

**Log Kayıtları:**
```
info: Stock.Infrastructure.Services.AuthService[0]
      Kullanıcı bulundu: admin, ID: 1, IsAdmin: True
info: Stock.Infrastructure.Services.PasswordHasher[0]
      Şifre doğrulama sonucu: True
info: Stock.Infrastructure.Services.AuthService[0]
      Başarılı giriş: admin, Token üretildi
```

### Şifre Değiştirme Endpoint Hatası (04.03.2025)

**Hata:**
```
POST http://localhost:5037/api/auth/change-password 404 (Not Found)
```

**Nedeni:**
1. Clean Architecture geçişi sırasında şifre değiştirme endpoint'i uygulanmamış
2. IAuthService arayüzünde şifre değiştirme metodu eksik
3. AuthController'da şifre değiştirme endpoint'i eksik
4. ChangePasswordDto sınıfı oluşturulmamış

**Çözüm:**
1. IAuthService arayüzüne ChangePasswordAsync metodu ekle:
```csharp
Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId);
```

2. ChangePasswordDto sınıfını oluştur:
```csharp
public class ChangePasswordDto
{
    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır")]
    public string NewPassword { get; set; } = string.Empty;
}
```

3. AuthResponseDto sınıfına Message özelliği ekle:
```csharp
public string? Message { get; set; }
```

4. AuthService sınıfına ChangePasswordAsync metodunu uygula:
```csharp
public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
{
    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var user = await _unitOfWork.Users.GetByIdAsync(userId);
    if (user == null)
    {
        _logger.LogWarning($"Kullanıcı bulunamadı - ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Kullanıcı bulunamadı"
        };
    }

    bool isCurrentPasswordValid = _passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash);
    _logger.LogInformation($"Mevcut şifre doğrulama sonucu: {isCurrentPasswordValid}");

    if (!isCurrentPasswordValid)
    {
        _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Mevcut şifre hatalı"
        };
    }

    try
    {
        user.PasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = true,
            Message = "Şifre başarıyla değiştirildi"
        };
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifre değiştirme hatası: {ex.Message}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = $"Şifre değiştirme hatası: {ex.Message}"
        };
    }
}
```

5. AuthController'a şifre değiştirme endpoint'ini ekle:
```csharp
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
{
    _logger.LogInformation("Şifre değiştirme isteği başlatılıyor...");
    
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
        _logger.LogWarning("Token'da kullanıcı ID'si bulunamadı veya geçersiz");
        return Unauthorized(new { message = "Geçersiz kullanıcı kimliği" });
    }

    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var result = await _authService.ChangePasswordAsync(changePasswordDto, userId);
    
    if (!result.Success)
    {
        _logger.LogWarning($"Şifre değiştirme başarısız - Kullanıcı ID: {userId}, Hata: {result.ErrorMessage}");
        return BadRequest(result);
    }
    
    _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
    return Ok(result);
}
```

**Önemli Notlar:**
- Şifre değiştirme işlemi için her zaman mevcut şifre doğrulaması yapılmalıdır
- Yeni şifre ve şifre tekrarı alanları eşleşmelidir
- Şifre değiştirme işlemi başarılı olduğunda kullanıcıya bildirim gösterilmelidir
- Hata durumlarında kullanıcıya anlamlı hata mesajları gösterilmelidir

## Şifre Değiştirme Formu Güncellemesi

### Şifre Değiştirme Formu Tasarım Güncellemesi

**Yapılan Değişiklikler:**
1. Şifre değiştirme formunun arka plan rengi ve stilini güncelledik
2. Input alanlarını daha modern bir görünüme kavuşturduk
3. Şifre göster/gizle işlevselliği ekledik
4. Butonların tasarımını iyileştirdik

**Olası Hatalar ve Çözümleri:**

1. **Şifre göster/gizle işlevselliği çalışmıyor**
   - **Neden:** Angular'da event binding sorunları olabilir
   - **Çözüm:** HTML'de (click) event binding'in doğru şekilde uygulandığından emin olun

2. **Şifre değiştirme API çağrısı başarısız oluyor**
   - **Neden:** Backend API endpoint'i değişmiş olabilir veya token doğrulama sorunu olabilir
   - **Çözüm:** Network isteklerini kontrol edin, API endpoint'in doğru olduğundan emin olun ve token'ın geçerli olduğunu doğrulayın

3. **Form stillerinde bozulmalar**
   - **Neden:** CSS override sorunları veya PrimeNG bileşenleriyle çakışmalar
   - **Çözüm:** CSS seçicilerinin özgüllüğünü artırın veya !important kullanın

4. **Şifre değiştirme işlemi sonrası form kapanmıyor**
   - **Neden:** togglePasswordChange() metodu çağrılmamış olabilir
   - **Çözüm:** API çağrısı başarılı olduğunda togglePasswordChange() metodunun çağrıldığından emin olun

**Genel Notlar:**
- Şifre değiştirme işlemi için her zaman mevcut şifre doğrulaması yapılmalıdır
- Yeni şifre ve şifre tekrarı alanları eşleşmelidir
- Şifre değiştirme işlemi başarılı olduğunda kullanıcıya bildirim gösterilmelidir
- Hata durumlarında kullanıcıya anlamlı hata mesajları gösterilmelidir

## Kullanıcı Silme İşlemi Hataları

### Hata 1: DeleteUserCommandHandler'da Remove Metodu Hatası

**Tarih:** 2025-03-08

**Hata Mesajı:** DeleteUserCommandHandler.cs dosyasında derleme hatası: Remove metodu için gerekli argüman eksik.

**Nedeni:** DeleteUserCommandHandler içinde `_unitOfWork.Users.Remove(user)` şeklinde bir metot çağrısı yapılıyor.

**Çözüm:**
```csharp
// Hatalı kod
_unitOfWork.Users.Remove(user);

// Düzeltilmiş kod
await _unitOfWork.Users.DeleteAsync(user);
```

**Önemli Notlar:**
- Repository arayüzlerinde metot isimleri tutarlı olmalı
- Asenkron metotlar için await kullanılmalı
- Metot isimleri ve imzaları arayüz ile uyumlu olmalı

### Hata 2: API Endpoint Tutarsızlıkları ve Karışıklık

**Tarih:** 2025-03-08

**Sorun:** Hem UserController (tekil) hem de UsersController (çoğul) olması, API endpoint'lerinde tutarsızlıklara ve frontend uygulamasında karışıklığa neden oldu.

**Nedeni:**
1. Clean Architecture'a geçiş sırasında eski yapı tamamen kaldırılmamış
2. Frontend hala eski endpoint'i referans alıyordu
3. Tekil (User) ve çoğul (Users) controller'lar arasında karışıklık vardı

**Çözüm:**
1. Eski proje dosyalarını yedekleyip kaldırdık:
   - Stock.API
   - Stock.Infrastructure
   - UserController.cs
2. Yeni UsersController.cs dosyasında Delete metodu oluşturduk
3. Frontend'deki UserService içindeki endpoint'leri düzelttik:
   ```typescript
   // Hatalı
   return this.http.delete<void>(`${this.apiUrl}/User/${id}`);
   
   // Düzeltilmiş
   return this.http.delete<void>(`${this.apiUrl}/Users/${id}`);
   ```

**Önemli Notlar:**
- Clean Architecture'da çoğul isimlendirme tercih edilmeli (UsersController)
- Frontend ve backend arasındaki API endpoint'leri tutarlı olmalı
- Yapısal değişiklikler sırasında eski kod tamamen kaldırılmalı veya açıkça işaretlenmeli

### Hata 3: Çoklu Command Sınıfları Çakışması

**Tarih:** 2025-03-08

**Hata Mesajı:** CS0104: 'DeleteUserCommand' adı iki farklı ad alanında bulundu.

**Nedeni:** Projede iki farklı namespace'te DeleteUserCommand sınıfı tanımlanmıştı:
1. `Stock.Application.Features.Users.Commands.DeleteUserCommand`
2. `Stock.Application.Features.Users.Commands.DeleteUser.DeleteUserCommand`

Aynı şekilde, CreateUserCommand ve UpdateUserCommand için de benzer çakışmalar vardı.

**Çözüm:**
UsersController.cs dosyasında tam nitelikli tip adları kullanıldı:

```csharp
// Hatalı kod
using Stock.Application.Features.Users.Commands.DeleteUser;

// Düzeltilmiş kod
using Stock.Application.Features.Users.Commands;
```

**Önemli Notlar:**
- Namespace'lerde tutarlı bir yapı kullanılmalı
- Aynı isimli sınıflar farklı namespace'lerde olmamalı
- Çakışma durumunda tam nitelikli tip adları kullanılabilir
- Uzun vadede, proje yapısı düzenlenerek çakışmalar giderilmeli

## Kullanıcı Güncelleme Hatası

**Hata:**
```
System.InvalidOperationException: No service for type 'MediatR.IRequestHandler`2[Stock.Application.Features.Users.Commands.UpdateUserCommand,Stock.Application.Models.DTOs.UserDto]' has been registered.
```

**Nedeni:**
1. UpdateUserCommand için bir handler sınıfı (UpdateUserCommandHandler) oluşturulmamış
2. MediatR, UpdateUserCommand için bir handler bulamıyor

**Çözüm:**
1. UpdateUserCommandHandler sınıfını oluştur:
```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user == null)
                return null;

            user.Username = request.Username;
            user.IsAdmin = request.IsAdmin;
            user.RoleId = request.RoleId;
            user.Sicil = request.Sicil;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            return _mapper.Map<UserDto>(updatedUser);
        }
    }
}
```

2. Ayrıca CreateUserCommandHandler sınıfını da oluştur (eksikse):
```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Username = command.Username,
                PasswordHash = _passwordHasher.HashPassword(command.Password),
                IsAdmin = command.IsAdmin,
                RoleId = command.RoleId,
                Sicil = command.Sicil
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }
    }
}
```

3. UpdateUserCommand'e sicil alanını ekledik:
```csharp
public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public string Sicil { get; set; } = string.Empty;
}
```

4. User entity'sine sicil alanını ekle:
```csharp
[StringLength(50)]
public string Sicil { get; set; } = string.Empty;
```

5. UserDto'ya sicil alanını ekledik:
```csharp
public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string Sicil { get; set; } = string.Empty;
}
```

6. Veritabanı şemasını güncellemek için migration oluşturduk ve uyguladık:
```bash
dotnet ef migrations add AddSicilFieldToUser
dotnet ef database update
```

### Clean Architecture Perspektifinden Değerlendirme
Bu hata, Clean Architecture'ın temel prensiplerinden biri olan "iç katmanlar dış katmanlara bağımlı olmamalıdır" ilkesini vurgulamaktadır. Frontend (dış katman) tarafından beklenen bir alan (sicil), Domain ve Application katmanlarında (iç katmanlar) tanımlanmamıştı.

Çözüm süreci, Clean Architecture'ın aşağıdaki katmanlarında değişiklik gerektirdi:
- **Domain Layer**: User entity'sinin güncellenmesi
- **Application Layer**: Command ve DTO modellerinin güncellenmesi
- **Infrastructure Layer**: Veritabanı şemasının güncellenmesi
- **API Layer**: Controller'ın düzgün yapılandırılması

Bu çözüm, tüm katmanların uyumlu çalışmasını sağlayarak Clean Architecture prensiplerini korumaktadır.

### Öğrenilen Dersler
1. Veritabanı kısıtlamaları eklemeden önce mevcut verilerin bu kısıtlamalara uygun olup olmadığı kontrol edilmelidir.
2. Mevcut veriler kısıtlamalara uymuyorsa, önce veri temizliği yapılmalı veya uygulama seviyesinde kontrollerle ilerlenmelidir.
3. Hata durumlarını kullanıcıya net ve anlaşılır mesajlarla iletmek, kullanıcı memnuniyetini artırır.
4. Farklı katmanlar arasında hata mesajlarının iletimi, iyi tasarlanmış bir API sözleşmesi ile sağlanmalıdır.
5. Frontend uygulamasında API hata mesajlarını doğrudan göstermek, sorunların hızlı teşhisine olanak tanır.
6. Loglama stratejisi, hem geliştirme hem de canlı ortamlarda sorunların tespit edilmesine yardımcı olur.

## Kullanıcı Ekleme Hatası (500 Internal Server Error)

**Hata:** Kullanıcı ekleme işlemi sırasında 500 Internal Server Error alınıyor.

**Nedeni:**
1. Frontend'den gönderilen istekte `passwordHash` parametresi kullanılıyordu, ancak backend `password` parametresi bekliyordu.
2. `userToCreate` nesnesi frontend'de `passwordHash` alanına sahipti, backend'de ise bu alan `password` olarak tanımlanmıştı.
3. `CreateUserCommand` sınıfında `password` alanı varken, frontend'den `passwordHash` gönderiliyordu.

**Çözüm:**
1. `user-management.component.ts` dosyasında `passwordHash` alanı `password` olarak değiştirildi.
2. `user.service.ts` dosyasında `createUser` metodu değiştirildi:
```typescript
password: user.password || user.passwordHash || '', // Önce password, yoksa passwordHash kullan
```

3. Form değerleri konsola yazdırılarak hata ayıklama yapıldı.

**Önemli Notlar:**
- Frontend ve backend arasındaki model uyumsuzlukları API hatalarına neden olabilir
- Veri modeli değişikliklerinde hem frontend hem de backend güncellenmelidir
- Şifre gibi hassas bilgilerin üretim ortamında loglanmasından kaçınılmalıdır
- HTTP durum kodları (500, 400 vb.) sorunların kaynağını belirlemeye yardımcı olur

## Sicil Alanı Benzersizlik Kontrolü

**Hata:** Sicil alanı için benzersizlik kontrolü uygulanırken veritabanı güncelleme hatası alındı.

**Nedeni:**
1. Entity Framework Core migrasyonu, sicil alanı için veritabanında benzersiz indeks oluşturmaya çalışıyordu.
2. Veritabanında zaten aynı sicil numarasına sahip kullanıcılar olduğu için bu işlem başarısız oldu.
3. Bu durum, "The model for context 'ApplicationDbContext' has pending changes. Add a new migration before updating the database." hatası ile sonuçlandı.

**Çözüm:**
1. `src/Stock.Infrastructure/Data/Configurations/UserConfiguration.cs` dosyasından `Sicil` alanı için tanımlanmış benzersizlik indeksi kaldırıldı:
   ```csharp
   // Kaldırılan kod
   builder.HasIndex(u => u.Sicil)
       .IsUnique();
   ```

2. Yeni bir "RemoveSicilUniquenessConstraint" migrasyonu oluşturuldu.
3. Veritabanı güncellendi.
4. Benzersizlik kontrolü, uygulama seviyesinde (Command Handler sınıflarında) uygulandı:
   ```csharp
   // Eklenen veya Güncellenen kullanıcının sicil numarası kontrolü
   var existingUsers = await _unitOfWork.Users.FindAsync(u => u.Sicil == request.Sicil && u.Id != request.Id);
   if (existingUsers.Any())
   {
       throw new InvalidOperationException($"'{request.Sicil}' sicil numarası zaten başka bir kullanıcı tarafından kullanılmaktadır. Her kullanıcının benzersiz bir sicil numarası olmalıdır.");
   }
   ```

5. Controller sınıflarında hata yönetimi iyileştirildi:
   ```csharp
   catch (InvalidOperationException ex)
   {
       // Sicil benzersizlik hatası veya diğer doğrulama hataları
       string errorMessage = ex.Message;
       _logger.LogWarning("Kullanıcı işlemi sırasında doğrulama hatası: {Message}", errorMessage);
       
       // Özel olarak sicil numarası hatası için kontrol et
       if (errorMessage.Contains("sicil numarası zaten"))
       {
           return BadRequest(new { error = errorMessage });
       }
       
       return BadRequest(new { error = errorMessage });
   }
   ```

6. Frontend bileşenlerinde API'dan dönen hata mesajları doğrudan kullanıcıya gösterildi:
   ```typescript
   error: (error) => {
     console.error('Kullanıcı ekleme hatası:', error);
     
     // API'dan dönen hata mesajını göster (varsa)
     const errorMessage = error.error?.error || 'Kullanıcı eklenirken bir hata oluştu.';
     
     this.messageService.add({
       severity: 'error',
       summary: 'Hata',
       detail: errorMessage,
       life: 5000
     });
   }
   ```

**Kullanıcı Deneyimi İyileştirmeleri:**
1. Hata mesajları daha açıklayıcı ve kullanıcı dostu hale getirildi.
2. Spesifik hata durumlarına göre özelleştirilmiş mesajlar sunuldu.
3. Sicil benzersizlik hatası için özel kontrol eklenip kullanıcıya net bir mesaj gösterildi.
4. Hata mesajları daha uzun süre ekranda tutuldu (5 saniye).
5. Loglama iyileştirildi ve hata ayıklama bilgileri zenginleştirildi.

**Öğrenilen Dersler:**
1. Veritabanı kısıtlamaları eklemeden önce mevcut verilerin bu kısıtlamalara uygun olup olmadığı kontrol edilmelidir.
2. Mevcut veriler kısıtlamalara uymuyorsa, önce veri temizliği yapılmalı veya uygulama seviyesinde kontrollerle ilerlenmelidir.
3. Hata durumlarını kullanıcıya net ve anlaşılır mesajlarla iletmek, kullanıcı memnuniyetini artırır.
4. Farklı katmanlar arasında hata mesajlarının iletimi, iyi tasarlanmış bir API sözleşmesi ile sağlanmalıdır.
5. Frontend uygulamasında API hata mesajlarını doğrudan göstermek, sorunların hızlı teşhisine olanak tanır.
6. Loglama stratejisi, hem geliştirme hem de canlı ortamlarda sorunların tespit edilmesine yardımcı olur.

## GitHub Senkronizasyonu ve Kullanıcı Şifre/Sicil Sorunları

**Hata:** GitHub'dan son değişiklikleri çektikten sonra kullanıcı girişi yapılamıyor. Veritabanında sadece admin kullanıcısı kalıyor ve sicil alanı boş oluyor.

**Tarih:** 2025-03-05

**Nedeni:**
1. Son migration'lar veritabanında kritik değişiklikler yapıyor:
   - Normal kullanıcı (user) siliniyor
   - User rolü siliniyor
   - Admin kullanıcısının şifresi değişiyor ve sicil alanı boşaltılıyor
2. SeedData mekanizması, veritabanında zaten bir admin kullanıcısı olduğu için çalışmıyor ve yeni kullanıcıları eklemiyor.
3. Farklı ortamlarda (ev ve iş) aynı GitHub repo kullanıldığında, her ortamda migration'lar farklı durumda olabiliyor.

**İncelenen Migration'lar:**
```sql
-- 20250305173319_PendingChanges migration'ından
DELETE FROM "Users" WHERE "Id" = 2;
DELETE FROM "Roles" WHERE "Id" = 2;
UPDATE "Users" SET "PasswordHash" = '...', "Sicil" = '' WHERE "Id" = 1;
```

**Çözüm:**
1. FixPasswordController adında yeni bir controller oluşturuldu:
   ```csharp
   [HttpGet("fix-passwords")]
   public async Task<IActionResult> FixPasswords()
   {
       // Admin kullanıcısının şifresini ve sicil alanını düzelt
       var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
       if (adminUser != null)
       {
           adminUser.PasswordHash = _passwordHasher.HashPassword("admin123");
           adminUser.Sicil = "A001";
           await _context.SaveChangesAsync();
       }
       
       // Normal kullanıcı yoksa oluştur veya güncelle
       var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
       if (normalUser == null)
       {
           // User kullanıcısı yoksa oluştur
           var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
           if (userRole == null)
           {
               // User rolü yoksa oluştur
               userRole = new Role { Name = "User", Description = "Normal kullanıcı" };
               await _context.Roles.AddAsync(userRole);
               await _context.SaveChangesAsync();
           }
           
           var newUser = new User
           {
               Username = "user",
               PasswordHash = _passwordHasher.HashPassword("user123"),
               IsAdmin = false,
               RoleId = userRole.Id,
               Sicil = "U001"
           };
           
           await _context.Users.AddAsync(newUser);
           await _context.SaveChangesAsync();
       }
       else
       {
           normalUser.PasswordHash = _passwordHasher.HashPassword("user123");
           normalUser.Sicil = "U001";
           await _context.SaveChangesAsync();
       }
   }
   ```

2. SeedData mekanizması güncellendi, artık kullanıcılar varsa da güncelleniyor:
   ```csharp
   private static async Task SeedUsersAsync(ApplicationDbContext context, IServiceProvider services)
   {
       var passwordHasher = services.GetRequiredService<IPasswordHasher>();
       
       // Admin kullanıcısını kontrol et ve güncelle
       var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
       if (adminUser == null)
       {
           // Admin kullanıcısı yoksa oluştur
           adminUser = new User
           {
               Username = "admin",
               PasswordHash = passwordHasher.HashPassword("admin123"),
               IsAdmin = true,
               Sicil = "A001"
           };
           
           await context.Users.AddAsync(adminUser);
       }
       else
       {
           // Admin kullanıcısı varsa kontrol et ve güncelle
           if (string.IsNullOrEmpty(adminUser.Sicil))
           {
               adminUser.Sicil = "A001";
           }
           
           adminUser.PasswordHash = passwordHasher.HashPassword("admin123");
           context.Users.Update(adminUser);
       }
       
       // Normal kullanıcı için benzer kontroller ve güncellemeler...
   }
   ```

**Kullanımı:**
GitHub'dan son değişiklikleri çektikten sonra, eğer kullanıcı girişi yapılamıyorsa:
1. API uygulamasını başlatın
2. `/api/FixPassword/fix-passwords` endpoint'ine istek atın
3. Bu işlem hem admin hem de normal kullanıcı hesaplarını düzeltecek

**Öğrenilen Dersler:**
1. Veritabanı şemasında kritik değişiklikler yapan migration'lar dikkatli planlanmalı
2. Seed data mekanizmaları mevcut verilerin kontrolünü ve gerektiğinde güncellemesini içermeli
3. Farklı ortamlar arasında geçiş yapıldığında veritabanı durumunu senkronize tutmak için özel mekanizmalar gerekebilir
4. Critical fix endpoint'leri gibi acil durum onarım mekanizmaları oluşturulmalı
5. Migration'ların içeriğini commit etmeden önce olası etkileri değerlendirilmeli
6. Farklı ortamlar arasındaki senkronizasyon sorunları için dokümantasyon sağlanmalı

**Önleyici Önlemler:**
1. Migration'lar, mevcut verileri silmek yerine güncellemeyi tercih etmeli
2. Seed data, var olan verilerin kontrolünü ve gerektiğinde güncellemesini içermeli
3. Kritik veritabanı değişiklikleri için rollback stratejisi planlanmalı
4. Önemli alanlar için varsayılan değerler atanmalı
5. Farklı ortamlar arasında geçiş rehberi oluşturulmalı

## Rol Terminolojisi Değişikliği

**Hata:** Kullanıcı yönetimi arayüzünde "Katkıda Bulunan" terimi kullanılıyordu, ancak bu terim proje genelindeki rol terminolojisine uygun değildi. Ayrıca, sistemde "İzleyici" rolü bulunmasına rağmen bu rol aktif olarak kullanılmıyordu.

**Nedeni:**
- Kullanıcı izinleri tablosunda "Contributor" teknik değeri için "Katkıda Bulunan" çevirisi kullanılmıştı.
- Projenin standardizasyonu için tüm arayüzlerde "Yönetici" ve "Kullanıcı" şeklinde iki ana rol kullanılması gerekiyor.
- "İzleyici" rolü kullanılmadığı halde kullanıcı arayüzünde görüntüleniyordu, bu da kullanıcıların kafasını karıştırabilirdi.

**Çözüm:**
1. İlk güncelleme: "Katkıda Bulunan" ifadesi "Kullanıcı" olarak güncellendi
   ```diff
   {{ user.permissions === 'Admin' ? 'Yönetici' : 
   -  user.permissions === 'Contributor' ? 'Katkıda Bulunan' : 
   +  user.permissions === 'Contributor' ? 'Kullanıcı' : 
      user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
   ```

2. İkinci güncelleme: "İzleyici" rolü tamamen kaldırıldı
   ```diff
   roleFilterOptions: any[] = [
     { label: 'Tümü', value: null },
     { label: 'Yönetici', value: 'Admin' },
   -  { label: 'Kullanıcı', value: 'Contributor' },
   -  { label: 'İzleyici', value: 'Viewer' }
   +  { label: 'Kullanıcı', value: 'Contributor' }
   ];
   ```

3. Kullanıcı izinleri görüntüleme kısmında "İzleyici" kontrolü kaldırıldı
   ```diff
   <span class="permission-badge" [ngClass]="{
     'admin-badge': user.permissions === 'Admin',
   -  'contributor-badge': user.permissions === 'Contributor',
   -  'viewer-badge': user.permissions === 'Viewer'
   +  'contributor-badge': user.permissions === 'Contributor'
   }">
     {{ user.permissions === 'Admin' ? 'Yönetici' : 
        user.permissions === 'Contributor' ? 'Kullanıcı' : 
   -    user.permissions === 'Viewer' ? 'İzleyici' : user.permissions }}
   +    user.permissions }}
   </span>
   ```

**Önemli Notlar:**
- Teknik değerler (arka planda kullanılan değerler) değiştirilmedi, sadece kullanıcıya gösterilen etiketler güncellendi.
- Rol tabanlı erişim kontrol sistemi bu değişiklikten etkilenmedi.
- Sistem, artık sadece "Yönetici" ve "Kullanıcı" rollerini destekliyor.
- İlgili detaylı bilgi için bkz: `knowledge-base/user_role_terminology_update.md`

## Sayfa Erişim İzinleri Geliştirmesi

### Hata 1: PrimeNG Bileşenleri Görüntülenemedi
**Sorun:** Rol detay sayfasında p-accordion ve p-toggleButton bileşenleri görüntülenemedi.
**Çözüm:** Standalone bileşenlerde gerekli PrimeNG modüllerinin imports dizisine eklenmesi gerekiyor:
```typescript
@Component({
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AccordionModule,
    ToggleButtonModule,
    CheckboxModule,
    ButtonModule,
    ProgressSpinnerModule
  ]
})
```

### Hata 2: İzin Gruplarına Erişilemedi
**Sorun:** Şablonda belirli gruplar için izinlere erişirken hata alındı.
**Çözüm:** Permission servisinden izinleri grup bazlı döndürmek için yardımcı metotlar eklendi:
```typescript
getPermissionsByGroup(groupName: string): Permission[] {
  const group = this.permissionGroups.find(g => g.group === groupName);
  return group ? group.permissions : [];
}
```

### Hata 3: İzinlerin Kaydedilmesi Sırasında 401 Hatası
**Sorun:** İzin güncelleme işlemi yapıldığında 401 Unauthorized hatası alındı.
**Çözüm:** Kimlik doğrulama token'ının HTTP isteklerinde gönderildiğinden emin olmak ve API izin kontrolünü düzeltmek:
```typescript
// HTTP isteğine token ekleme
const headers = {
  'Authorization': `Bearer ${this.authService.getToken()}`
};
return this.http.post<boolean>(url, { permissionIds }, { headers });
```

### Hata 4: ToggleButton [checked] ve [value] Özelliği Hatası

**Sorun:** Rol detay sayfasında p-toggleButton bileşenleri için şu hatalar alındı:
```
Can't bind to 'checked' since it isn't a known property of 'p-toggleButton'.
Can't bind to 'value' since it isn't a known property of 'p-toggleButton'.
```

**Nedeni:**
1. ToggleButton bileşeni, standart HTML input bileşenleri gibi `checked` ve `value` özelliklerini desteklemiyor.
2. PrimeNG ToggleButton, bir dizi içinde değer varlığı kontrolü için doğrudan kullanılamıyor.

**Çözüm:**
1. `[checked]` ve `[value]` özellikleri yerine tek yönlü veri bağlama kullan:
```html
<!-- Hatalı kod -->
<p-toggleButton 
    [checked]="isPermissionSelected(permission.id)" 
    [(ngModel)]="selectedPermissions"
    [value]="permission.id">
</p-toggleButton>

<!-- Doğru kod -->
<p-toggleButton 
    [ngModel]="isPermissionSelected(permission.id)"
    (onChange)="togglePermission(permission.id)">
</p-toggleButton>
```

2. isPermissionSelected metodu, izin ID'sinin seçili izinler listesinde olup olmadığını kontrol eder:
```typescript
isPermissionSelected(permissionId: number): boolean {
  return this.selectedPermissions.includes(permissionId);
}
```

3. togglePermission metodu, izin ID'sini izin listesinden ekler veya çıkarır:
```typescript
togglePermission(permissionId: number): void {
  const index = this.selectedPermissions.indexOf(permissionId);
  if (index > -1) {
    this.selectedPermissions.splice(index, 1);
  } else {
    this.selectedPermissions.push(permissionId);
  }
}
```

**Öğrenilen Dersler:**
- PrimeNG bileşenleri, standart HTML bileşenlerinden farklı şekilde çalışır ve farklı özellikler kullanır.
- ToggleButton için değer listesi bağlama doğrudan desteklenmez, özel bir mantıkla yönetilmelidir.
- Bileşenlerde sorun yaşandığında, PrimeNG dokümantasyonunu kontrol etmek faydalıdır.

### Hata 5: Admin/Roles Rota Hatası

**Sorun:** Admin dashboard'dan rol yönetimi sayfasına gitmeye çalışıldığında aşağıdaki hata alınıyor:
```
ERROR RuntimeError: NG04002: Cannot match any routes. URL Segment: 'admin/roles'
```

**Nedeni:**
1. Admin dashboard bileşeninde `navigateToRoleManagement()` metodu `/admin/roles` rotasına yönlendiriyor
2. Ancak app.routes.ts dosyasında `admin/roles` rotası tanımlanmamış
3. Sadece `roles/:id` ve `role-management` rotaları tanımlanmış

**Çözüm:**
1. app.routes.ts dosyasına eksik olan `admin/roles` rotasını ekle:
```typescript
{ 
  path: 'admin/roles', 
  component: RoleManagementComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.RoleManagement'
  }
}
```

2. Ayrıca tutarlılık için `roles` rotasını da ekle:
```typescript
{ 
  path: 'roles', 
  component: RoleManagementComponent,
  canActivate: [AuthGuard],
  data: { 
    requiresAdmin: true,
    requiredPermission: 'Pages.RoleManagement'
  }
}
```

**Öğrenilen Dersler:**
- Bir sayfaya yönlendirme yapmadan önce, hedef rotanın app.routes.ts dosyasında tanımlanmış olduğundan emin olunmalı
- Admin dashboard gibi merkezi bir komponenten yapılan yönlendirmeler kontrol edilmeli
- Angular'da rotaların tutarlı bir şekilde tanımlanması önemli
- URL yolları ve rota tanımları arasında uyumsuzluk olmamalı

### Hata 6: API Endpoint Uyumsuzluğu (Rol Yönetimi)

**Sorun:** Rol yönetimi sayfasında roller yüklenirken aşağıdaki hata alınıyor:
```
GET http://localhost:5037/api/roles 404 (Not Found)
```

**Nedeni:**
1. Frontend'de RoleService, `api/roles` endpoint'ine istek yapıyor
2. Backend'de controller `api/Role` endpoint'ini sunuyor (controller adı RoleController)
3. ASP.NET Core'da controller adı rota olarak kullanılıyor ve büyük/küçük harf duyarlı

**Çözüm:**
Frontend'deki RoleService'de API URL'sini değiştir:
```typescript
// Hatalı
private apiUrl = `${environment.apiUrl}/api/roles`;

// Doğru
private apiUrl = `${environment.apiUrl}/api/Role`;
```

**Alternatif Çözüm:**
Backend'de controller adını değiştirmek:
```csharp
// Hatalı
public class RoleController : ControllerBase

// Doğru
public class RolesController : ControllerBase
```

**Öğrenilen Dersler:**
- API endpoint'leri frontend ve backend arasında tam olarak eşleşmeli
- ASP.NET Core'da controller adları rota olarak kullanılır ve büyük/küçük harf duyarlıdır
- Frontend ve backend arasındaki API sözleşmesi tutarlı olmalı
- API endpoint'lerini test etmek için Swagger veya Postman kullanılabilir

### Hata 7: PrimeNG Table Responsive Özelliği Uyarısı

**Sorun:** Rol yönetimi sayfasında PrimeNG Table bileşeni için aşağıdaki uyarı alınıyor:
```
responsive property is deprecated as table is always responsive with scrollable behavior.
```

**Nedeni:**
PrimeNG'nin güncel sürümlerinde Table bileşeni her zaman responsive olarak çalışıyor ve `responsive` özelliği kullanımdan kaldırılmış.

**Çözüm:**
Table bileşeninden `[responsive]="true"` özelliğini kaldır:
```html
<!-- Hatalı -->
<p-table [value]="roles" [paginator]="true" [rows]="10" [responsive]="true" [loading]="loading">

<!-- Doğru -->
<p-table [value]="roles" [paginator]="true" [rows]="10" [loading]="loading">
```

**Öğrenilen Dersler:**
- Kütüphane güncellemeleriyle bazı özellikler kullanımdan kaldırılabilir
- Konsol uyarılarını düzenli olarak kontrol etmek ve çözmek önemli
- PrimeNG gibi UI kütüphanelerinin dokümantasyonunu takip etmek gerekli

### Hata 8: 500 Internal Server Error (Rol Yönetimi) - Döngüsel Referans

**Sorun:** Rol yönetimi sayfasında roller yüklenirken aşağıdaki hata alınıyor:
```
GET http://localhost:5037/api/Role 500 (Internal Server Error)

System.Text.Json.JsonException: A possible object cycle was detected. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32. Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
```

**Nedeni:**
1. RoleController'da `Include(r => r.Users)` kullanılması döngüsel referans sorununa neden oluyor
2. JSON serialization sırasında Role -> Users -> Role -> Users -> ... şeklinde sonsuz döngü oluşuyor
3. Döngüsel referanslar için uygun JSON serialization ayarları kullanılmamış
4. Entity Framework ile ilişkisel verileri döndürürken projection kullanılmamış

**Çözüm 1: RoleController'da döngüsel referans sorununu önlemek için Select ile projection kullanımı:**
```csharp
// Hatalı
return await _context.Roles.Include(r => r.Users).ToListAsync();

// Doğru
var roles = await _context.Roles
    .AsNoTracking() // Performans için
    .Select(r => new 
    {
        r.Id,
        r.Name,
        r.Description,
        r.CreatedAt,
        r.UpdatedAt,
        UserCount = r.Users.Count,
        // İlişkili kullanıcıları Role referansı olmadan seçici şekilde al
        Users = r.Users.Select(u => new { u.Id, u.Username }).ToList()
    })
    .ToListAsync();

return Ok(roles);
```

**Çözüm 2: Program.cs'de JSON serialization ayarlarını Preserve olarak güncelleme:**
```csharp
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {