# Login Sayfası Knowledge Base

## Geliştirme Süreci

### 1. Component Yapısı
- Standalone Angular component
- PrimeNG Card, Input ve Button componentleri kullanıldı
- Reactive Forms ile form yönetimi

### 2. Karşılaşılan Hatalar ve Çözümleri

#### Hata 1: API URL Sorunu
**Sorun:** Frontend'deki AuthService, API URL'sini yanlış porta yönlendiriyordu.

**Çözüm:** AuthService içindeki API URL'si `http://localhost:5037/api` olarak güncellendi.

```typescript
private apiUrl = 'http://localhost:5037/api';
```

Backend'in launchSettings.json dosyasında doğru port ayarları:

```json
"profiles": {
  "http": {
    "commandName": "Project",
    "dotnetRunMessages": true,
    "launchBrowser": true,
    "launchUrl": "swagger",
    "applicationUrl": "http://localhost:5037",
    "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Development"
    }
  }
}
```

Backend'de CORS ayarları frontend'in 4200 portuna izin verecek şekilde yapılandırıldı.

#### Hata 2: Token Storage
**Sorun:** Login sonrası token'ın localStorage'da saklanması ve yönetimi

**Çözüm:**
1. AuthService içinde token yönetimi merkezi hale getirildi
2. Token decode işlemi için JwtHelperService kullanıldı

#### Hata 3: Şifre Hash Formatı Uyumsuzluğu

### Sorun
Seed data'da kullanılan şifre hash'i formatı ile `PasswordHasher` sınıfında kullanılan format arasında uyumsuzluk vardı. Seed data'da BCrypt formatında dinamik olarak oluşturulan hash değeri, her migrasyon uygulandığında farklı bir değer oluşturuyordu.

### Çözüm
1. `ApplicationDbContext.cs` dosyasında seed data için sabit bir BCrypt hash değeri kullanıldı:
```csharp
PasswordHash = "$2a$11$ij4DH2oHJIgakH3QZS94Uu1/jandJqMjWZtQB8B5.JoK4zrBd2Lhu", // Sabit BCrypt hash değeri "Admin123" için
```

2. Yeni bir migrasyon oluşturuldu ve veritabanı güncellendi:
```bash
dotnet ef migrations add FixedPasswordHash
dotnet ef database update
```

### Önemli Notlar
- Entity Framework Core'da seed data içinde dinamik değerler kullanmak (örn. `BCrypt.Net.BCrypt.HashPassword("Admin123")`) her seferinde modelin değişmesine neden olur ve "PendingModelChangesWarning" hatası alınır.
- Seed data için sabit değerler kullanmak bu sorunu çözer.
- Kullanıcı adı: `admin`, şifre: `Admin123`

### Hata Mesajı
```
System.InvalidOperationException: Unable to resolve service for type 'Stock.Application.Services.IAuthService' while attempting to activate 'Stock.API.Controllers.AuthController'.
```

### Nedeni
Bu hata, `IAuthService` arayüzünün bağımlılık enjeksiyon konteynerine kaydedilmemesinden kaynaklanmaktadır. Clean Architecture yapısında, her katmanın kendi `DependencyInjection` sınıfı vardır ve bu sınıfların `Program.cs` dosyasında çağrılması gerekir.

### Çözüm
`Program.cs` dosyasında Application ve Infrastructure katmanlarının DependencyInjection sınıflarının çağrıldığından emin olun:

