# Proje Başlatma Sorunları ve Çözümleri

## Backend Başlatma Sorunu (18.02.2024)

### Sorun:
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> dotnet run
Çalıştırılacak bir proje bulunamadı. C:\Users\muham\OneDrive\Masaüstü\Stock içinde bir projenin bulunduğundan emin olun veya --project kullanarak yolu projeye geçirin.
```

### Nedeni:
1. Proje ana dizininde çalıştırıldığında dotnet run komutu çalıştırılacak projeyi bulamıyor
2. PowerShell'de && operatörü desteklenmiyor
3. Doğru dizinde olmadan komut çalıştırılıyor

### Çözüm:
1. Backend için:
```powershell
cd backend/StockAPI
dotnet run --project StockAPI.csproj
```

2. Frontend için (yeni terminal):
```powershell
cd frontend
ng serve --open
```

### Önemli Notlar:
- Her servis için ayrı terminal penceresi kullanılmalı
- Backend servisi 5126 portunda çalışmalı
- Frontend servisi 4200 portunda çalışmalı
- Servisleri başlatmadan önce tüm portların müsait olduğundan emin olunmalı

## Frontend Başlatma Sorunu (18.02.2024)

### Sorun:
```
PS C:\Users\muham\OneDrive\Masaüstü\Stock> ng serve
Error: This command is not available when running the Angular CLI outside a workspace.
```

### Nedeni:
1. Angular CLI komutu (ng serve) Angular workspace dışında çalıştırılıyor
2. Komut frontend dizininde değil, ana dizinde çalıştırılıyor

### Çözüm:
```powershell
cd frontend
npm install    # Eğer node_modules yoksa
ng serve
```

### Önemli Notlar:
- Angular komutları her zaman frontend dizini içinde çalıştırılmalı
- Proje ilk kez çalıştırılıyorsa önce npm install ile bağımlılıklar yüklenmeli
- ng serve komutu başarılı olduğunda http://localhost:4200 adresinde uygulama çalışacak

## Frontend TypeScript Hatası (18.02.2024)

### Sorun:
```
X [ERROR] TS2345: Argument of type '{ username: any; passwordHash: any; isAdmin: any; roleId: any; }' is not assignable to parameter of type 'User'.
  Property 'createdAt' is missing in type '{ username: any; passwordHash: any; isAdmin: any; roleId: any; }' but required in type 'User'.
```

### Nedeni:
1. User interface'inde createdAt alanı zorunlu (required) olarak tanımlanmış
2. Yeni kullanıcı oluştururken bu alan gönderilmiyor
3. TypeScript tip uyumsuzluğu hatası oluşuyor

### Çözüm:
1. CreateUserDto interface'inde createdAt alanını opsiyonel yap:
```typescript
export interface CreateUserDto {
    username: string;
    password: string;
    passwordHash?: string;
    isAdmin: boolean;
    roleId?: number;
    createdAt?: Date;
}
```

2. Kullanıcı oluştururken createdAt alanını ekle:
```typescript
const newUser = {
    username: userData.username,
    passwordHash: userData.password,
    isAdmin: userData.isAdmin,
    roleId: userData.roleId,
    createdAt: new Date()
};
```

### Önemli Notlar:
- TypeScript tip güvenliği için interface'leri doğru tanımlamak önemli
- DTO (Data Transfer Object) interface'leri ile model interface'leri farklı olabilir
- Backend'de oluşturulacak alanlar frontend DTO'larında opsiyonel olmalı

## Hash Uyumsuzluğu Sorunu (19.02.2024)

### Sorun:
```
[22:24:42 INF] Gelen şifre hash'i: JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=
[22:24:42 INF] DB'deki hash: pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=
[22:24:42 WRN] Şifre eşleşmedi - Kullanıcı: admin
```

### Nedeni:
1. Seed data'da kullanılan hash algoritması ile giriş sırasında kullanılan hash algoritması farklı
2. Veritabanındaki admin kullanıcısının hash'i yanlış değere sahip
3. Hash hesaplama yöntemi sistemde tutarlı değil

### Çözüm:
1. HashService'i düzgün yapılandır:
```csharp
public string ComputeSha256Hash(string input)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(bytes);
    }
}
```

2. Veritabanını sıfırla ve yeniden oluştur:
```powershell
cd backend/StockAPI
dotnet ef migrations remove --context StockContext --force
dotnet ef migrations add RebuildDatabase --context StockContext
dotnet ef database update --context StockContext
```

3. Seed data'da doğru hash'i kullan:
```csharp
modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        Username = "admin",
        PasswordHash = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", // admin123
        IsAdmin = true,
        RoleId = 1,
        CreatedAt = seedTime
    }
);
```

### Önemli Notlar:
- Hash algoritması tüm sistemde tutarlı olmalı
- Seed data'daki hash'ler doğru hesaplanmalı
- Detaylı loglama hash sorunlarını tespit etmeye yardımcı olur
- Migration'ları temiz tutmak önemli

## Rol Yönetimi Tip Uyumsuzluğu Hatası (21.02.2024)

### Sorun:
```
TS2322: Type 'RoleWithUsers[]' is not assignable to type 'Role[]'.
  Type 'RoleWithUsers' is not assignable to type 'Role'.
    Types of property 'createdAt' are incompatible.
      Type 'string' is not assignable to type 'Date'.
