# Sistem Başlatma Rehberi

Bu rehber, stok yönetim sisteminin doğru şekilde başlatılması için adım adım talimatları içerir.

## İçindekiler
- [Ön Koşullar](#ön-koşullar)
- [Veritabanı Kurulumu](#veritabanı-kurulumu)
- [Backend Başlatma](#backend-başlatma)
- [Frontend Başlatma](#frontend-başlatma)
- [Sorun Giderme](#sorun-giderme)

## Ön Koşullar

Sistemin çalışması için aşağıdaki yazılımların yüklü olması gerekmektedir:

- .NET Core 9 SDK
- PostgreSQL 17.3 veya üzeri 
- Node.js 20.x veya üzeri
- Angular CLI 19.x

## Veritabanı Kurulumu

1. PostgreSQL servisinin çalıştığından emin olun.
2. `appsettings.json` dosyasındaki veritabanı bağlantı dizesini kontrol edin.
3. Yeni bir veritabanı oluşturmak veya mevcut veritabanını güncellemek için:

```powershell
# Stock.API klasörüne gidin
cd src/Stock.API

# Veritabanını sıfırdan oluşturmak için
dotnet ef database drop --force
dotnet ef database update
```

## Backend Başlatma

```powershell
# Stock.API klasörüne gidin
cd src/Stock.API

# Uygulamayı başlatın
dotnet run
```

Backend uygulama varsayılan olarak http://localhost:5037 adresinde çalışır. Swagger arayüzüne http://localhost:5037/swagger adresinden erişebilirsiniz.

## Frontend Başlatma

```powershell
# Frontend klasörüne gidin
cd frontend

# Bağımlılıkları yükleyin (sadece ilk kez veya bağımlılıklar güncellendiğinde)
npm install

# Uygulamayı başlatın
ng serve --port 4202
```

Frontend uygulama http://localhost:4202 adresinde çalışır.

## Sorun Giderme

### GitHub Senkronizasyonu Sonrası Login Sorunu

GitHub'dan son değişiklikleri çektikten sonra login yapılamıyorsa (401 Unauthorized hatası), kullanıcı şifreleri veya sicil alanları ile ilgili bir sorun olabilir. Bu durumda:

```powershell
# Backend'i başlatın
cd src/Stock.API
dotnet run

# Tarayıcınızda aşağıdaki URL'i açın veya Postman ile GET isteği gönderin
http://localhost:5037/api/FixPassword/fix-passwords
```

Bu endpoint, kullanıcı şifrelerini ve sicil alanlarını otomatik olarak düzeltecektir. İşlem tamamlandığında aşağıdaki kullanıcı bilgileriyle giriş yapabilirsiniz:

- Admin Kullanıcı:
  - Kullanıcı adı: admin
  - Sicil: A001
  - Şifre: admin123

- Normal Kullanıcı:  
  - Kullanıcı adı: user
  - Sicil: U001
  - Şifre: user123

### Migration Hataları

Eğer "pending changes" veya "add a new migration" şeklinde hatalar alıyorsanız:

```powershell
cd src/Stock.Infrastructure
dotnet ef migrations add NewMigration --startup-project ../Stock.API
cd ../Stock.API
dotnet ef database update
```

### Backend ve Frontend Port Çakışmaları

Port çakışması durumunda, backend için:

```powershell
dotnet run --urls=http://localhost:5038
```

Frontend için:

```powershell
ng serve --port 4203
```

## Ortamlar Arası Geçiş (Ev ve İş)

Farklı ortamlar (ev ve iş) arasında geçiş yaparken, GitHub senkronizasyonu sonrası şu adımları izleyin:

1. Önce değişiklikleri çekin: `git pull`
2. Backend projesine gidin: `cd src/Stock.API`
3. Veritabanı güncellemesini kontrol edin: `dotnet ef database update --verbose`
4. Backend'i başlatın: `dotnet run`
5. Kullanıcı şifrelerini düzeltin: `http://localhost:5037/api/FixPassword/fix-passwords` (tarayıcıda açın)
6. Frontend'i başlatın: `cd frontend && ng serve --port 4202`

Bu adımlar, farklı ortamlar arasında geçiş yaparken oluşabilecek veritabanı ve kullanıcı kimlik doğrulama sorunlarını önleyecektir. 