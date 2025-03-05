# Login 401 Unauthorized Hatası ve Çözümü

## Sorun Tanımı

Frontend uygulamasında login işlemi sırasında aşağıdaki hata alındı:
```
POST http://localhost:5037/api/auth/login 401 (Unauthorized)
```

Kullanıcı, A001 sicil numarası ve admin123 şifresi ile giriş yapmaya çalışıyor, ancak 401 Unauthorized hatası alıyor. Bu, kimlik doğrulama işleminin başarısız olduğunu gösteriyor.

## Sorunun Nedenleri

Sorunun olası nedenleri şunlardır:

1. **Sicil numarası sorunu**: Kullanıcı sicil numarası veritabanında güncellenmiş, ancak şifre hash'i güncellenememiş olabilir.
2. **Şifre hash'i sorunu**: Şifre hash'i güncellenmiş, ancak hash algoritması veya parametreleri (örneğin work factor) değişmiş olabilir.
3. **Şifre doğrulama sorunu**: AuthService sınıfındaki login metodu, şifre doğrulama işlemini doğru şekilde yapamıyor olabilir.
4. **BCrypt kütüphanesi sorunu**: BCrypt kütüphanesi, şifre hash'leme ve doğrulama işlemlerini doğru şekilde yapamıyor olabilir.

## Sorunun Analizi

AuthService sınıfındaki login metodu incelendiğinde, kullanıcının sicil numarasına göre arandığı ve şifrenin doğrulandığı görüldü:

```csharp
public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
{
    _logger.LogInformation($"Giriş denemesi: {loginDto.Sicil}, Şifre uzunluğu: {loginDto.Password?.Length ?? 0}");
    
    var user = await _unitOfWork.Users.GetBySicilAsync(loginDto.Sicil);
    
    if (user == null)
    {
        _logger.LogWarning($"Kullanıcı bulunamadı: Sicil No: {loginDto.Sicil}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Geçersiz sicil numarası veya şifre"
        };
    }

    _logger.LogInformation($"Kullanıcı bulundu: Sicil No: {loginDto.Sicil}, ID: {user.Id}, IsAdmin: {user.IsAdmin}");
    _logger.LogInformation($"Şifre doğrulanıyor. Hash: {user.PasswordHash}");
    _logger.LogInformation($"Hash uzunluğu: {user.PasswordHash?.Length ?? 0}");
    _logger.LogInformation($"Hash formatı: {(user.PasswordHash?.StartsWith("$2") == true ? "BCrypt" : "Bilinmeyen")}");
    
    bool isPasswordValid = false;
    try
    {
        // IPasswordHasher arayüzünü kullanarak şifreyi doğrula
        isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
        _logger.LogInformation($"Şifre doğrulama sonucu: {isPasswordValid}");
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifre doğrulama hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Şifre doğrulama hatası"
        };
    }

    if (!isPasswordValid)
    {
        _logger.LogWarning($"Geçersiz şifre: Sicil No: {loginDto.Sicil}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Geçersiz sicil numarası veya şifre"
        };
    }

    var token = _tokenGenerator.GenerateToken(user);
    _logger.LogInformation($"Başarılı giriş: Sicil No: {loginDto.Sicil}, Token üretildi");

    return new AuthResponseDto
    {
        Success = true,
        Token = token,
        Username = user.Username,
        IsAdmin = user.IsAdmin,
        RoleName = user.Role?.Name
    };
}
```

PasswordHasher sınıfı incelendiğinde, şifre hash'leme ve doğrulama işlemlerinin BCrypt kütüphanesi kullanılarak yapıldığı görüldü:

```csharp
public class PasswordHasher : IPasswordHasher
{
    private readonly ILogger<PasswordHasher> _logger;
    private const int WORK_FACTOR = 11; // Sabit work factor

    public PasswordHasher(ILogger<PasswordHasher> logger)
    {
        _logger = logger;
    }

    public string HashPassword(string password)
    {
        try
        {
            _logger.LogInformation($"Şifre hash'leniyor... Work Factor: {WORK_FACTOR}");
            
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, WORK_FACTOR);
            
            _logger.LogInformation($"Şifre başarıyla hash'lendi. Hash: {hashedPassword}");
            _logger.LogInformation($"Hash formatı: {(hashedPassword.StartsWith("$2") ? "BCrypt" : "Bilinmeyen")}");
            _logger.LogInformation($"Hash uzunluğu: {hashedPassword.Length}");
            
            return hashedPassword;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Şifre hash'leme hatası: {ex.Message}");
            throw;
        }
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        try
        {
            _logger.LogInformation("Şifre doğrulanıyor...");
            _logger.LogInformation($"Hash formatı: {(passwordHash.StartsWith("$2") ? "BCrypt" : "Bilinmeyen")}");
            _logger.LogInformation($"Hash uzunluğu: {passwordHash.Length}");
            _logger.LogInformation($"Work Factor: {passwordHash.Split('$')[2]}");
            
            var result = BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
            
            _logger.LogInformation($"Şifre doğrulama sonucu: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Şifre doğrulama hatası: {ex.Message}");
            throw;
        }
    }
}
```

