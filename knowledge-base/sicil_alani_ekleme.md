# Sicil Alanı Ekleme İşlemi

Bu belge, kullanıcı modeline Sicil alanı ekleme işleminin detaylarını içermektedir.

## Genel Bakış

Kullanıcı yönetimi ekranında, kullanıcıların email alanı yerine Sicil numarası gösterilmesi istenmiştir. Bu değişiklik, backend ve frontend tarafında yapılan güncellemelerle gerçekleştirilmiştir.

## Backend Değişiklikleri

### User Modeli Güncellemesi

```csharp
public class User : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Sicil { get; set; } = string.Empty;

    public bool IsAdmin { get; set; }

    public DateTime? LastLoginAt { get; set; }

    // Role ilişkisi
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }
}
```

### ApplicationDbContext Güncellemesi

```csharp
// User yapılandırması
modelBuilder.Entity<User>(entity =>
{
    entity.HasKey(e => e.Id);
    entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
    entity.Property(e => e.PasswordHash).IsRequired();
    entity.Property(e => e.Sicil).IsRequired().HasMaxLength(20);
    entity.Property(e => e.CreatedAt).IsRequired();
    entity.HasOne(e => e.Role)
        .WithMany(r => r.Users)
        .HasForeignKey(e => e.RoleId)
        .OnDelete(DeleteBehavior.Restrict);
});
```

### SeedData Güncellemesi

```csharp
var adminUser = new User
{
    Id = 1,
    Username = "admin",
    PasswordHash = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", // admin123
    Sicil = "A001",
    IsAdmin = true,
    RoleId = adminRole.Id,
    CreatedAt = seedTime
};
```

### Migration Oluşturma ve Veritabanı Güncelleme

```bash
dotnet ef migrations add AddSicilToUser --context ApplicationDbContext
dotnet ef database update --context ApplicationDbContext
```

## Frontend Değişiklikleri

### User Model Güncellemesi

```typescript
export interface User {
    id?: number;
    username: string;
    passwordHash?: string;
    password?: string;
    sicil: string;
    isAdmin: boolean;
    createdAt: Date;
    lastLoginAt?: Date;
    roleId?: number;
    role?: Role;
}

export interface CreateUserDto {
    username: string;
    password: string;
    sicil: string;
    passwordHash?: string;
    isAdmin: boolean;
    roleId?: number;
    createdAt?: Date;
}

export interface UpdateUserDto {
    username?: string;
    password?: string;
    sicil?: string;
    isAdmin?: boolean;
    roleId?: number;
}
```

### Kullanıcı Yönetimi Bileşeni HTML Güncellemesi

```html
<div class="header-cell sicil-cell">Sicil</div>
...
<div class="row-cell sicil-cell">{{ user.sicil }}</div>
...
<input type="text" id="sicil" pInputText formControlName="sicil" 
  placeholder="Sicil numarası girin">
<small *ngIf="submitted && f['sicil'].errors?.['required']" class="p-error">
  Sicil numarası gereklidir.
</small>
```

### Kullanıcı Yönetimi Bileşeni TypeScript Güncellemesi

```typescript
initForm() {
  this.userForm = this.formBuilder.group({
    id: [null],
    sicil: ['', [Validators.required]],
    // ...
  });
}
```

### Kullanıcı Verilerinin Görüntülenmesi Sorunu ve Çözümü

**Sorun:** Kullanıcı yönetimi sayfasında, backend'den gelen kullanıcı verileri görüntülenmiyordu. Bunun nedeni, bileşenin sadece localStorage'dan veri yüklemesi ve backend API'ye bağlanmamasıydı.

**Çözüm:** UserService'i kullanarak backend'den kullanıcı verilerini çekmek için loadUsers metodu güncellendi:

```typescript
loadUsers() {
  // Önce localStorage'dan kullanıcıları yüklemeyi deneyelim
  const storedUsers = localStorage.getItem('users');
  
  if (storedUsers) {
    this.users = JSON.parse(storedUsers);
    this.filteredUsers = [...this.users];
    this.updatePagination();
  }
  
  // Backend'den kullanıcıları çekelim
  this.userService.getUsers().subscribe({
    next: (users) => {
      if (users && users.length > 0) {
        // Backend'den gelen kullanıcıları formatlayalım
        this.users = users.map(user => ({
          id: user.id,
          sicil: user.sicil,
          fullName: user.username,
          permissions: user.isAdmin ? 'Admin' : user.roleId ? 'Contributor' : 'Viewer',
          isActive: true,
          joinDate: new Date(user.createdAt).toLocaleDateString('tr-TR', { year: 'numeric', month: 'long', day: 'numeric' }),
          avatar: 'default-avatar.png'
        }));
        
        // Kullanıcıları localStorage'a kaydedelim
        localStorage.setItem('users', JSON.stringify(this.users));
        
        this.filteredUsers = [...this.users];
        this.updatePagination();
      } else {
        console.log('Backend\'den kullanıcı verisi alınamadı veya boş.');
      }
    },
    error: (error) => {
      console.error('Kullanıcıları yükleme hatası:', error);
      this.messageService.add({
        severity: 'error',
        summary: 'Hata',
        detail: 'Kullanıcılar yüklenirken bir hata oluştu.',
        life: 3000
      });
    }
  });
}
```

