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

### Frontend Başlatma Sorunu

**Hata:**
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> ng serve
Error: This command is not available when running the Angular CLI outside a workspace.
```

**Nedeni:**
1. Angular CLI komutu (ng serve) Angular workspace dışında çalıştırılıyor
2. Komut frontend dizininde değil, ana dizinde çalıştırılıyor

**Çözüm:**
```powershell
cd frontend
npm install    # Eğer node_modules yoksa
ng serve
```

### PowerShell Komut Çalıştırma Sorunu

**Hata:**
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> cd frontend && ng serve
At line:1 char:13
+ cd frontend && ng serve
+             ~~
The token '&&' is not a valid statement separator in this version.
```

**Nedeni:**
1. PowerShell, Bash veya CMD'den farklı olarak && operatörünü desteklemiyor
2. PowerShell'de komutları birleştirmek için farklı bir sözdizimi kullanılıyor

**Çözüm:**
```powershell
cd frontend; ng serve
```

veya

```powershell
cd frontend
ng serve
```

### Chart.js Bağımlılık Sorunu

**Hata:**
```
X [ERROR] Could not resolve "chart.js/auto"
    node_modules/primeng/fesm2022/primeng-chart.mjs:5:18:
      5 │ import Chart from 'chart.js/auto';
        ╵                   ~~~~~~~~~~~~~~~
```

**Nedeni:**
1. PrimeNG Chart bileşeni, chart.js kütüphanesine bağımlıdır
2. chart.js kütüphanesi projeye eklenmemiş
3. PrimeNG 19.0.6 sürümü, chart.js kütüphanesini otomatik olarak yüklemiyor

**Çözüm:**
```powershell
cd frontend
npm install chart.js --save
```

### Port Çakışması Sorunu

**Hata:**
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock\frontend> ng serve
? Port 4200 is already in use.
Would you like to use a different port? No
An unhandled exception occurred: Port 4200 is already in use. Use '--port' to specify a different port.
```

**Nedeni:**
1. 4200 portu başka bir uygulama tarafından kullanılıyor
2. Önceki Angular uygulaması düzgün kapatılmamış olabilir
3. Başka bir servis 4200 portunu kullanıyor olabilir

**Çözüm:**
1. Çalışan tüm node işlemlerini durdurun:
```powershell
Get-Process -Name "node" -ErrorAction SilentlyContinue | Stop-Process -Force
```

2. Belirli bir portu kullanan işlemleri bulun ve durdurun:
```powershell
netstat -ano | findstr :4200
Get-Process -Id <PID> -ErrorAction SilentlyContinue | Stop-Process -Force
```

3. Farklı bir port kullanarak uygulamayı başlatın:
```powershell
ng serve --port 4202
```

**Önemli Notlar:**
- Her servis için ayrı terminal penceresi kullanılmalı
- Backend servisi 5126 portunda çalışmalı
- Frontend servisi 4200 portunda çalışmalı
- Servisleri başlatmadan önce tüm portların müsait olduğundan emin olunmalı
- PowerShell'de komutları birleştirmek için ; (noktalı virgül) kullanılmalı
- Port çakışması durumunda farklı bir port kullanılabilir
- PrimeNG Chart bileşeni için chart.js kütüphanesi gereklidir

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
  "Microsoft.EntityFrameworkCore.Relational, Version=9.0.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" ve "Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60" arasında çakışma vardı.
```

**Nedeni:**
1. Projede farklı sürümlerde Entity Framework Core paketleri kullanılıyor
2. Farklı projelerde farklı sürümlerde paketler referans edilmiş
3. Paket sürümleri arasında tutarsızlık var

**Çözüm:**
1. Tüm projelerde aynı sürümde Entity Framework Core paketlerini kullanın:
```powershell
cd Stock.API
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 9.0.2
cd ../src/Stock.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 9.0.2
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

### Hata: 401 Unauthorized Hatası (Giriş Yapma Sorunu)
**Tarih:** 2025-03-02
**Hata Mesajı:** 
```
POST http://localhost:5126/api/auth/login 401 (Unauthorized)
```

**Nedeni:**
1. Veritabanında kullanıcı kaydı bulunmuyor olabilir
2. Kullanıcı adı veya şifre yanlış olabilir
3. Veritabanı bağlantısı doğru yapılandırılmamış olabilir
4. Veritabanı migration'ları uygulanmamış olabilir
5. Seed data oluşturulmamış olabilir

**Çözüm:**
1. Veritabanını sıfırlayıp yeniden oluşturun:
```powershell
cd Stock.API
dotnet ef database drop --force
dotnet ef database update
```

2. Admin kullanıcısını manuel olarak oluşturun:
```csharp
// Seed data oluşturma
var adminUser = new User
{
    Username = "Admin",
    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), // BCrypt formatında hash
    IsAdmin = true,
    RoleId = 1,
    CreatedAt = seedDate,
    IsDeleted = false
};
context.Users.Add(adminUser);
context.SaveChanges();
```

3. Doğru kullanıcı adı ve şifre ile giriş yapmayı deneyin:
```json
{
  "username": "Admin",
  "password": "Admin123"
}
```

4. Veritabanı bağlantı ayarlarını kontrol edin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=czDb;Username=postgres;Password=19909687"
}
```