```

### Nedeni:
1. `RoleWithUsers` interface'i iki farklı yerde tanımlanmış
2. `Role` modelinde `createdAt` alanı sadece `Date` tipinde tanımlanmış
3. Backend'den gelen veriler string formatında

### Çözüm:
1. `role.model.ts`'de `createdAt` ve `updatedAt` tiplerini güncelledik:
```typescript
export interface Role {
    id: number;
    name: string;
    createdAt: string | Date;
    updatedAt?: string | Date;
}
```

2. `RoleWithUsers` interface'ini tek bir yerde (models) tanımladık:
```typescript
export interface RoleWithUsers extends Role {
    users: {
        id: number;
        username: string;
        isAdmin: boolean;
        createdAt: string;
    }[];
}
```

3. `role.service.ts`'den duplicate interface'i kaldırdık ve models'dan import ettik:
```typescript
import { Role, RoleWithUsers } from '../models/role.model';
```

### Önemli Notlar:
- Interface'ler tek bir yerde tanımlanmalı ve oradan import edilmeli
- Backend'den gelen tarih verileri string olarak geldiği için tip tanımlarında bunu hesaba katmalıyız
- Tip uyumsuzluklarını çözerken veri akışını dikkatlice takip etmeliyiz

## Proje Çökme ve Git ile Kurtarma (21.02.2024)

### Sorun:
- Proje dosyaları silinmiş veya bozulmuş
- Backend servisleri çalışmıyor
- Git index dosyası kilitli kalmış

### Nedeni:
1. Git işlemleri sırasında beklenmedik kesinti
2. Dosya sistemi kilitleri düzgün temizlenmemiş
3. Çalışan servisler düzgün kapatılmamış

### Çözüm:
1. Tüm IDE'leri ve servisleri kapat
2. PowerShell'i yönetici olarak çalıştır
3. Sırasıyla şu komutları çalıştır:
```powershell
Stop-Process -Name "StockAPI" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "dotnet" -Force -ErrorAction SilentlyContinue
Remove-Item -Force .git/index.lock -ErrorAction SilentlyContinue
git reset --hard HEAD
git clean -fd
```
4. Backend'i başlat:
```powershell
cd backend/StockAPI; dotnet run
```
5. Frontend'i başlat (yeni terminal):
```powershell
cd frontend; npm start
```

### Önemli Notlar:
- Düzenli commit yapılmalı
- Önemli değişikliklerden önce branch oluşturulmalı
- Değişiklikler test edilmeden ana branch'e merge edilmemeli
- PowerShell'de && yerine ; kullanılmalı
- Servisleri kapatırken tüm bağlantılı işlemlerin sonlandırıldığından emin olunmalı

## PrimeNG Dropdown Görünüm ve Seçim Sorunları (21.02.2024)

### Sorun:
1. Dropdown menüde roller tam görünmüyor, liste kısıtlı kalıyor
2. Seçilen rol "[object Object]" olarak görünüyor
3. Dropdown içeriği sayfa içinde sınırlı kalıyor

### Nedeni:
1. Dropdown varsayılan ayarları uzun listeler için optimize edilmemiş
2. Template tanımlaması eksik
3. Dropdown container'ı sayfa içinde kısıtlı

### Çözüm:
1. HTML template'de dropdown özelliklerini güncelledik:
```html
<p-dropdown 
  [options]="roles" 
  formControlName="name" 
  optionLabel="name" 
  [showClear]="true" 
  placeholder="Rol seçin" 
  [filter]="true" 
  [virtualScroll]="true" 
  [virtualScrollItemSize]="38"
  [style]="{'width':'100%'}" 
  appendTo="body">
  <ng-template pTemplate="selectedItem" let-role>
    <span>{{role?.name}}</span>
  </ng-template>
