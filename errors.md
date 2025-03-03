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

**Nedeni:** Backend API'si çalışmıyor veya hatalı çalışıyor olabilir.

**Çözüm:**
1. Backend API'sinin çalıştığından emin olun.
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
- Şifre değiştirme işlemi için kullanıcının kimliği doğrulanmalı (Authorize attribute)
- Mevcut şifre doğrulanmalı
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

## Kullanıcı Yönetimi Sayfası Tasarım ve İşlevsellik Sorunları

### Sorun: Kullanıcı Yönetimi Sayfası Tasarım Uyumsuzluğu

**Hata Açıklaması:** Kullanıcı yönetimi sayfası, istenen tasarıma uygun değildi. Satır yükseklikleri, avatar boyutları, metin stilleri ve genel düzen referans görüntüden farklıydı.

**Nedeni:**
1. PrimeNG tablosunun varsayılan stilleri istenen tasarıma uygun değildi
2. Kullanıcı satırları arasında çizgiler vardı, ancak referans tasarımda sadece boşluk bulunuyordu
3. Avatar görüntüleri çok küçüktü ve kullanıcı bilgileri yeterince belirgin değildi
4. Rozet stilleri referans tasarımla uyuşmuyordu

**Çözüm:**
1. PrimeNG tablosu yerine özel bir tablo yapısı oluşturuldu:

```html
<div class="custom-table">
  <div class="table-header">
    <!-- Tablo başlıkları -->
  </div>
  <div class="table-row" *ngFor="let user of filteredUsers">
    <!-- Kullanıcı bilgileri -->
  </div>
</div>
```

2. Özel CSS stilleri eklendi:

```scss
.custom-table {
  background-color: #1e1e1e;
  border-radius: 8px;
  overflow: hidden;
  
  .table-row {
    display: flex;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    min-height: 100px;
    
    &:hover {
      background-color: rgba(255, 255, 255, 0.05);
    }
  }
  
  .user-avatar {
    width: 70px;
    height: 70px;
    border-radius: 50%;
    object-fit: cover;
  }
}
```

3. Rozet stilleri güncellendi:

```scss
.user-badge {
  padding: 5px 10px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 500;
  
  &.admin {
    background-color: rgba(220, 38, 38, 0.2);
    color: #ef4444;
  }
  
  &.editor {
    background-color: rgba(59, 130, 246, 0.2);
    color: #3b82f6;
  }
  
  &.user {
    background-color: rgba(34, 197, 94, 0.2);
    color: #22c55e;
  }
}
```

### Sorun: Kullanıcı Yönetimi Sayfası İşlevsellik Eksikliği

**Hata Açıklaması:** Kullanıcı yönetimi sayfasında arama, filtreleme ve sayfalama işlevleri çalışmıyordu.

**Nedeni:**
1. Bileşen dosyasında gerekli fonksiyonlar tanımlanmamıştı
2. HTML dosyasında işlevsellik için gerekli bağlantılar eksikti
3. Türkçe dil desteği yoktu

**Çözüm:**
1. Bileşen dosyasına gerekli özellikler ve fonksiyonlar eklendi:

```typescript
filteredUsers: User[] = [];
activeFilters: { role: string | null, joined: string | null } = { role: null, joined: null };
searchText: string = '';

ngOnInit(): void {
  this.loadUsers();
}

applyFilters(): void {
  this.filteredUsers = [...this.users];
  
  // Arama filtresi
  if (this.searchText) {
    const searchLower = this.searchText.toLowerCase();
    this.filteredUsers = this.filteredUsers.filter(user => 
      user.name.toLowerCase().includes(searchLower) || 
      user.email.toLowerCase().includes(searchLower)
    );
  }
  
  // Rol filtresi
  if (this.activeFilters.role) {
    this.filteredUsers = this.filteredUsers.filter(user => 
      user.role.toLowerCase() === this.activeFilters.role?.toLowerCase()
    );
  }
  
  // Katılma tarihi filtresi
  if (this.activeFilters.joined) {
    // Katılma tarihine göre filtreleme mantığı
  }
}
```

