# Kimlik Doğrulama (Authentication)

## Genel Bakış
Bu belge, Stock uygulamasının kimlik doğrulama sistemini açıklar.

## Teknolojiler
- JWT (JSON Web Token) tabanlı kimlik doğrulama
- ASP.NET Core Identity
- SHA256 şifre hash'leme

## Kullanıcı Tipleri
1. Admin Kullanıcı
   - Tam yetkili
   - Kullanıcı yönetimi yapabilir
   - Varsayılan admin hesabı:
     - Kullanıcı adı: A001
     - Şifre: admin123

2. Normal Kullanıcı
   - Sınırlı yetkiler
   - Sadece kendine atanan işlemleri yapabilir

## API Endpoints

### 1. Giriş (/api/auth/login)
- Method: POST
- Request Body:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```
- Response:
  ```json
  {
    "token": "string",
    "user": {
      "id": "number",
      "username": "string",
      "isAdmin": "boolean"
    }
  }
  ```

### 2. İlk Admin Oluşturma (/api/auth/create-first-admin)
- Method: POST
- Request Body:
  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

### 3. Mevcut Kullanıcı Bilgisi (/api/auth/me)
- Method: GET
- Header: Authorization: Bearer {token}
- Response:
  ```json
  {
    "id": "number",
    "username": "string",
    "isAdmin": "boolean"
  }
  ```

## Güvenlik Önlemleri
1. Şifreler SHA256 ile hash'lenir
2. JWT token'ları 24 saat geçerli
3. Hassas bilgiler loglanmaz
4. CORS politikası sadece frontend'e izin verir

## Hata Durumları
1. Yanlış kullanıcı adı/şifre: 401 Unauthorized
2. Token eksik/geçersiz: 401 Unauthorized
3. Yetkisiz işlem: 403 Forbidden
4. Sunucu hatası: 500 Internal Server Error

## Bilinen Sorunlar ve Çözümleri
1. Migration Sorunları
   - Sorun: Seed data'da DateTime.UtcNow kullanımı
   - Çözüm: Sabit tarih kullanımı

2. Veritabanı Oluşturma
   - Sorun: Database.EnsureCreated() ve migrations çakışması
   - Çözüm: Sadece migrations kullanımı

## İyileştirme Önerileri
1. Şifre politikası güçlendirme
2. Oturum yönetimi geliştirme
3. Parola sıfırlama mekanizması