</p-dropdown>
```

2. Component'te form değerlerini düzenledik:
```typescript
const selectedRole = this.roleForm.get('name')?.value;
const roleData = {
  id: this.editMode ? selectedRole.id : null,
  name: selectedRole.name
};
```

### Önemli Notlar:
- `[virtualScroll]="true"` uzun listelerin performanslı gösterimi için önemli
- `[filter]="true"` arama özelliği ekler
- `appendTo="body"` dropdown menünün sayfa içinde kısıtlanmamasını sağlar
- `[style]="{'width':'100%'}"` genişlik sorunlarını çözer
- Template'de seçilen değerin gösterimi için `selectedItem` template'i kullanılmalı
- Form değerleri kaydedilirken seçilen nesnenin doğru özelliği (name) alınmalı

## .NET Core 9 Geçiş Sorunları (Potansiyel)

### Sorun 1: SDK Uyumluluk Hatası
```
error NETSDK1045: Geçerli .NET SDK, .NET 9.0 hedefini desteklemiyor.
```

### Nedeni:
1. .NET Core 9 SDK yüklü değil
2. Visual Studio sürümü uyumlu değil
3. Global.json dosyası eski SDK sürümünü zorluyor olabilir

### Çözüm:
1. .NET Core 9 SDK'yı yükle:
```powershell
winget install Microsoft.DotNet.SDK.9
```

2. Visual Studio 2024'ü yükle veya güncelle
3. Global.json dosyasını güncelle veya kaldır
4. Sistemi yeniden başlat

### Önemli Notlar:
- SDK yüklemeden önce tüm Visual Studio instance'larını kapat
- Yükleme sonrası PATH değişkenlerini kontrol et
- SDK sürümünü doğrula: `dotnet --version`

### Sorun 2: NuGet Paket Uyumsuzlukları
```
error NU1202: Package X is not compatible with net9.0
```

### Nedeni:
1. NuGet paketleri .NET 9'u desteklemiyor
2. Paket versiyonları uyumsuz
3. Paket bağımlılıkları çakışıyor

### Çözüm:
1. packages.config veya .csproj dosyasını güncelle:
```xml
<TargetFramework>net9.0</TargetFramework>
```

2. NuGet paketlerini güncelle:
```powershell
dotnet restore --force
```

3. Uyumsuz paketler için alternatif çözümler bul

### Önemli Notlar:
- Paket güncellemelerinden önce yedek al
- Breaking change'leri kontrol et
- Test coverage'ı korumaya dikkat et

### Sorun 3: Breaking Changes Sorunları
```
error CS0117: 'X' does not contain a definition for 'Y'
```

### Nedeni:
1. .NET 9'da kaldırılan API'ler kullanılıyor
2. Değişen method imzaları
3. Namespace değişiklikleri

### Çözüm:
1. Microsoft'un migration guide'ını takip et
2. Deprecated API'leri yeni versiyonlarıyla değiştir
3. Code analyzer'ları kullan:
```xml
<PropertyGroup>
    <AnalysisMode>All</AnalysisMode>
    <EnableNETAnalyzers>true</EnableNETAnalyzers>
</PropertyGroup>
```

### Önemli Notlar:
- Breaking change'leri dokümante et
- Test coverage'ı artır
- Değişiklikleri aşamalı uygula

### Sorun 4: Entity Framework Core Migrations Sorunları
```
The model backing the 'X' context has changed since the database was created
```

### Nedeni:
1. EF Core 9 değişiklikleri
2. Model değişiklikleri
3. Migration conflict'leri

### Çözüm:
1. Migrations'ları temizle ve yeniden oluştur:
```powershell
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

2. DbContext'i güncelle:
```csharp
public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "your_connection_string",
            x => x.MigrationsHistoryTable("__EFMigrationsHistory")
        );
    }
}
```

### Önemli Notlar:
- Veritabanı yedeklerini al
- Migration script'lerini test et
- Prod deployment öncesi staging'de test et

### Sorun 5: Performance Regression Sorunları
```
warn: Microsoft.AspNetCore.HttpsPolicy[...]
```

### Nedeni:
1. Yeni runtime optimizasyonları
2. Memory kullanım değişiklikleri
3. Threading model değişiklikleri