## Çözüm

Sorunun çözümü için, kullanıcı şifrelerini yeniden oluşturmak ve veritabanında güncellemek gerekiyordu. Bu amaçla, `FixPasswordController` sınıfına `fix-passwords` endpoint'i eklendi:

```csharp
[HttpGet("fix-passwords")]
public async Task<IActionResult> FixPasswords()
{
    try
    {
        _logger.LogInformation("Kullanıcı şifrelerini düzeltme işlemi başlatıldı");
        
        // Admin kullanıcısının şifresini düzelt
        var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
        if (adminUser != null)
        {
            var adminPassword = "admin123";
            adminUser.PasswordHash = _passwordHasher.HashPassword(adminPassword);
            _logger.LogInformation($"Admin kullanıcısının şifresi güncellendi. Yeni hash: {adminUser.PasswordHash}");
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("Admin kullanıcısı bulunamadı");
        }

        // Normal kullanıcının şifresini düzelt
        var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
        if (normalUser != null)
        {
            var userPassword = "user123";
            normalUser.PasswordHash = _passwordHasher.HashPassword(userPassword);
            _logger.LogInformation($"User kullanıcısının şifresi güncellendi. Yeni hash: {normalUser.PasswordHash}");
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogWarning("User kullanıcısı bulunamadı");
        }

        return Ok(new { 
            message = "Kullanıcı şifreleri başarıyla güncellendi.",
            adminInfo = "Kullanıcı adı: admin, Sicil: A001, Şifre: admin123",
            userInfo = "Kullanıcı adı: user, Sicil: U001, Şifre: user123"
        });
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifreler güncellenirken hata oluştu: {ex.Message}");
        return StatusCode(500, "Bir hata oluştu");
    }
}
```

Bu endpoint, admin ve normal kullanıcıların şifrelerini yeniden oluşturarak veritabanında güncelledi. Şifre hash'leme işlemi, PasswordHasher sınıfındaki HashPassword metodu kullanılarak yapıldı.

## Uygulama Adımları

1. Backend API başlatıldı.
2. Tarayıcıdan `http://localhost:5037/api/FixPassword/fix-passwords` adresine gidilerek kullanıcı şifreleri güncellendi.
3. Frontend uygulaması başlatıldı.
4. Aşağıdaki bilgilerle giriş yapıldı:
   - Admin: Sicil: A001, Şifre: admin123
   - User: Sicil: U001, Şifre: user123

## Öğrenilen Dersler

1. **Şifre hash'leme algoritması değişiklikleri**: Şifre hash'leme algoritması veya parametreleri değiştiğinde, mevcut şifre hash'lerinin güncellenmesi gerekir.
2. **Şifre doğrulama uyumluluğu**: Şifre doğrulama işlemi, hash algoritması ve parametreleri ile uyumlu olmalıdır.
3. **Kimlik doğrulama sorunlarının çözümü**: Kimlik doğrulama sorunlarını çözmek için, şifre hash'lerini yeniden oluşturmak etkili bir çözüm olabilir.
4. **PowerShell komut çalıştırma**: PowerShell'de komut çalıştırırken && operatörü yerine ayrı ayrı komutlar kullanılmalıdır.
5. **API'ye bağlanma**: API'ye bağlanırken curl komutu yerine Invoke-WebRequest komutu kullanılmalıdır.

## Gelecek İçin Öneriler

1. **Şifre hash'leme algoritması standardizasyonu**: Şifre hash'leme algoritması ve parametrelerinin standardize edilmesi ve değiştirilmemesi.
2. **Şifre doğrulama testleri**: Şifre doğrulama işleminin düzgün çalıştığından emin olmak için otomatik testler eklenmesi.
3. **Kimlik doğrulama loglama**: Kimlik doğrulama işlemlerinin detaylı olarak loglanması ve hata durumlarının izlenmesi.
4. **Şifre politikası**: Güçlü şifre politikası uygulanması ve kullanıcıların düzenli olarak şifrelerini değiştirmelerinin sağlanması.
5. **Şifre sıfırlama mekanizması**: Kullanıcıların şifrelerini unutmaları durumunda kullanabilecekleri güvenli bir şifre sıfırlama mekanizması eklenmesi. 