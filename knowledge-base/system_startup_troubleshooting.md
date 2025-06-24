# Sistem Başlatma ve Sorun Giderme Bilgi Tabanı

Bu belge, Stock yönetim sisteminin başlatılması sırasında karşılaşılan sorunlar ve çözümleri hakkında detaylı bilgiler içermektedir.

## İçindekiler
- [Frontend Derleme Hataları](#frontend-derleme-hataları)
- [Backend Başlatma Sorunları](#backend-başlatma-sorunları)
- [Port Yapılandırması](#port-yapılandırması)
- [API Erişim Sorunları](#api-erişim-sorunları)
- [Veritabanı Bağlantı Sorunları](#veritabanı-bağlantı-sorunları)

## Frontend Derleme Hataları

### CreateUserRequest Modeli Sicil Alanı Eksikliği

**Hata:**
```
X [ERROR] TS2741: Property 'sicil' is missing in type '{ username: string; password: string; isAdmin: boolean; }' but required in type 'CreateUserRequest'.
```

**Nedeni:**
1. UserService içindeki createUser metodunda CreateUserRequest nesnesine sicil alanı eklenmemiş
2. CreateUserRequest modelinde sicil alanı zorunlu olarak tanımlanmış

**Çözüm:**
UserService içindeki createUser metodunda CreateUserRequest nesnesine sicil alanını ekle:
```typescript
// frontend/src/app/services/user.service.ts
createUser(user: User): Observable<any> {
  const createUserRequest: CreateUserRequest = {
    username: user.username,
    password: user.passwordHash || '',
    sicil: user.sicil,
    isAdmin: user.isAdmin
  };
  return this.http.post<any>(`${this.apiUrl}/auth/create-user`, createUserRequest);
}
```

**Teknik Detaylar:**
- CreateUserRequest modeli auth.model.ts dosyasında tanımlanmıştır
- Sicil alanı zorunlu bir alan olarak tanımlanmıştır
- UserService içindeki createUser metodunda bu alanın eklenmesi gerekir

## Backend Başlatma Sorunları

### API Endpoint Bulunamadı Hatası

**Hata:**
```
Invoke-WebRequest : Uzak sunucu hata döndürdü: (404) Bulunamadı.
```

**Nedeni:**
1. API endpoint'i mevcut değil
2. API endpoint'i yanlış yazılmış
3. Backend servisi henüz tam olarak başlatılmamış

**Çözüm:**
1. Backend servisinin tam olarak başlatılmasını bekleyin
2. Doğru endpoint'i kullanın
3. Swagger UI üzerinden mevcut endpoint'leri kontrol edin: http://localhost:5037/swagger/index.html

**Teknik Detaylar:**
- Backend servisi başlatıldığında konsolda "Now listening on: http://localhost:5037" mesajı görünmelidir
- Swagger UI üzerinden mevcut endpoint'leri kontrol edebilirsiniz
- API endpoint'lerinin büyük/küçük harf duyarlı olduğunu unutmayın

## Port Yapılandırması

### Port Çakışması Sorunu

**Hata:**
```
Port 4200 is already in use.
```

**Nedeni:**
1. 4200 portu başka bir uygulama tarafından kullanılıyor
2. Önceki Angular uygulaması düzgün kapatılmamış

**Çözüm:**
1. Farklı bir port numarası belirterek frontend'i başlatın:
```powershell
cd frontend
npm run start -- --port=4202
```

2. Portu kullanan işlemi sonlandırın:
```powershell
# Windows
netstat -ano | findstr :4200
taskkill /PID <PID> /F

# Linux/macOS
lsof -i :4200
kill -9 <PID>
```

**Teknik Detaylar:**
- Angular CLI, --port parametresi ile farklı bir port numarası belirtmenize olanak tanır
- Frontend servisinin varsayılan portu 4200'dür
- Backend servisinin portu launchSettings.json dosyasında tanımlanmıştır (5037)

## API Erişim Sorunları

### Kimlik Doğrulama Hatası

**Hata:**
```
Invoke-WebRequest : Uzak sunucu hata döndürdü: (401) Onaylanmadı.
```

**Nedeni:**
1. Endpoint kimlik doğrulama gerektiriyor
2. JWT token eksik veya geçersiz

**Çözüm:**
1. Önce login endpoint'i üzerinden token alın
2. İsteğe Authorization header'ı ekleyin:
```powershell
$token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
Invoke-WebRequest -Uri http://localhost:5037/api/users -Headers @{Authorization = "Bearer $token"} -UseBasicParsing
```

**Teknik Detaylar:**
- API'nin çoğu endpoint'i kimlik doğrulama gerektirir
- JWT token, login işlemi sonrasında alınabilir
- Token'ın geçerlilik süresi sınırlıdır (varsayılan: 1 saat)

## Veritabanı Bağlantı Sorunları

### Veritabanı Bağlantı Hatası

**Hata:**
```
An error occurred while using the connection to database 'StockDb' on server 'tcp://localhost:5432'
```

**Nedeni:**
1. PostgreSQL servisi çalışmıyor
2. Veritabanı kullanıcı adı veya şifre yanlış
3. Veritabanı mevcut değil

**Çözüm:**
1. PostgreSQL servisinin çalıştığından emin olun:
```powershell
# Windows
sc query postgresql-x64-17
# veya
Get-Service -Name postgresql-x64-17

# Linux
systemctl status postgresql
```

2. Veritabanını temizleyip yeniden oluşturun:
```powershell
cd src/Stock.Infrastructure
dotnet ef database drop -f -s ../Stock.API/Stock.API.csproj
dotnet ef database update -s ../Stock.API/Stock.API.csproj
```

**Teknik Detaylar:**
- Veritabanı bağlantı bilgileri appsettings.json dosyasında tanımlanmıştır
- PostgreSQL varsayılan olarak 5432 portunda çalışır
- Veritabanı adı: StockDb

## Önemli Notlar ve En İyi Uygulamalar

1. **Servis Başlatma Sırası:**
   - Önce veritabanı servisinin çalıştığından emin olun
   - Sonra backend servisini başlatın
   - En son frontend servisini başlatın

2. **Hata Ayıklama İpuçları:**
   - Backend konsolunda detaylı hata mesajlarını kontrol edin
   - Frontend konsolunda (F12 ile tarayıcı geliştirici araçları) hataları kontrol edin
   - API isteklerini ve yanıtlarını tarayıcı Network sekmesinde izleyin

3. **Sistem Durumu Kontrolü:**
   - Backend: http://localhost:5037/api/health (404 hatası alınması normaldir, bu endpoint mevcut değil)
   - Frontend: http://localhost:4202
   - Swagger UI: http://localhost:5037/swagger/index.html

4. **Yaygın Sorunlar ve Çözümleri:**
   - Frontend modellerinin backend modelleriyle uyumlu olması gerekir
   - API URL'sinin environment.ts dosyasında doğru yapılandırıldığından emin olun
   - CORS hatalarında backend'in doğru origin'lere izin verdiğinden emin olun
   - JWT token süresi dolduğunda yeniden login yapılması gerekir 