Bu değişiklik, hem localStorage'dan hem de backend API'den kullanıcı verilerini yükleyerek, kullanıcı yönetimi sayfasında verilerin doğru şekilde görüntülenmesini sağlar.

### API Endpoint Hatası ve Çözümü

**Sorun:** Kullanıcı yönetimi sayfasında, backend API'ye yapılan isteklerde 404 Not Found hatası alınıyordu:
```
GET http://localhost:5037/api/user 404 (Not Found)
```

**Nedeni:**
1. Frontend'de UserService içinde API endpoint'i küçük harfle 'user' olarak tanımlanmıştı
2. Backend'de controller sınıfı 'UserController' olarak tanımlanmıştı (büyük harfle başlayan)
3. .NET Core'da route'lar varsayılan olarak büyük/küçük harf duyarlıdır

**Çözüm:**
UserService içindeki API endpoint'lerini büyük harfle başlayacak şekilde güncelledik:

```typescript
// Önceki kod
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(`${this.apiUrl}/user`);
}

// Düzeltilmiş kod
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(`${this.apiUrl}/User`);
}
```

Aynı şekilde diğer endpoint'leri de güncelledik:
- `${this.apiUrl}/user/${id}` → `${this.apiUrl}/User/${id}` (updateUser)
- `${this.apiUrl}/user/${id}` → `${this.apiUrl}/User/${id}` (deleteUser)

Bu değişiklik, frontend'in backend API'ye doğru endpoint'lerle istek yapmasını sağlayarak, kullanıcı verilerinin başarıyla çekilmesini sağlar.

### API URL Hatası ve Çözümü

**Sorun:** Kullanıcı yönetimi sayfasında, API endpoint'i düzeltilmesine rağmen hala 404 Not Found hatası alınıyordu:
```
GET http://localhost:5037/api/User 404 (Not Found)
```

**Nedeni:**
1. Backend API'nin doğru çalışıp çalışmadığı kontrol edilmemişti
2. Backend API'nin doğru portta çalışıp çalışmadığı kontrol edilmemişti
3. Backend API'nin doğru controller'a sahip olup olmadığı kontrol edilmemişti

**Çözüm:**
1. Backend API'nin çalıştığından emin olduk
2. Backend API'nin 5037 portunda çalıştığını doğruladık
3. Backend API'nin UserController'a sahip olduğunu doğruladık

Bu değişiklik, frontend'in doğru port üzerinden backend API'ye istek yapmasını sağlayarak, kullanıcı verilerinin başarıyla çekilmesini sağlar.

## Önemli Notlar

- Sicil alanı string tipinde ve zorunlu bir alan olarak tanımlandı
- Maksimum uzunluğu 20 karakter olarak belirlendi
- Admin kullanıcısı için varsayılan Sicil değeri "A001" olarak atandı
- Frontend tarafında email alanları sicil olarak güncellendi
- Sicil alanı için email validasyonu kaldırıldı, sadece required validasyonu bırakıldı
- Kullanıcı verilerinin görüntülenmesi için UserService entegrasyonu yapıldı
- API endpoint'lerinin büyük/küçük harf duyarlılığına dikkat edilmelidir
- Clean Architecture geçişi sırasında API URL'leri ve portları değişebilir

## Karşılaşılan Sorunlar ve Çözümleri

1. **PowerShell'de && operatörü çalışmaması**
   - Sorun: PowerShell'de && operatörü desteklenmemektedir
   - Çözüm: Komutlar ayrı ayrı çalıştırıldı

2. **Birden fazla DbContext olması**
   - Sorun: Migration oluşturulurken hangi context'in kullanılacağı belirtilmedi
   - Çözüm: `--context ApplicationDbContext` parametresi eklendi 

3. **Kullanıcı verilerinin görüntülenmemesi**
   - Sorun: Kullanıcı yönetimi bileşeni sadece localStorage'dan veri yüklüyordu
   - Çözüm: UserService kullanılarak backend API'den veri çekilmesi sağlandı

4. **API Endpoint Hatası**
   - Sorun: Frontend'de API endpoint'i küçük harfle 'user' olarak tanımlanmıştı
   - Çözüm: API endpoint'leri büyük harfle başlayacak şekilde 'User' olarak güncellendi

5. **API URL Hatası**
   - Sorun: Backend API'nin doğru çalışıp çalışmadığı kontrol edilmemişti
   - Çözüm: Backend API'nin 5037 portunda çalıştığını ve UserController'a sahip olduğunu doğruladık