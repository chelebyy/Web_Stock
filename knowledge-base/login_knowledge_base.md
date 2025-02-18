# Login Sayfası Knowledge Base

## Geliştirme Süreci

### 1. Component Yapısı
- Standalone Angular component
- PrimeNG Card, Input ve Button componentleri kullanıldı
- Reactive Forms ile form yönetimi

### 2. Karşılaşılan Hatalar ve Çözümleri

#### Hata 1: API URL Sorunu
**Sorun:** Backend'e istek atılırken yanlış port kullanımı

**Çözüm:**
1. AuthService'deki API URL'i `http://localhost:5126/api` olarak güncellendi
2. Backend'de CORS ayarları frontend portu (55075) için güncellendi

#### Hata 2: Token Storage
**Sorun:** Login sonrası token'ın localStorage'da saklanması ve yönetimi

**Çözüm:**
1. AuthService içinde token yönetimi merkezi hale getirildi
2. Token decode işlemi için JwtHelperService kullanıldı

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
