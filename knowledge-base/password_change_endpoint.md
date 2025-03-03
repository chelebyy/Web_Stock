# Şifre Değiştirme Endpoint'i Uygulaması

Bu belge, şifre değiştirme endpoint'inin uygulanması sürecini ve karşılaşılan sorunları belgelemektedir.

## Sorun

Clean Architecture geçişi sırasında, şifre değiştirme endpoint'i uygulanmamıştı. Bu nedenle, kullanıcılar şifrelerini değiştirmeye çalıştıklarında 404 Not Found hatası alıyorlardı.

Hata mesajı:
```
POST http://localhost:5037/api/auth/change-password 404 (Not Found)
```

## Çözüm Adımları

### 1. IAuthService Arayüzünün Güncellenmesi

IAuthService arayüzüne şifre değiştirme metodu eklendi:

```csharp
public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    string GenerateJwtToken(UserDto user);
    Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId);
}
```

### 2. ChangePasswordDto Sınıfının Oluşturulması

Şifre değiştirme isteği için gerekli DTO sınıfı oluşturuldu:

```csharp
public class ChangePasswordDto
{
    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır")]
    public string NewPassword { get; set; } = string.Empty;
}
```

### 3. AuthResponseDto Sınıfının Güncellenmesi

AuthResponseDto sınıfına başarılı işlemler için Message özelliği eklendi:

```csharp
public class AuthResponseDto
{
    public bool Success { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public string? RoleName { get; set; }
    public string? ErrorMessage { get; set; }
    public string? Message { get; set; }
}
```

### 4. AuthService Sınıfının Güncellenmesi

AuthService sınıfına şifre değiştirme metodunun uygulaması eklendi:

```csharp
public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
{
    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var user = await _unitOfWork.Users.GetByIdAsync(userId);
    if (user == null)
    {
        _logger.LogWarning($"Kullanıcı bulunamadı - ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Kullanıcı bulunamadı"
        };
    }

    bool isCurrentPasswordValid = false;
    try
    {
        isCurrentPasswordValid = _passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash);
        _logger.LogInformation($"Mevcut şifre doğrulama sonucu: {isCurrentPasswordValid}");
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

    if (!isCurrentPasswordValid)
    {
        _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = "Mevcut şifre hatalı"
        };
    }

    try
    {
        user.PasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
        await _unitOfWork.SaveChangesAsync();
        
        _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
        return new AuthResponseDto
        {
            Success = true,
            Message = "Şifre başarıyla değiştirildi"
        };
    }
    catch (Exception ex)
    {
        _logger.LogError($"Şifre değiştirme hatası: {ex.Message}, StackTrace: {ex.StackTrace}");
        return new AuthResponseDto
        {
            Success = false,
            ErrorMessage = $"Şifre değiştirme hatası: {ex.Message}"
        };
    }
}
```

### 5. AuthController'a Endpoint Eklenmesi

AuthController'a şifre değiştirme endpoint'i eklendi:

```csharp
[Authorize]
[HttpPost("change-password")]
public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
{
    _logger.LogInformation("Şifre değiştirme isteği başlatılıyor...");
    
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
    {
        _logger.LogWarning("Token'da kullanıcı ID'si bulunamadı veya geçersiz");
        return Unauthorized(new { message = "Geçersiz kullanıcı kimliği" });
    }

    _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
    
    var result = await _authService.ChangePasswordAsync(changePasswordDto, userId);
    
    if (!result.Success)
    {
        _logger.LogWarning($"Şifre değiştirme başarısız - Kullanıcı ID: {userId}, Hata: {result.ErrorMessage}");
        return BadRequest(result);
    }
    
    _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
    return Ok(result);
}
```

## Önemli Güvenlik Kontrolleri

1. **Kimlik Doğrulama**: Endpoint, [Authorize] attribute ile korunmaktadır.
2. **Kullanıcı Doğrulama**: Token'dan kullanıcı ID'si çıkarılarak, sadece kendi şifresini değiştirebilmesi sağlanmıştır.
3. **Mevcut Şifre Doğrulama**: Kullanıcının mevcut şifresi doğrulanarak, yetkisiz şifre değişikliği önlenmiştir.
4. **Şifre Güvenliği**: Yeni şifre, güvenli bir şekilde hash'lenerek veritabanına kaydedilmektedir.
5. **Detaylı Loglama**: Tüm işlemler detaylı bir şekilde loglanarak, güvenlik izleme sağlanmıştır.

## Sonuç

Bu değişikliklerle birlikte, kullanıcılar artık şifrelerini güvenli bir şekilde değiştirebilmektedir. Frontend uygulaması, mevcut API endpoint'ini kullanarak şifre değiştirme işlemini gerçekleştirebilir. 