### Çözüm:
1. Performance profiling yap:
```powershell
dotnet-trace collect --process-id <PID>
```

2. Memory kullanımını optimize et:
```csharp
services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024;
});
```

3. Logging seviyelerini ayarla:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

### Önemli Notlar:
- Performance metriklerini kaydet
- Load testing yap
- Scaling stratejilerini gözden geçir

## AuditLog ve Null Değer Atama Hataları (23.02.2024)

### Sorun:
1. AuditLog sınıfında eksik özellikler:
```
error CS0117: 'AuditLog' bir 'EntityType' tanımı içermiyor
error CS0117: 'AuditLog' bir 'Path' tanımı içermiyor
error CS0117: 'AuditLog' bir 'Details' tanımı içermiyor
```

2. Null değer atama hataları:
```
warning CS8618: Null atanamaz özellik 'Password', 'Username', 'Role' vb. oluşturucudan çıkış yaparken null olmayan bir değer içermelidir.
```

### Nedeni:
1. AuditLog sınıfında bazı özellikler eksikti
2. Bazı özellikler null olarak işaretlenmişti ancak null değer alamazlardı
3. Entity ilişkileri doğru yapılandırılmamıştı

### Çözüm:
1. AuditLog sınıfı BaseEntity'den türetildi
2. Eksik özellikler eklendi:
```csharp
public string EntityType { get; set; } = string.Empty;
public string Path { get; set; } = string.Empty;
public string Details { get; set; } = string.Empty;
```

3. Null atanabilir özellikler düzeltildi:
```csharp
public virtual User? User { get; set; }
public string UserId { get; set; } = string.Empty;
```

4. ApplicationDbContext'te tarih atamaları düzeltildi:
```csharp
var currentTime = DateTime.UtcNow;
entity.UpdatedAt = currentTime;
entity.CreatedAt = currentTime;
```

### Önemli Notlar:
- String özellikler için string.Empty kullanıldı
- Nullable referans tipleri için ? operatörü kullanıldı
- İlişkisel özellikler için null kontrolü eklendi
- UTC zaman damgası kullanıldı
- BaseEntity'den türetme yapıldı

## Derleme Uyarıları ve Hataları (23.02.2024)

### Sorun:
1. ConfigureAwait Uyarıları:
```
warning CA2007: Beklenen görevde ConfigureAwait çağrısı yapmayı düşünün
```

2. Logger Performans Uyarıları:
```
warning CA1848: İyileştirilmiş performans için LoggerMessage temsilcilerini kullanın
```

3. Null Kontrol Uyarıları:
```
warning CA1062: Dışarıdan görünen metodda parametreyi kullanmadan önce null olmadığını doğrulayın
```

4. Genel Exception Yakalama Uyarıları:
```
warning CA1031: Daha belirli bir izin verilen özel durum türünü yakalayın
```

### Nedeni:
1. Async/await çağrılarında ConfigureAwait(false) kullanılmamış
2. ILogger doğrudan kullanılmış, LoggerMessage temsilcileri kullanılmamış
3. Dış parametrelerde null kontrolü yapılmamış
4. Çok genel exception yakalama yapılmış

### Çözüm:
1. ConfigureAwait(false) eklendi:
```csharp
await _context.SaveChangesAsync().ConfigureAwait(false);
```

2. LoggerMessage temsilcileri eklendi:
```csharp
private static readonly Action<ILogger, string, Exception?> _logRoleCreationError =
    LoggerMessage.Define<string>(
        LogLevel.Error,
        new EventId(1, nameof(CreateRole)),
        "Rol oluşturulurken hata oluştu - Name: {RoleName}");
```

3. Null kontrolleri eklendi:
```csharp
ArgumentNullException.ThrowIfNull(createRoleDto);
```

4. Spesifik exception'lar eklendi:
```csharp
catch (DbUpdateException ex)
{
    _logRoleCreationError(_logger, createRoleDto.Name, ex);
    return StatusCode(500, new { message = "Veritabanı hatası oluştu" });
}
```

### Önemli Notlar:
- Her async çağrıda ConfigureAwait(false) kullanılmalı
- LoggerMessage temsilcileri performans için önemli
- Dış parametrelerde her zaman null kontrolü yapılmalı
- Exception'lar mümkün olduğunca spesifik yakalanmalı
- DbUpdateException gibi özel exception'lar ayrı yakalanmalı

## JWT Anahtar Boyutu Hatası (23.02.2024)