5. API'nin çalıştığından ve doğru portta olduğundan emin olun (5126)

**Önemli Notlar:**
- Veritabanı migration'larının başarıyla uygulandığından emin olun
- Seed data'nın oluşturulduğundan emin olun
- Kullanıcı adı ve şifrenin doğru olduğundan emin olun
- Şifre hash'leme algoritmasının doğru çalıştığından emin olun
- API'nin doğru portta çalıştığından emin olun
- Swagger UI üzerinden API'yi test edin

## Şifre Hashleme ve Doğrulama Sorunları

### BCrypt.Net-Next Güncellemesi

**Sorun:** Şifre hashleme için kullanılan PBKDF2 algoritması, modern güvenlik standartlarına göre yeterince güçlü değil ve Türkçe karakterleri doğru şekilde işleyemiyor.

**Nedeni:**
1. PBKDF2 algoritması, brute-force saldırılarına karşı BCrypt kadar dirençli değil
2. Karakter kodlama sorunları nedeniyle Türkçe karakterler doğru şekilde işlenemiyor
3. Şifre hashleme için work factor parametresi kullanılmıyor

**Çözüm:**
1. BCrypt.Net-Next paketini NuGet üzerinden projeye ekle:
```bash
dotnet add package BCrypt.Net-Next
```

2. PasswordHasher sınıfını güncelle:
```csharp
public string HashPassword(string password)
{
    // BCrypt.Net-Next kullanarak şifreyi hashle
    // Varsayılan olarak 13 work factor kullanılıyor
    return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
}

public bool VerifyPassword(string password, string passwordHash)
{
    // BCrypt.Net-Next kullanarak şifreyi doğrula
    return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}
```

3. CreateAdminUser.cs dosyasını güncelle:
```csharp
var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123", 13);
```

**Önemli Notlar:**
- EnhancedHashPassword ve EnhancedVerify metotları, UTF-8 karakter kodlamasını destekler
- Work factor değeri (13), güvenlik ve performans arasında denge sağlar
- Mevcut kullanıcıların şifreleri, giriş yaptıklarında otomatik olarak BCrypt formatına dönüştürülmelidir

### Seed Data ve PasswordHasher Uyumsuzluğu

**Sorun:** ApplicationDbContext'teki seed data'da PBKDF2 formatında bir hash kullanılırken, PasswordHasher sınıfı BCrypt kullanıyor. Bu nedenle, seed data ile oluşturulan kullanıcılar ile giriş yapılamıyor.

**Hata Mesajı:**
```
POST http://localhost:5037/api/auth/login 401 (Unauthorized)
```

**Nedeni:**
1. Seed data'daki şifre hash'i ile PasswordHasher sınıfındaki doğrulama algoritması uyumsuz
2. Seed data'da PBKDF2 formatında hash kullanılırken, PasswordHasher BCrypt kullanıyor
3. BCrypt.Verify metodu PBKDF2 formatındaki hash'leri doğrulayamıyor

**Çözüm:**
1. Veritabanını sıfırla:
```powershell
cd src/Stock.API
dotnet ef database drop --force
```

2. ApplicationDbContext'teki seed data'yı BCrypt formatına güncelle:
```csharp
// Seed admin user
modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        Username = "admin",
        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123"), // BCrypt formatında hash
        IsAdmin = true,
        RoleId = 1,
        CreatedAt = seedDate,
        IsDeleted = false
    }
);
```

3. Veritabanını yeniden oluştur:
```powershell
dotnet ef database update
```

4. Alternatif olarak, register endpoint'i ile manuel kullanıcı oluştur:
```
POST http://localhost:5037/api/auth/register
{
  "username": "Admin",
  "password": "Admin123",
  "confirmPassword": "Admin123",
  "isAdmin": true
}
```

**Önemli Notlar:**
- Şifre hashleme algoritması değiştirildiğinde, mevcut kullanıcıların şifreleri geçersiz hale gelir
- Seed data'daki hash formatı ile PasswordHasher sınıfındaki format aynı olmalıdır
- BCrypt, modern web uygulamaları için PBKDF2'den daha güvenli bir seçenektir
- Şifre doğrulama hatalarında 401 Unauthorized hatası döner, bu normal bir davranıştır

## BCrypt Verify Metodu Sorunu

**Hata:** 
```
POST http://localhost:5037/api/auth/login 401 (Unauthorized)
```

**Tarih:** 2025-03-03

**Sorun:** 
Kullanıcı giriş yaparken 401 Unauthorized hatası alınıyor. Hata mesajları incelendiğinde, BCrypt.Verify metodunun şifre doğrulamasında başarısız olduğu görülüyor.

**Nedeni:**
1. PasswordHasher sınıfında BCrypt.Verify metodu kullanılırken, seed data'da BCrypt.EnhancedHashPassword ile oluşturulan hash değeri kullanılıyor.
2. BCrypt.Verify metodu, BCrypt.EnhancedHashPassword ile oluşturulan hash'leri doğrulayamıyor.