2. HTML dosyasında arama ve filtreleme bileşenleri eklendi:

```html
<div class="search-filters">
  <div class="search-box">
    <i class="pi pi-search"></i>
    <input type="text" [(ngModel)]="searchText" (input)="applyFilters()" placeholder="Kullanıcı ara...">
  </div>
  
  <div class="filter-options">
    <p-dropdown [options]="roleOptions" [(ngModel)]="activeFilters.role" 
                (onChange)="applyFilters()" placeholder="Rol filtresi">
    </p-dropdown>
    
    <p-dropdown [options]="joinedOptions" [(ngModel)]="activeFilters.joined" 
                (onChange)="applyFilters()" placeholder="Katılma tarihi">
    </p-dropdown>
  </div>
</div>
```

### Öğrenilen Dersler

1. **Tasarım Referanslarına Sadık Kalma:** Bir tasarımı uygularken, referans görüntüye mümkün olduğunca sadık kalmak önemlidir. Bu, kullanıcı deneyiminin tutarlılığını sağlar.

2. **Özel Bileşenler vs. Kütüphane Bileşenleri:** Bazen kütüphane bileşenlerini (PrimeNG gibi) özelleştirmek yerine, sıfırdan özel bileşenler oluşturmak daha etkili olabilir. Bu, daha fazla kontrol sağlar ve tasarım gereksinimlerine daha iyi uyum sağlar.

3. **İşlevsellik ve Tasarım Dengesi:** Bir sayfayı tasarlarken, hem görsel çekiciliği hem de işlevselliği dengelemek önemlidir. Güzel görünen ancak işlevsel olmayan bir sayfa, kullanıcı deneyimini olumsuz etkiler.

4. **Dil Desteği:** Uluslararası kullanıcılar için dil desteği sağlamak, kullanıcı deneyimini iyileştirir. Türkçe gibi yerel dilleri desteklemek, kullanıcıların uygulamayı daha kolay kullanmasını sağlar.

5. **Responsive Tasarım:** Farklı ekran boyutlarında düzgün çalışan responsive bir tasarım oluşturmak, tüm kullanıcılar için tutarlı bir deneyim sağlar.

## Kullanıcı Yönetimi Sayfası Tasarım Güncellemeleri

### Checkbox Görünümü Sorunları

**Hata:** Checkbox'ların görünümü referans görseldeki gibi değildi. PrimeNG'nin varsayılan checkbox stilleri istenen tasarıma uygun değildi.

**Nedeni:**
1. PrimeNG'nin varsayılan checkbox stilleri koyu tema ile uyumlu değildi
2. Checkbox'ların boyutu ve renkleri istenen tasarıma uygun değildi
3. Checkbox'ların yanındaki etiketler için özel stiller gerekiyordu

**Çözüm:**
1. SCSS dosyasında checkbox'lar için özel stiller tanımlandı:
```scss
.field-checkbox {
  display: flex;
  align-items: center;
  margin-bottom: 1rem;
  position: relative;
  padding: 0.5rem 0.75rem;
  border-radius: 6px;
  transition: all 0.2s;
  
  &:hover {
    background-color: rgba(255, 255, 255, 0.05);
  }
  
  .p-checkbox {
    width: 24px;
    height: 24px;
    
    .p-checkbox-box {
      border-radius: 4px;
      border: 2px solid rgba(255, 255, 255, 0.3);
      background-color: transparent;
      transition: all 0.2s;
      width: 24px;
      height: 24px;
      
      &.p-highlight {
        border-color: #ff5722;
        background-color: #ff5722;
      }
    }
  }
}
```

2. Checkbox etiketleri için özel stiller ve ikonlar eklendi:
```html
<label for="isAdmin" class="ml-2">
  <i class="pi pi-shield mr-1"></i>
  Yönetici yetkisi
</label>
```

3. Farklı checkbox'lar için farklı renkler tanımlandı:
```scss
#isAdmin + label {
  color: #ff5722;
  
  .pi-shield {
    color: #ff5722;
  }
}

#isActive + label {
  color: #4caf50;
  
  .pi-check-circle {
    color: #4caf50;
  }
}
```