```csharp
// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

### Önemli Notlar
1. Clean Architecture'da her katmanın kendi DependencyInjection sınıfı olmalıdır.
2. Bu sınıflar `Program.cs` dosyasında mutlaka çağrılmalıdır.
3. Bağımlılıkların doğru sırada kaydedilmesi önemlidir. Önce Application, sonra Infrastructure katmanı kaydedilmelidir.
4. Infrastructure katmanındaki `DependencyInjection.cs` dosyasında `IAuthService` arayüzünün `AuthService` sınıfına bağlandığından emin olun:
   ```csharp
   services.AddScoped<IAuthService, AuthService>();
   ```

### Kontrol Listesi
- [x] Program.cs dosyasında AddApplication() ve AddInfrastructure() metotları çağrılmış mı?
- [x] Infrastructure katmanında IAuthService kaydı var mı?
- [x] Uygulama yeniden başlatıldı mı?
- [x] Entity Framework paket sürümleri arasında çakışma var mı? (Uyarı olarak görünebilir)

### 3. Güvenlik Önlemleri

1. **Input Validasyonu:**
   - Boş kullanıcı adı ve şifre kontrolü
   - Minimum şifre uzunluğu kontrolü

2. **Hata Yönetimi:**
   - Backend'den gelen hata mesajlarının kullanıcıya gösterilmesi
   - Generic hata mesajları ile hassas bilgilerin gizlenmesi

3. **Token Yönetimi:**
   - JWT token'ın güvenli şekilde saklanması
   - Token expire olduğunda otomatik logout

### 4. Kullanıcı Deneyimi

1. **Loading State:**
   - Login işlemi sırasında loading indicator gösterimi
   - Butonun disabled edilmesi

2. **Hata Gösterimi:**
   - PrimeNG Toast component'i ile hata mesajları
   - Kullanıcı dostu hata açıklamaları

3. **Yönlendirme:**
   - Başarılı login sonrası dashboard'a yönlendirme
   - 1 saniyelik delay ile smooth transition

### 5. Best Practices

1. **Form Yönetimi:**
   - Reactive Forms kullanımı
   - Form state ve validasyon yönetimi

2. **Servis Mimarisi:**
   - AuthService ile authentication logic'in merkezi yönetimi
   - Observable pattern kullanımı

3. **Error Handling:**
   - Try-catch blokları ile hata yönetimi
   - Kullanıcı dostu hata mesajları

### 6. Gelecek Geliştirmeler İçin Notlar

1. **Güvenlik İyileştirmeleri:**
   - Password strength kontrolü
   - Rate limiting implementasyonu
   - CAPTCHA entegrasyonu

2. **UX İyileştirmeleri:**
   - "Beni Hatırla" özelliği
   - Şifremi unuttum fonksiyonu
   - Social login entegrasyonu

## Genel Bilgiler
- Kullanıcı girişi `/login` rotasında gerçekleşir
- Varsayılan admin kullanıcısı:
  - Kullanıcı adı: `admin`
  - Şifre: `Admin123!`

## Güvenlik
- Şifreler SHA256 ile hashlenir (hexadecimal format)
- JWT token kullanılarak kimlik doğrulama yapılır
- Token'lar localStorage'da saklanır

## Karşılaşılan Sorunlar ve Çözümleri

### 1. CORS Hatası
**Sorun:** Frontend'den backend'e yapılan isteklerde CORS hatası alındı.
**Çözüm:**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

// Middleware sıralaması önemli
app.UseCors("AllowAngularApp");
```

### 2. Şifre Hashleme Uyumsuzluğu
**Sorun:** AuthController ve ApplicationDbContext'te farklı hash formatları kullanıldı (Base64 vs Hexadecimal).
**Çözüm:** Tüm uygulamada hexadecimal formatı standardize edildi:
```csharp
private string HashPassword(string password)
{
    using (SHA256 sha256Hash = SHA256.Create())
    {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}
```

### 3. Veritabanı Sıfırlama
**Sorun:** Hash uyumsuzluğu nedeniyle veritabanındaki kullanıcı şifreleri geçersiz hale geldi.
**Çözüm:**
1. Veritabanı sıfırlandı: `dotnet ef database drop --force`
2. Yeniden oluşturuldu: `dotnet ef database update`
3. Seed data ile admin kullanıcısı otomatik oluşturuldu

