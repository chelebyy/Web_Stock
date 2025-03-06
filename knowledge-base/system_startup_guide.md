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

### Veritabanı Sıfırlama

Veritabanı sorunları yaşıyorsanız, sıfırdan oluşturmak için:

```powershell
cd src/Stock.API
dotnet ef database drop --force
dotnet ef database update
```

### Kullanıcı Verilerini Düzeltme

Login sorunları yaşıyorsanız, kullanıcı verilerini düzeltmek için:

```powershell
# Backend çalışırken tarayıcıda aşağıdaki URL'i açın
http://localhost:5037/api/FixPassword/fix-passwords
```

Bu işlem sonrasında aşağıdaki bilgilerle giriş yapabilirsiniz:
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

6. **Önemli:** Kullanıcı verilerini düzelten endpoint'i çağırın:
```
http://localhost:5037/api/FixPassword/fix-passwords
```
Bu işlem, aşağıdaki düzeltmeleri gerçekleştirir:
   - Admin kullanıcısının şifresini ve sicil alanını düzeltir (admin/admin123, sicil: A001)
   - Normal kullanıcı hesaplarını kontrol eder, yoksa oluşturur, varsa günceller
   - Rol tanımlamalarını kontrol eder, eksikse oluşturur

7. **Yeni:** Eksik izinleri eklemek için aşağıdaki endpoint'i çağırın (Admin olarak giriş yaptıktan sonra):
```
http://localhost:5037/api/permissions/add-missing-permissions
```
Bu işlem, aşağıdaki izinleri kontrol eder ve eksikse ekler:
   - Pages.Revir.View (Revir sayfasını görüntüleme izni)
   - Pages.BilgiIslem.View (Bilgi İşlem sayfasını görüntüleme izni)
   - Yeni eklenen diğer sayfa izinleri

8. Frontend'i başlatın: 
```powershell
cd frontend
npm install  # Sadece gerekiyorsa (bağımlılıklar değiştiyse)
ng serve --port 4202
```

9. Tarayıcıdan uygulamaya erişin ve login olun: 
```
http://localhost:4202
```

10. **Yeni:** Kullanıcı izinlerini güncellemek için:
    - Admin olarak giriş yapın
    - Kullanıcı Yönetimi > Kullanıcılar sayfasına gidin
    - İlgili kullanıcıyı seçin ve "İzinleri Düzenle" butonuna tıklayın
    - Kullanıcıya erişim vermek istediğiniz sayfaların izinlerini (örn. Pages.Revir.View, Pages.BilgiIslem.View) seçin ve kaydedin

Bu adımlar, farklı ortamlar arasında geçiş yaparken oluşabilecek veritabanı, kullanıcı kimlik doğrulama ve izin sorunlarını önleyecektir. 