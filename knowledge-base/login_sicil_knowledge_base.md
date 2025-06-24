# Login İşleminde Sicil Numarası Kullanımı

## Yapılan Değişiklik Özeti

Login işlemi kullanıcı adı yerine sicil numarası kullanacak şekilde değiştirildi. Bu değişiklik, tüm frontend ve backend bileşenlerinde kullanıcı adına yapılan referansların sicil numarasına dönüştürülmesini içerdi.

## Değiştirilen Dosyalar

### Frontend

1. **Models**
   - `frontend/src/app/models/auth.model.ts`: LoginRequest arayüzünde "username" yerine "sicil" alanı eklendi
   - `frontend/src/app/models/user.model.ts`: User arayüzünde sicil alanı güncellendi

2. **Services**
   - `frontend/src/app/services/auth.service.ts`: Login metodu sicil numarası kullanacak şekilde güncellendi

3. **Komponentler**
   - `frontend/src/app/components/login/login.component.ts`: Bileşendeki username değişkeni sicil olarak değiştirildi ve ilgili metodlar güncellendi
   - `frontend/src/app/components/login/login.component.html`: Kullanıcı adı giriş alanı sicil numarası alanı olarak değiştirildi, etiketler güncellendi

### Backend

1. **DTO**
   - `LoginDto` sınıfı: Username alanı yerine Sicil alanı tanımlandı

2. **Services**
   - `AuthService`: Kullanıcı doğrulama işlemi sicil numarası kullanılarak yapılacak şekilde güncellendi

3. **Repository**
   - `UserRepository`: GetBySicilAsync metodu eklenerek sicil numarası ile kullanıcı bulma işlemi eklendi

## Yapılan Değişiklik Detayları

### 1. LoginRequest Arayüzünde Değişiklik

```typescript
// Önceki hali
export interface LoginRequest {
  username: string;
  password: string;
}

// Yeni hali
export interface LoginRequest {
  sicil: string;
  password: string;
}
```

### 2. Login Komponenti Değişikliği

```typescript
// Önceki hali
username: string = '';
password: string = '';

// Yeni hali
sicil: string = '';
password: string = '';

// Hata mesajı değişikliği
// Önceki hali
if (!this.username || !this.password) {
  this.messageService.add({
    severity: 'error',
    summary: 'Hata',
    detail: 'Kullanıcı adı ve şifre gereklidir!',
    life: 3000
  });
  return;
}

// Yeni hali
if (!this.sicil || !this.password) {
  this.messageService.add({
    severity: 'error',
    summary: 'Hata',
    detail: 'Sicil numarası ve şifre gereklidir!',
    life: 3000
  });
  return;
}
```

### 3. Login Ekranı HTML Değişikliği

```html
<!-- Önceki hali -->
<input 
  type="text" 
  pInputText
  id="username"
  [(ngModel)]="username"
  name="username"
  placeholder="Kullanıcı adı"
  class="glass-input"
  required>

<!-- Yeni hali -->
<input 
  type="text" 
  pInputText
  id="sicil"
  [(ngModel)]="sicil"
  name="sicil"
  placeholder="Sicil Numarası"
  class="glass-input"
  required>
```

### 4. AuthService Değişikliği

```typescript
// Önceki login metodu
login(loginRequest: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest);
}

// Yeni login metodu - Login isteğinde sicil parametresi kullanılıyor
login(loginRequest: LoginRequest): Observable<LoginResponse> {
  return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, loginRequest);
}
```

### 5. Backend LoginDto Değişikliği

```csharp
// Önceki hali
public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}

// Yeni hali
public class LoginDto
{
    [Required]
    public string Sicil { get; set; } = string.Empty;
    
    [Required]
    public string Password { get; set; } = string.Empty;
}
```

### 6. Backend Authentication Service Değişikliği

```csharp
// Önceki hali - Username ile kullanıcı doğrulama
public async Task<Result<string>> LoginAsync(LoginDto loginDto)
{
    var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
    if (user == null)
    {
        return Result.Failure<string>("Kullanıcı bulunamadı");
    }

    // Şifre doğrulama işlemi...
}

// Yeni hali - Sicil numarası ile kullanıcı doğrulama
public async Task<Result<string>> LoginAsync(LoginDto loginDto)
{
    var user = await _userRepository.GetBySicilAsync(loginDto.Sicil);
    if (user == null)
    {
        return Result.Failure<string>("Kullanıcı bulunamadı");
    }

    // Şifre doğrulama işlemi...
}
```

## Dikkat Edilmesi Gerekenler

1. Login işlemi artık kullanıcı adı yerine sicil numarası ile yapılıyor
2. Kullanıcı oluşturma ve güncelleme işlemlerinde sicil numarası alanının zorunlu olması sağlandı
3. Hata mesajlarında "kullanıcı adı" yerine "sicil numarası" ifadesi kullanıldı
4. Frontend tarafında login işlemi için istek formatı değiştiği için API endpoint parametreleri buna göre güncellendi

## Potansiyel Sorunlar ve Çözümleri

### 1. Eski kullanıcı hesaplarına erişim

Eski kullanıcı hesaplarının sicil numarası alanı boş olabilir ve bu durumda giriş yapılamayabilir.

**Çözüm:** Tüm mevcut kullanıcıların sicil numaraları kontrol edilmeli ve gerekiyorsa güncellenmelidir.

### 2. UserRepository eksik metodu

Sicil numarası ile kullanıcı arama metodu olmadığı durumlarda hata alınabilir.

**Çözüm:** UserRepository sınıfına GetBySicilAsync metodu eklendi.

### 3. Hatalı alan isimleri

Login formunda yanlış alan adı kullanılması durumunda veri gönderilmeyebilir.

**Çözüm:** Login bileşeninde tüm kullanıcı adı alanları sicil alanı ile değiştirildi. İstek ve yanıt modellerinin doğru şekilde güncellendiğinden emin olundu. 