**Çözüm:**
1. PasswordHasher sınıfındaki VerifyPassword metodunu güncelleyerek BCrypt.Verify yerine BCrypt.EnhancedVerify kullanmak:

```csharp
public bool VerifyPassword(string password, string passwordHash)
{
    try
    {
        // BCrypt.Net-Next kullanarak şifreyi doğrula
        return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
    catch (Exception)
    {
        // Hata durumunda false döndür
        return false;
    }
}
```

2. Veritabanını sıfırlayıp yeniden oluşturmak:
```
dotnet ef database drop --force
dotnet ef database update
```

**Önemli Notlar:**
- BCrypt.EnhancedHashPassword ve BCrypt.EnhancedVerify metotları UTF-8 karakter kodlamasını destekler ve Türkçe karakterleri doğru şekilde işler.
- BCrypt.Verify ve BCrypt.EnhancedVerify metotları farklı hash formatları kullanır ve birbirlerinin yerine kullanılamazlar.
- Şifre doğrulama hatalarında 401 Unauthorized hatası döner, bu normal bir davranıştır.
- Kullanıcı adı: `admin`, şifre: `Admin123`

## Bağımlılık Enjeksiyon Hataları

### Hata: IAuthService Bağımlılık Çözümleme Hatası
**Tarih:** 2025-03-03
**Hata Mesajı:** 
```
System.InvalidOperationException: Unable to resolve service for type 'Stock.Application.Common.Interfaces.IAuthService' while attempting to activate 'Stock.API.Controllers.AuthController'.
```

**Nedeni:**
1. `IAuthService` arayüzünün bağımlılık enjeksiyon konteynerine kaydedilmemesi
2. Program.cs dosyasında Application ve Infrastructure katmanlarındaki DependencyInjection sınıflarının çağrılmaması
3. Infrastructure katmanında IAuthService kaydının eksik olması

**Çözüm:**
1. `Program.cs` dosyasında Application ve Infrastructure katmanlarının DependencyInjection sınıflarının çağrıldığından emin olun:

```csharp
// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

2. Infrastructure katmanındaki `DependencyInjection.cs` dosyasında `IAuthService` arayüzünün `AuthService` sınıfına bağlandığından emin olun:

```csharp
services.AddScoped<IAuthService, AuthService>();
```

3. Uygulamayı yeniden başlatın.

**Kontrol Listesi:**
- Program.cs dosyasında AddApplication() ve AddInfrastructure() metotları çağrılmış mı?
- Infrastructure katmanında IAuthService kaydı var mı?
- Uygulama yeniden başlatıldı mı?
- Entity Framework paket sürümleri arasında çakışma var mı? (Uyarı olarak görünebilir)

**Önemli Notlar:**
- Clean Architecture'da her katmanın kendi DependencyInjection sınıfı olmalıdır.
- Bu sınıflar `Program.cs` dosyasında mutlaka çağrılmalıdır.
- Bağımlılıkların doğru sırada kaydedilmesi önemlidir. Önce Application, sonra Infrastructure katmanı kaydedilmelidir.
- Servis kayıtları eksik olduğunda, controller'lar oluşturulurken bağımlılık çözümleme hataları alınabilir.

## Entity Framework Core Paket Sürüm Uyumsuzluğu Hatası

**Hata Mesajı:** System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'.

**Nedeni:** Stock.API projesi ile Infrastructure projesi arasında Entity Framework Core paketlerinin sürüm uyumsuzluğu.

**Çözüm:**
1. Stock.API projesindeki Entity Framework Core paketlerini, Infrastructure projesinde kullanılan sürümlerle uyumlu hale getirin:
   ```
   dotnet add package Microsoft.EntityFrameworkCore --version 9.0.2
   dotnet add package Microsoft.EntityFrameworkCore.Relational --version 9.0.2
   dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 9.0.4
   ```
2. Çalışan API sürecini durdurun:
   ```
   Get-Process -Name "Stock.API" | Stop-Process -Force
   ```
3. Projeyi yeniden derleyin:
   ```
   dotnet build
   ```
4. API'yi yeniden başlatın:
   ```
   dotnet run
   ```

**Not:** Farklı projelerde kullanılan paket sürümlerinin uyumlu olduğundan emin olun. Çalışan süreçlerin dosyaları kilitleyebileceğini ve derleme hatalarına neden olabileceğini unutmayın.

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

2. app.routes.ts dosyasında rotaların tanımlı olduğundan emin ol:
```typescript
export const routes: Routes = [
  { 
    path: 'admin-dashboard', 
    component: AdminDashboardComponent,
    canActivate: [AuthGuard],
    data: { requiresAdmin: true }
  },
  { 
    path: 'user-dashboard', 
    component: UserDashboardComponent,
    canActivate: [AuthGuard]
  }
];
```

**Önemli Notlar:**
- Yönlendirme işlemi sırasında konsola detaylı log mesajları eklendi
- AuthService'den kullanıcı bilgisi alınarak doğru dashboard'a yönlendirme yapılıyor
- Her iki dashboard rotası da AuthGuard ile korunuyor
- Admin dashboard için ekstra requiresAdmin data parametresi eklendi