### Sorun:
```
IDX10720: Unable to create KeyedHashAlgorithm for algorithm 'http://www.w3.org/2001/04/xmldsig-more#hmac-sha256', the key size must be greater than: '256' bits, key has '224' bits.
```

### Nedeni:
1. JWT token oluşturma için kullanılan anahtar boyutu yetersiz (256 bitten küçük)
2. SecretKey'in uzunluğu yeterli değil
3. Key'in Base64 formatında olmaması

### Çözüm:
1. appsettings.json'da SecretKey'i güncelledik:
```json
{
  "Jwt": {
    "SecretKey": "StockAPI_SuperSecretKey_MinLength32Chars!!",
    "Issuer": "stock-api",
    "Audience": "stock-client",
    "ExpirationHours": 24
  }
}
```

2. Program.cs'de key oluşturma ve kullanma mantığını güncelledik:
```csharp
// JWT için güvenli bir anahtar oluştur (en az 256 bit)
var key = Convert.ToBase64String(Encoding.UTF8.GetBytes(jwtSettings.SecretKey.PadRight(32)));

// Token doğrulama parametrelerinde key'i kullan
options.TokenValidationParameters = new TokenValidationParameters
{
    // ... diğer ayarlar ...
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
};
```

### Önemli Notlar:
- JWT SecretKey en az 32 karakter (256 bit) olmalı
- Key Base64 formatında olmalı
- Token oluşturma ve doğrulama için aynı key kullanılmalı
- Güvenlik için key'in karmaşık ve tahmin edilemez olması önemli

## JWT Token Doğrulama Sorunu (23.02.2024)

### Sorun:
```
IDX10503: Signature validation failed. Token does not have a kid. Keys tried: '[PII...]'. 
Number of keys in TokenValidationParameters: '1'.
```

### Nedeni:
1. JWT token oluşturma ve doğrulama sırasında kullanılan anahtarlar tutarsız
2. Token oluşturma ve doğrulama için farklı key'ler kullanılıyor
3. Key boyutu yetersiz veya uyumsuz

### Çözüm:
1. Program.cs'de JWT yapılandırmasını düzenledik:
```csharp
var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey.PadRight(32));
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings.Issuer,
    ValidAudience = jwtSettings.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(key)
};
```

### Önemli Notlar:
- Key oluşturma mantığı her yerde aynı olmalı
- Key boyutu en az 256 bit (32 karakter) olmalı
- UTF8 encoding kullanılmalı
- PadRight ile key boyutu sabitlenmeli
- Token doğrulama parametreleri doğru yapılandırılmalı

## RolesController Bağımlılık Sorunu (23.02.2024)

### Sorun:
```
Unable to resolve service for type 'StockAPI.Data.StockContext' while attempting to activate 'StockAPI.Controllers.RolesController'
```

### Nedeni:
1. RolesController yanlış context tipini kullanıyor
2. StockContext yerine ApplicationDbContext kullanılmalı
3. Dependency Injection yanlış yapılandırılmış

### Çözüm:
1. RolesController'da context tipini değiştir:
```csharp
private readonly ApplicationDbContext _context;

public RolesController(ApplicationDbContext context, ILogger<RolesController> logger)
{
    _context = context;
    _logger = logger;
}
```

### Önemli Notlar:
- Tüm controller'larda ApplicationDbContext kullanılmalı
- StockContext artık kullanılmıyor
- Dependency Injection için Program.cs'de sadece ApplicationDbContext kayıtlı

## Login Sayfası Geliştirmeleri (24.02.2024)

### Yapılan İyileştirmeler:
1. Modern UI/UX:
   - Animasyonlu arka plan
   - Glassmorphism efektleri
   - Neon yazı efektleri
   - Gradient butonlar
   - Özel form elemanları

2. Backend Entegrasyonu:
   - CORS politikası güncellendi
   - JWT token yönetimi optimize edildi
   - API endpoint'leri test edildi

3. Hata Yönetimi:
   - Toast bildirimleri eklendi
   - Form validasyonları güçlendirildi
   - API hata yakalama mekanizması geliştirildi

### Önemli Notlar:
- Login sayfası responsive tasarıma sahip
- Tüm animasyonlar performans optimize
- Güvenlik kontrolleri tamamlandı
- Cross-browser uyumluluğu test edildi

### Test Sonuçları:
- Chrome, Firefox ve Edge'de test edildi
- Mobil görünüm kontrol edildi
- API yanıt süreleri 200ms altında
- Memory leak tespit edilmedi
