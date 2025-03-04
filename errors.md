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
- [Kullanıcı Yönetimi Entegrasyonu](#kullanıcı-yönetimi-entegrasyonu)
- [Admin Dashboard'dan Kullanıcı Yönetimine Yönlendirme](#admin-dashboard'dan-kullanıcı-yönetimine-yönlendirme)
- [Kullanıcı Yönetimi Bileşeni Entegrasyonu](#kullanıcı-yönetimi-bileşeni-entegrasyonu)
- [PrimeNG ve Tailwind CSS Entegrasyonu](#primeNG-ve-tailwind-CSS-entegrasyonu)
- [Tasarım Renk Şeması Değişikliği](#tasarım-renk-şeması-değişikliği)
- [PrimeNG Bileşen Özellikleri Hataları](#primeNG-bileşen-özellikleri-hataları)
- [Kullanıcı Yönetimi Sayfası Tasarım ve İşlevsellik Sorunları](#kullanıcı-yönetimi-sayfası-tasarım-ve-işlevsellik-sorunları)
- [Kullanıcı Yönetimi Sayfası Tasarım Güncellemeleri](#kullanıcı-yönetimi-sayfası-tasarım-güncellemeleri)
  - [Checkbox Görünümü Sorunları](#checkbox-görünümü-sorunları)
  - [Form Alanları Etiket Sorunları](#form-alanları-etiket-sorunları)
  - [Dialog Footer Düzenleme Sorunları](#dialog-footer-düzenleme-sorunları)
  - [PowerShell Komut Çalıştırma Sorunları (Angular Serve)](#powershell-komut-çalıştırma-sorunları-angular-serve)
- [İzin Yönetimi Hataları](#izin-yönetimi-hataları)
  - [Hata 1: RoleService'de getRoleById metodu eksikti](#hata-1-role-service-de-getrolebyid-metodu-eksikti)
  - [Hata 2: App routing modülünde Routes ve AuthGuard tanımlı değildi](#hata-2-app-routing-modülünde-routes-ve-authguard-tanımlı-değildi)
- [Sicil Alanı Ekleme İşlemi](#sicil-alanı-ekleme-işlemi)
  - [Kullanıcı Modeline Sicil Alanı Ekleme](#kullanıcı-modeline-sicil-alanı-ekleme)
  - [Kullanıcı Verilerinin Görüntülenmesi Sorunu](#kullanıcı-verilerinin-görüntülenmesi-sorunu)
  - [API Endpoint Hatası](#api-endpoint-hatası)
  - [API URL Hatası](#api-url-hatası)
- [Kullanıcı Silme İşlemi Hataları](#kullanıcı-silme-işlemi-hataları)
- [Kullanıcı Güncelleme Hatası](#kullanıcı-güncelleme-hatası)
- [Frontend Derleme Hataları](#frontend-derleme-hataları)
- [Kullanıcı Ekleme Hatası (500 Internal Server Error)](#kullanıcı-ekleme-hatası-500-internal-server-error)
- [Sicil Alanı Benzersizlik Kontrolü](#sicil-alanı-benzersizlik-kontrolü)

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
1. Frontend ve backend arasındaki model uyumluluğu kritik önem taşır
2. Clean Architecture'da değişiklikler tüm katmanları etkileyebilir
3. CQRS pattern kullanıldığı için Command ve Handler sınıflarının eksiksiz tanımlanması gerekir
4. API endpoint'lerini test etmek için Swagger veya Postman gibi araçlar kullanılmalıdır
5. Hata ayıklama için detaylı loglama yapılması sorunların hızlı çözülmesine yardımcı olur
6. Frontend isteklerinin beklendiği gibi formatlarda gönderilip gönderilmediğini kontrol etmek önemlidir

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
