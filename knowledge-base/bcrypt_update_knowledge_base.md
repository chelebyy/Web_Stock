# BCrypt.Net-Next Güncellemesi Bilgi Tabanı

## Genel Bakış

Bu belge, Stock projesinde şifre hashleme ve doğrulama işlemleri için BCrypt.Net-Next paketinin güncellenmesi ve uygulanması sürecini belgelemektedir.

## Güncelleme Nedeni

Önceki şifre hashleme yöntemi (PBKDF2), güvenlik açısından yeterli olsa da, BCrypt algoritması şifre hashleme için daha modern ve güvenli bir alternatif sunmaktadır. BCrypt, özellikle brute-force saldırılarına karşı daha dirençlidir ve work factor parametresi sayesinde gelecekteki donanım gelişmelerine karşı ölçeklenebilir güvenlik sağlar.

## Yapılan Değişiklikler

### 1. Paket Güncellemesi

BCrypt.Net-Next paketi NuGet üzerinden projeye eklendi:

```bash
dotnet add package BCrypt.Net-Next
```

### 2. CreateAdminUser.cs Dosyası Güncellemesi

Admin kullanıcısı oluşturulurken kullanılan şifre hashleme yöntemi güncellendi:

```csharp
// Eski kod
var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123");

// Yeni kod
var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123", 13);
```

EnhancedHashPassword metodu, standart HashPassword metodundan farklı olarak UTF-8 karakter kodlamasını destekler ve Türkçe karakterlerin doğru şekilde işlenmesini sağlar. Work factor parametresi 13 olarak ayarlandı, bu değer güvenlik ve performans arasında iyi bir denge sağlar.

### 3. PasswordHasher Sınıfı Güncellemesi

PasswordHasher sınıfı, PBKDF2 algoritmasından BCrypt algoritmasına geçecek şekilde güncellendi:

```csharp
// Eski HashPassword metodu
public string HashPassword(string password)
{
    // Generate a 128-bit salt using a secure PRNG
    byte[] salt = new byte[128 / 8];
    using (var rng = RandomNumberGenerator.Create())
    {
        rng.GetBytes(salt);
    }

    // Derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: 10000,
        numBytesRequested: 256 / 8));

    // Format: {algorithm}${iterations}${salt}${hash}
    return $"PBKDF2${10000}${Convert.ToBase64String(salt)}${hashed}";
}

// Yeni HashPassword metodu
public string HashPassword(string password)
{
    // BCrypt.Net-Next kullanarak şifreyi hashle
    // Varsayılan olarak 13 work factor kullanılıyor
    return BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
}
```

```csharp
// Eski VerifyPassword metodu
public bool VerifyPassword(string password, string passwordHash)
{
    // Parse the hash
    var parts = passwordHash.Split('$');
    if (parts.Length != 4)
    {
        return false;
    }

    var algorithm = parts[0];
    var iterations = int.Parse(parts[1]);
    var salt = Convert.FromBase64String(parts[2]);
    var hash = parts[3];

    // Verify the hash
    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        password: password,
        salt: salt,
        prf: KeyDerivationPrf.HMACSHA256,
        iterationCount: iterations,
        numBytesRequested: 256 / 8));

    return hash == hashed;
}

// Yeni VerifyPassword metodu
public bool VerifyPassword(string password, string passwordHash)
{
    // BCrypt.Net-Next kullanarak şifreyi doğrula
    return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}
```

## Dikkat Edilmesi Gereken Noktalar

1. **Geriye Dönük Uyumluluk**: Bu güncelleme, mevcut kullanıcıların şifrelerinin yeniden hashlemesini gerektirir. Yeni kullanıcılar BCrypt ile hashlenirken, mevcut kullanıcılar giriş yaptıklarında şifreleri otomatik olarak BCrypt formatına dönüştürülmelidir.

2. **Work Factor**: Work factor değeri (13), güvenlik ve performans arasında denge sağlar. Gelecekte donanım gelişmelerine bağlı olarak bu değer artırılabilir.

3. **UTF-8 Desteği**: EnhancedHashPassword ve EnhancedVerify metotları, Türkçe karakterler gibi UTF-8 karakterlerini doğru şekilde işler.

## Test Sonuçları

- Admin kullanıcısı başarıyla oluşturuldu ve BCrypt ile hashlenen şifre ile giriş yapılabildi.
- Yeni kullanıcı kaydı ve giriş işlemleri sorunsuz çalışıyor.
- Türkçe karakterler içeren şifreler doğru şekilde hashlenip doğrulanabiliyor.

## Gelecek İyileştirmeler

- Şifre politikalarının güçlendirilmesi (minimum uzunluk, karmaşıklık gereksinimleri)
- Şifre sıfırlama işlemlerinin güncellenmesi
- İki faktörlü kimlik doğrulama (2FA) desteği eklenmesi

## Kaynaklar

- [BCrypt.Net-Next GitHub Deposu](https://github.com/BcryptNet/bcrypt.net)
- [BCrypt Algoritması Hakkında](https://en.wikipedia.org/wiki/Bcrypt)
- [OWASP Şifre Depolama Kılavuzu](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html) 