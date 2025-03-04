# Sistem Başlatma Rehberi

Bu belge, Stock yönetim sisteminin başlatılması için gerekli adımları içermektedir.

## İçindekiler
- [Veritabanı Hazırlığı](#veritabanı-hazırlığı)
- [Backend Başlatma](#backend-başlatma)
- [Frontend Başlatma](#frontend-başlatma)
- [Sistem Testi](#sistem-testi)
- [Sorun Giderme](#sorun-giderme)

## Veritabanı Hazırlığı

### Veritabanı Temizleme
Eğer veritabanında sorun yaşanıyorsa, veritabanını tamamen temizleyip yeniden oluşturmak en etkili çözümdür:

```powershell
cd src/Stock.Infrastructure
dotnet ef database drop -f -s ../Stock.API/Stock.API.csproj
```

### Veritabanı Güncelleme
Veritabanını güncellemek için:

```powershell
cd src/Stock.Infrastructure
dotnet ef database update -s ../Stock.API/Stock.API.csproj
```

## Backend Başlatma

Backend'i başlatmak için:

```powershell
cd src/Stock.API
dotnet run
```

Backend başarıyla başlatıldığında, aşağıdaki URL'lerde erişilebilir olacaktır:
- API: http://localhost:5037
- Swagger UI: http://localhost:5037/swagger/index.html

## Frontend Başlatma

Frontend'i başlatmak için:

```powershell
cd frontend
npm run start -- --port=4202
```

Frontend başarıyla başlatıldığında, aşağıdaki URL'de erişilebilir olacaktır:
- http://localhost:4202

## Sistem Testi

Sistemin doğru çalıştığını test etmek için:

1. Swagger UI üzerinden API'yi test edin: http://localhost:5037/swagger/index.html
2. Frontend üzerinden login işlemini test edin: http://localhost:4202/login
   - Kullanıcı adı: admin
   - Şifre: admin123

## Sorun Giderme

### Backend Başlatma Sorunları

**Sorun:** Proje bulunamadı hatası
```
Çalıştırılacak bir proje bulunamadı. C:\Users\muham\OneDrive\Masaüstü\Stock içinde bir projenin bulunduğundan emin olun veya --project kullanarak yolu projeye geçirin.
```

**Çözüm:**
Doğru dizinde olduğunuzdan emin olun:
```powershell
cd src/Stock.API
```

### Frontend Başlatma Sorunları

**Sorun:** Angular CLI workspace dışında çalıştırılıyor
```
Error: This command is not available when running the Angular CLI outside a workspace.
```

**Çözüm:**
Frontend dizinine geçin:
```powershell
cd frontend
```

**Sorun:** npm start komutu çalışmıyor
```
npm error code ENOENT
npm error syscall open
npm error path C:\Users\muham\OneDrive\Masaüstü\Stock\package.json
```

**Çözüm:**
Frontend dizininde olduğunuzdan emin olun ve npm run start komutunu kullanın:
```powershell
cd frontend
npm run start
```

**Sorun:** Frontend derleme hatası - CreateUserRequest modelinde sicil alanı eksikliği
```
X [ERROR] TS2741: Property 'sicil' is missing in type '{ username: string; password: string; isAdmin: boolean; }' but required in type 'CreateUserRequest'.
```

**Çözüm:**
UserService içindeki createUser metodunda CreateUserRequest nesnesine sicil alanını ekleyin:
```typescript
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

**Sorun:** Port çakışması
```
Port 4200 is already in use.
```

**Çözüm:**
Farklı bir port numarası belirterek frontend'i başlatın:
```powershell
cd frontend
npm run start -- --port=4202
```

### Veritabanı Sorunları

**Sorun:** Migrations uygulanırken hata
```
An error occurred while using the connection to database 'StockDb' on server 'tcp://localhost:5432'
```

**Çözüm:**
1. PostgreSQL servisinin çalıştığından emin olun
2. Veritabanını temizleyip yeniden oluşturun:
```powershell
cd src/Stock.Infrastructure
dotnet ef database drop -f -s ../Stock.API/Stock.API.csproj
dotnet ef database update -s ../Stock.API/Stock.API.csproj
```

## Önemli Notlar

- Her servis için ayrı terminal penceresi kullanın
- Backend servisi 5037 portunda çalışır
- Frontend servisi 4202 portunda çalışır (port çakışması durumunda değiştirildi)
- Servisleri başlatmadan önce tüm portların müsait olduğundan emin olun
- Veritabanı işlemlerinde hata alındığında, veritabanını temizleyip yeniden oluşturmak etkili bir çözümdür
- Frontend modellerinin backend modelleriyle uyumlu olması gerekir
- API URL'sinin environment.ts dosyasında doğru yapılandırıldığından emin olun 