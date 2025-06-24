# Sistem Başlatma ve Durdurma Rehberi

Bu rehber, stok yönetim sisteminin Windows ortamında başlatılması ve durdurulması için basit komutları içerir.

## İçindekiler
- [Ön Koşullar](#ön-koşullar)
- [Veritabanı Kurulumu](#veritabanı-kurulumu)
- [Servisleri Başlatma](#servisleri-başlatma)
- [Servisleri Durdurma](#servisleri-durdurma)
- [Sorun Giderme](#sorun-giderme)
- [Ortamlar Arası Geçiş (Ev ve İş)](#ortamlar-arası-geçiş-ev-ve-iş)

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

## Servisleri Başlatma

### Backend Başlatma

```powershell
# Stock.API klasörüne gidin
cd src/Stock.API

# Uygulamayı başlatın
dotnet run
```

Backend uygulama varsayılan olarak http://localhost:5037 adresinde çalışır.

### Frontend Başlatma

```powershell
# Frontend klasörüne gidin
cd frontend

# Uygulamayı başlatın
ng serve --port 4202
```

Frontend uygulama http://localhost:4202 adresinde çalışır.

## Servisleri Durdurma

### Backend Durdurma

```powershell
# Çalışan .NET Core uygulamalarını bulmak için
Get-Process -Name dotnet | Where-Object {$_.MainWindowTitle -like "*Stock.API*"} | Format-Table Id, ProcessName, MainWindowTitle

# Belirli bir ID'ye sahip işlemi durdurmak için
Stop-Process -Id <işlem_id>

# Alternatif olarak, tüm dotnet işlemlerini durdurmak için (dikkatli kullanın)
# Get-Process -Name dotnet | Stop-Process
```

### Frontend Durdurma

```powershell
# Çalışan Node.js işlemlerini bulmak için
Get-Process -Name node | Format-Table Id, ProcessName

# Angular CLI işlemlerini bulmak için
Get-Process | Where-Object {$_.MainWindowTitle -like "*ng serve*"} | Format-Table Id, ProcessName, MainWindowTitle

# Belirli bir ID'ye sahip işlemi durdurmak için
Stop-Process -Id <işlem_id>

# Alternatif olarak, tüm node işlemlerini durdurmak için (dikkatli kullanın)
# Get-Process -Name node | Stop-Process
```

### Tüm Servisleri Tek Komutla Durdurma

```powershell
# Hem dotnet hem de node işlemlerini durdurmak için
Get-Process -Name dotnet, node | Stop-Process -Force
```

## Sorun Giderme

### Port Çakışması Durumunda

Backend için farklı port kullanma:

```powershell
dotnet run --urls=http://localhost:5038
```

Frontend için farklı port kullanma:

```powershell
ng serve --port 4203
```

### Çalışan Portları Kontrol Etme

```powershell
# Belirli bir portu dinleyen işlemleri bulmak için
netstat -ano | findstr :5037
netstat -ano | findstr :4202

# Bulunan PID'ye sahip işlemi görmek için
Get-Process -Id <PID>
```

### Login Sorunlarını Giderme ve Varsayılan Kullanıcılar

Login sorunları yaşıyorsanız veya kullanıcı bilgilerini sıfırlamak istiyorsanız aşağıdaki adımları izleyebilirsiniz:

1.  **Veritabanını Sıfırlama ve Yeniden Oluşturma (Temiz Başlangıç):**
    Bu işlem, tüm mevcut verileri siler ve veritabanını başlangıç durumuna getirir. Geliştirme ortamında takılmalar yaşandığında veya kullanıcı verilerinde tutarsızlık olduğunda önerilir.
    ```powershell
    # Backend API'yi durdurun
    # src/Stock.API klasörüne gidin
    cd src/Stock.API
    dotnet ef database drop --force
    dotnet ef database update
    # Backend API'yi yeniden başlatın
    dotnet run
    ```
    Bu işlem sonrasında aşağıdaki varsayılan kullanıcı bilgileriyle giriş yapabilirsiniz:
    *   **Admin:** Sicil: `00000`, Şifre: `Admin123!` (Bu şifre `20250531143027_UpdateAdminPasswordFinal` migrasyonu ile ayarlanmıştır.)
    *   **Kullanıcı:** Sicil: `U001`, Şifre: `user123` (Eğer seed data ile bu kullanıcı hala oluşturuluyorsa geçerlidir.)

2.  **JWT Token Süresi Sorunu (IDX12401 Hatası):**
    Eğer giriş başarılı gibi görünüp "IDX12401: Expires must be after NotBefore" hatası alıyorsanız, sorun JWT token oluşturma yapılandırmasındadır.
    *   `src/Stock.API/appsettings.json` dosyasında `"Jwt": { "ExpirationHours": 24 }` gibi bir ayarın olduğundan emin olun.
    *   `src/Stock.Infrastructure/Services/JwtTokenGenerator.cs` dosyasındaki token oluşturma mantığının bu `ExpirationHours` değerini doğru okuyup kullandığını kontrol edin (saat cinsinden okuyup `AddHours` ile eklemeli).

### Veritabanı Sıfırlama

Veritabanı sorunları yaşıyorsanız, sıfırdan oluşturmak için:

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Kullanıcı Verilerini Düzeltme

Bu bölümdeki `http://localhost:5037/api/FixPassword/fix-passwords` endpoint'i şu anda aktif olmayabilir. Login sorunları için yukarıdaki "Login Sorunlarını Giderme ve Varsayılan Kullanıcılar" bölümüne başvurun.

Eski bilgiler (referans için saklanıyor, güncel olmayabilir):
```powershell
# Backend çalışırken tarayıcıda aşağıdaki URL'i açın
# http://localhost:5037/api/FixPassword/fix-passwords 
```
Bu işlem sonrasında aşağıdaki bilgilerle giriş yapabilirsiniz (güncel olmayabilir):
- Admin: sicil: A001, şifre: admin123
- Kullanıcı: sicil: U001, şifre: user123

## Ortamlar Arası Geçiş (Ev ve İş)

Farklı ortamlar (ev ve iş) arasında geçiş yaparken, GitHub senkronizasyonu sonrası şu adımları izleyin:

1. Önce değişiklikleri çekin: 
```powershell
git pull
```

2. Backend projesine gidin: 
```powershell
cd src/Stock.API
```

3. Veritabanı güncellemesini kontrol edin: 
```powershell
dotnet ef database update --verbose
```

4. Hata almanız durumunda, ortamlar arası geçiş sorunu yaşıyorsanız temiz başlangıç yapabilirsiniz:
```powershell
# Veritabanını tamamen yeniden oluşturun
dotnet ef database drop --force
dotnet ef database update
```

5. Backend'i başlatın: 
```powershell
dotnet run
```

6. **Önemli:** Kullanıcı verilerini düzeltmek için yukarıdaki "Login Sorunlarını Giderme ve Varsayılan Kullanıcılar" bölümündeki adımları izleyin. `http://localhost:5037/api/FixPassword/fix-passwords` endpoint'i çalışmıyor olabilir. Gerekirse veritabanını sıfırlayıp `dotnet ef database update` ile varsayılan kullanıcıların (özellikle Admin: Sicil `00000`, Şifre `Admin123!`) oluştuğundan emin olun.

7. **Yeni:** Eksik izinleri eklemek için aşağıdaki endpoint'i çağırın (Admin olarak giriş yaptıktan sonra):
```