**Önemli Not:** Seed data'da kullanılan şifre formatı ile PasswordHasher sınıfında kullanılan format arasında uyumsuzluk var. ApplicationDbContext'teki seed data'da PBKDF2 formatında bir hash kullanılırken, PasswordHasher sınıfı BCrypt kullanıyor. Bu nedenle, seed data ile oluşturulan kullanıcılar ile giriş yapılamıyor.

**Çözüm:**
1. Veritabanını sıfırla: `dotnet ef database drop --force`
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
3. Veritabanını yeniden oluştur: `dotnet ef database update`

**Not:** Eğer seed data güncellenemiyorsa, manuel olarak bir admin kullanıcısı oluşturmak için register endpoint'ini kullanabilirsiniz:
```
POST http://localhost:5037/api/auth/register
{
  "username": "Admin",
  "password": "Admin123",
  "confirmPassword": "Admin123",
  "isAdmin": true
}
```

### 4. Bağımlılık Enjeksiyon Hatası
**Sorun:** Login işlemi sırasında `System.InvalidOperationException: Unable to resolve service for type 'Stock.Application.Services.IAuthService' while attempting to activate 'Stock.API.Controllers.AuthController'` hatası alındı.

**Nedeni:** Program.cs dosyasında Infrastructure ve Application katmanlarındaki DependencyInjection sınıfları çağrılmamıştı.

**Çözüm:**
Program.cs dosyasına Infrastructure ve Application katmanlarındaki DependencyInjection sınıflarını ekledik:
```csharp
using Stock.Application;
using Stock.Infrastructure;

// ...

// Add Application and Infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
```

**Önemli Notlar:**
- Clean Architecture'da her katmanın kendi DependencyInjection sınıfı olmalı
- Program.cs dosyasında bu sınıflar çağrılmalı
- Bağımlılıklar doğru sırayla kaydedilmeli (önce Application, sonra Infrastructure)
- Servis kayıtları eksik olduğunda, controller'lar oluşturulurken bağımlılık çözümleme hataları alınabilir

## Önemli Noktalar
1. CORS ayarları middleware sıralamasında en üstte olmalı
2. Şifre hashleme yöntemi tüm uygulamada aynı olmalı
3. Hata mesajları güvenlik için genel tutulmalı ("Kullanıcı adı veya şifre hatalı")
4. Token'lar güvenli bir şekilde saklanmalı ve taşınmalı

## Test Senaryoları
1. Doğru kullanıcı adı ve şifre ile giriş
2. Yanlış kullanıcı adı ile giriş
3. Yanlış şifre ile giriş
4. Token'ın geçerliliğini yitirmesi durumu
5. CORS politikasının doğru çalışması

### Hata 3: Entity Framework Core Paket Sürüm Uyumsuzluğu

**Sorun:** Stock.API projesi ile Infrastructure projesi arasında Entity Framework Core paketlerinin sürüm uyumsuzluğu vardı.

**Hata Mesajı:**
```
System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.EntityFrameworkCore.Relational, Version=9.0.2.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'.
```

**Çözüm:** Stock.API projesindeki Entity Framework Core paketleri, Infrastructure projesinde kullanılan sürümlerle uyumlu hale getirildi:

1. Microsoft.EntityFrameworkCore paketini 9.0.2 sürümüne yükseltme
2. Microsoft.EntityFrameworkCore.Relational paketini 9.0.2 sürümüne yükseltme
3. Npgsql.EntityFrameworkCore.PostgreSQL paketini 9.0.4 sürümüne yükseltme

Paket sürümlerini güncelledikten sonra, çalışan API sürecini durdurup yeniden derleyerek sorunu çözdük.

## Öğrenilen Dersler

1. Farklı projelerde kullanılan paket sürümlerinin uyumlu olduğundan emin olunmalıdır.
2. Hata mesajlarını dikkatlice inceleyerek kök nedeni tespit etmek önemlidir.
3. Çalışan süreçlerin dosyaları kilitleyebileceğini ve derleme hatalarına neden olabileceğini unutmamak gerekir.
