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