### Form Alanları Etiket Sorunları

**Hata:** Form alanlarının üzerindeki etiketler gereksiz yer kaplıyordu ve tasarımı karmaşıklaştırıyordu.

**Nedeni:**
1. Form alanlarının üzerinde etiketler vardı, ancak input alanlarının içinde zaten placeholder metinleri bulunuyordu
2. Bu durum gereksiz tekrara ve karmaşık bir görünüme neden oluyordu

**Çözüm:**
1. HTML dosyasından etiketler kaldırıldı:
```html
<div class="field mb-4">
  <div class="p-inputgroup">
    <span class="p-inputgroup-addon">
      <i class="pi pi-id-card"></i>
    </span>
    <input type="text" id="fullName" pInputText formControlName="fullName" 
      placeholder="Ad soyad girin">
  </div>
</div>
```

2. Input alanlarının görünümü iyileştirildi:
```scss
.p-inputgroup {
  border-radius: 4px;
  overflow: hidden;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  
  .p-inputgroup-addon {
    background-color: #2d2d2d;
    border: none;
    border-right: 1px solid rgba(255, 255, 255, 0.1);
    color: rgba(255, 255, 255, 0.6);
    border-radius: 4px 0 0 4px;
    width: 48px;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  input {
    background-color: #2d2d2d;
    border: none;
    color: #fff;
    padding: 0.75rem 1rem;
    height: 48px;
    font-size: 1rem;
    
    &::placeholder {
      color: rgba(255, 255, 255, 0.5);
    }
    
    &:focus {
      outline: none;
      box-shadow: 0 0 0 2px rgba(255, 87, 34, 0.3);
    }
  }
}
```

### Dialog Footer Düzenleme Sorunları

**Hata:** Dialog footer kısmındaki butonlar istenen tasarıma uygun değildi.

**Nedeni:**
1. Butonlar arasında yeterli boşluk yoktu
2. Butonların genişliği tutarsızdı
3. Buton stilleri istenen tasarıma uygun değildi

**Çözüm:**
1. Dialog footer kısmı için özel bir div eklendi:
```html
<ng-template pTemplate="footer">
  <div class="dialog-footer">
    <button pButton pRipple label="İptal" icon="pi pi-times" class="p-button-text p-button-outlined" (click)="hideDialog()"></button>
    <button pButton pRipple label="Kaydet" icon="pi pi-check" class="p-button-primary" (click)="saveUser()"></button>
  </div>
</ng-template>
```

2. Dialog footer için özel stiller tanımlandı:
```scss
.dialog-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  
  button {
    min-width: 100px;
  }
}
```

3. Buton stilleri özelleştirildi:
```scss
button {
  &.p-button-outlined {
    color: rgba(255, 255, 255, 0.8);
    border-color: rgba(255, 255, 255, 0.2);
    
    &:hover {
      background-color: rgba(255, 255, 255, 0.05);
      border-color: rgba(255, 255, 255, 0.3);
    }
  }
  
  &.p-button-primary {
    background-color: #ff5722;
    border-color: #ff5722;
    
    &:hover {
      background-color: darken(#ff5722, 5%);
      border-color: darken(#ff5722, 5%);
    }
  }
}
```

### PowerShell Komut Çalıştırma Sorunları (Angular Serve)

**Hata:** Angular uygulamasını başlatırken port çakışması sorunu yaşandı.

**Nedeni:**
1. 4200 portu zaten kullanımda olduğu için Angular uygulaması başlatılamadı
2. PowerShell'de komutları birleştirmek için && operatörü kullanıldı, ancak bu PowerShell'de çalışmıyor

**Çözüm:**
1. Farklı bir port numarası belirtildi:
```powershell
cd frontend; ng serve --port 4206
```

2. PowerShell'de komutları birleştirmek için ; (noktalı virgül) kullanıldı:
```powershell
cd frontend; ng serve
```
