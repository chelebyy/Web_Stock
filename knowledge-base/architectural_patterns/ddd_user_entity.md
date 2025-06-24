# User Entity için Domain-Driven Design (DDD) Uygulaması

Bu belge, Stock projesinde `User` entity'sine Domain-Driven Design (DDD) prensiplerinin nasıl uygulandığını ve uygulama sürecinde alınan kararları açıklamaktadır.

## 1. Genel Bakış

`User` entity'si için DDD prensiplerini uygularken, Value Object'ler yerine daha pragmatik bir yaklaşım tercih edildi. Bu yaklaşım, Entity Framework Core ile daha iyi uyum sağlarken DDD'nin temel prensiplerini (encapsulation, davranış odaklı tasarım, iş kurallarını zorunlu kılma) koruyor.

## 2. Başlangıç Durumu ve Tasarım Kararları

Başlangıçta, Product entity'sinde olduğu gibi User için de Value Object'ler (`FirstName`, `LastName`, `Sicil` gibi) oluşturulması planlanmıştı. Ancak, User entity'sinin çeşitli modüllerle yoğun entegrasyonu, authentication süreçleri ve veritabanı ilişkileri göz önünde bulundurulduğunda, Value Object'ler yerine primitive tipler ile DDD prensiplerini koruyarak daha pragmatik bir yaklaşım tercih edildi.

Bu kararın başlıca nedenleri:

1. EF Core entegrasyonunda yaşanabilecek karmaşıklığı azaltmak
2. Auth süreçlerindeki mevcut kod tabanıyla uyumu korumak
3. User entity'sinin çok sayıda ilişki ve bağımlılık içermesi
4. Var olan sorgulama mekanizmalarının korunması

## 3. Uygulanan DDD Prensipleri

User entity'sinde aşağıdaki DDD prensipleri uygulandı:

### 3.1. Entity Encapsulation

Entity'nin iç durumu korundu ve değişiklikler davranış metotları (behavior methods) aracılığıyla yapılacak şekilde encapsulation sağlandı:

```csharp
// Örnek: Property tanımları
public string Adi { get; private set; } = string.Empty;
public string Soyadi { get; private set; } = string.Empty;
public string Sicil { get; private set; } = string.Empty;
```

### 3.2. Domain Davranışları

Entity'nin iç durumunu değiştiren metotlar ve iş kuralları encapsule edildi:

```csharp
// Örnek davranış metotları
public Result UpdateName(string newAdi, string newSoyadi)
{
    if (string.IsNullOrWhiteSpace(newAdi))
    {
        return Result.Failure(DomainErrors.User.AdiEmpty);
    }

    if (string.IsNullOrWhiteSpace(newSoyadi))
    {
        return Result.Failure(DomainErrors.User.SoyadiEmpty);
    }

    Adi = newAdi;
    Soyadi = newSoyadi;
    return Result.Success();
}

public Result UpdateSicil(string newSicil)
{
    if (string.IsNullOrWhiteSpace(newSicil))
    {
        return Result.Failure(DomainErrors.User.SicilEmpty);
    }
    Sicil = newSicil;
    return Result.Success();
}

public Result ChangePassword(string newPasswordHash)
{
    if (string.IsNullOrWhiteSpace(newPasswordHash))
    {
        return Result.Failure(DomainErrors.User.PasswordHashEmpty);
    }
    PasswordHash = newPasswordHash;
    return Result.Success();
}

public Result AssignRole(int? roleId)
{
    RoleId = roleId;
    return Result.Success();
}

public Result SetAdminStatus(bool isAdmin)
{
    IsAdmin = isAdmin;
    return Result.Success();
}

public Result UpdateLastLoginTimestamp()
{
    LastLoginAt = DateTime.UtcNow;
    return Result.Success();
}
```

### 3.3. Factory Metotları

Entity oluşturma işlemi factory metotları ile kontrollü bir şekilde yapıldı:

```csharp
public static Result<User> Create(
    string adi, 
    string soyadi, 
    string sicil, 
    string passwordHash,
    int? roleId = null, 
    bool isAdmin = false)
{
    // Validasyon kontrolleri
    if (string.IsNullOrWhiteSpace(adi))
    {
        return Result<User>.Failure(DomainErrors.User.AdiEmpty);
    }

    if (string.IsNullOrWhiteSpace(soyadi))
    {
        return Result<User>.Failure(DomainErrors.User.SoyadiEmpty);
    }

    if (string.IsNullOrWhiteSpace(sicil))
    {
        return Result<User>.Failure(DomainErrors.User.SicilEmpty);
    }

    if (string.IsNullOrWhiteSpace(passwordHash))
    {
        return Result<User>.Failure(DomainErrors.User.PasswordHashEmpty);
    }

    var user = new User(adi, soyadi, sicil, passwordHash, roleId, isAdmin);
    return Result<User>.Success(user);
}
```

### 3.4. İş Kuralları

Domain hataları `DomainErrors` sınıfında tanımlandı ve metotların dönüş değerleri `Result` pattern ile encapsule edildi:

```csharp
// DomainErrors sınıfı örneği
public static class User
{
    public const string SicilEmpty = "User.SicilEmpty: Sicil cannot be empty.";
    public static string SicilAlreadyExists(string sicil) => $"User.SicilAlreadyExists: Sicil '{sicil}' is already taken.";
    public const string AdiEmpty = "User.AdiEmpty: User name (Adi) cannot be empty.";
    public const string SoyadiEmpty = "User.SoyadiEmpty: User surname (Soyadi) cannot be empty.";
    public static string NotFound(int id) => $"User.NotFound: User with Id '{id}' not found.";
    public const string PasswordHashEmpty = "User.PasswordHashEmpty: Password hash cannot be empty.";
}
```

## 4. Domain Olayları (Gelecekte Uygulanabilir)

User entity'si içinde olaylar (events) henüz uygulanmadı, ancak kullanıcı kayıt, giriş, şifre değiştirme gibi olaylarda kullanılabilir:

```csharp
// Gelecekte eklenebilecek domain olayları
public class UserCreatedEvent : DomainEvent
{
    public int UserId { get; }
    public string Sicil { get; }

    public UserCreatedEvent(int userId, string sicil)
    {
        UserId = userId;
        Sicil = sicil;
    }
}
```

## 5. Entity Framework Core Entegrasyonu

User entity'sinin Entity Framework Core ile uyumlu çalışması için gerekli yapılandırmalar `UserConfiguration` sınıfında yapıldı:

```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Adi).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Soyadi).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Sicil).HasMaxLength(50).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
        
        builder.HasIndex(x => x.Sicil).IsUnique();

        builder.HasOne(x => x.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(x => x.RoleId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        // İlişkiler
    }
}
```

## 6. CQRS ve Command Handler'lar

Uygulama katmanında, User entity'si ile etkileşim için Command ve Query'ler kullanıldı. Bu handler'lar güncellenerek User entity'sinin factory ve davranış metotlarını kullanır hale getirildi.

### 6.1. CreateUserCommandHandler

```csharp
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    // ...

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInfo(LogMessages.CreatingUser, request.Sicil);

            // Sicil kontrolü
            var existingUserSpecSicil = new UserBySicilSpecification(request.Sicil, includeRole: false);
            var existingUserBySicil = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpecSicil, cancellationToken);
            
            if (existingUserBySicil != null)
            {
                throw DuplicateEntityException.Create("User", "Sicil", request.Sicil);
            }

            // Password hashing
            var passwordHash = _passwordHasher.HashPassword(request.Password);
            
            // User entity'sini oluştur (Factory metodu kullanarak)
            var userResult = User.Create(
                request.Adi, 
                request.Soyadi, 
                request.Sicil,
                passwordHash,
                request.RoleId,
                request.IsAdmin);

            if (!userResult.IsSuccess)
            {
                throw new BadRequestException(userResult.Error);
            }

            var newUser = userResult.Value;
            await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(newUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ErrorCreatingUser, request.Sicil);
            throw;
        }
    }
}
```

### 6.2. UpdateUserCommandHandler

```csharp
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    // ...

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInfo(LogMessages.UpdatingUser, request.Id);

            var existingUserSpec = new EntityByIdSpecification<User>(request.Id);
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpec, cancellationToken);
            
            if (existingUser == null)
            {
                throw new NotFoundException(LogMessages.UserNotFoundById, request.Id);
            }

            // Sicil değiştirilmişse kontrol et
            if (existingUser.Sicil != request.Sicil)
            {
                var userSpecSicil = new UserBySicilSpecification(request.Sicil, includeRole: false);
                var userBySicil = await _unitOfWork.Users.FirstOrDefaultAsync(userSpecSicil, cancellationToken);
                
                if (userBySicil != null && userBySicil.Id != request.Id)
                {
                    throw DuplicateEntityException.Create("User", "Sicil", request.Sicil);
                }
                
                // Sicil güncelleme (davranış metodu)
                var sicilResult = existingUser.UpdateSicil(request.Sicil);
                if (!sicilResult.IsSuccess)
                {
                    throw new BadRequestException(sicilResult.Error);
                }
            }
            
            // Ad/Soyad güncelleme (davranış metodu)
            var nameResult = existingUser.UpdateName(request.Adi, request.Soyadi);
            if (!nameResult.IsSuccess)
            {
                throw new BadRequestException(nameResult.Error);
            }
            
            // Admin durumu güncelleme (davranış metodu)
            existingUser.SetAdminStatus(request.IsAdmin);
            
            // Rol güncelleme (davranış metodu)
            existingUser.AssignRole(request.RoleId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(existingUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ErrorUpdatingUser, request.Id);
            throw;
        }
    }
}
```

### 6.3. ChangePasswordCommandHandler

```csharp
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    // ...

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInfo(LogMessages.ChangePasswordAttempt, request.UserId);

            var userSpec = new EntityByIdSpecification<User>(request.UserId);
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(userSpec, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(LogMessages.UserNotFoundById, request.UserId);
            }

            // Mevcut şifre kontrolü
            if (!_passwordHasher.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                throw new BadRequestException(ErrorMessages.InvalidCurrentPassword);
            }

            // Şifre hash'leme
            var newPasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            
            // Şifre değiştirme (davranış metodu)
            var passwordResult = user.ChangePassword(newPasswordHash);
            if (!passwordResult.IsSuccess)
            {
                throw new BadRequestException(passwordResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogMessages.ErrorChangingPassword, request.UserId);
            throw;
        }
    }
}
```

## 7. Service Katmanı Entegrasyonu

User entity'si ile ilgili servislerin (UserService, AuthService) DDD prensiplerine uygun olarak güncellenmesi yapıldı.

### 7.1. UserService

```csharp
// UserService güncelleme örneği
public async Task<User> UpdateUserAsync(int id, User user)
{
    try
    {
        _logger.LogInfo(LogMessages.UpdatingUser, id);

        var existingUserSpec = new EntityByIdSpecification<User>(id);
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(existingUserSpec);
        if (existingUser == null)
        {
            _logger.LogWarn(LogMessages.UserNotFoundById, id);
            throw new NotFoundException(LogMessages.UserNotFoundById, id);
        }

        // Sicil değiştiyse kontrol et
        if (existingUser.Sicil != user.Sicil)
        {
            var userSpecSicil = new UserBySicilSpecification(user.Sicil, includeRole: false);
            var userBySicil = await _unitOfWork.Users.FirstOrDefaultAsync(userSpecSicil);
            
            if (userBySicil != null && userBySicil.Id != id)
            {
                _logger.LogWarn(LogMessages.DuplicateSicil, user.Sicil);
                throw DuplicateEntityException.Create("User", "Sicil", user.Sicil);
            }
            
            // Sicil güncelleme (davranış metodu)
            var sicilResult = existingUser.UpdateSicil(user.Sicil);
            if (!sicilResult.IsSuccess)
            {
                throw new BadRequestException(sicilResult.Error);
            }
        }
        
        // Ad/Soyad güncelleme (davranış metodu)
        var nameResult = existingUser.UpdateName(user.Adi, user.Soyadi);
        if (!nameResult.IsSuccess)
        {
            throw new BadRequestException(nameResult.Error);
        }
        
        // Admin durumu güncelleme (davranış metodu)
        existingUser.SetAdminStatus(user.IsAdmin);
        
        // Rol güncelleme (davranış metodu)
        existingUser.AssignRole(user.RoleId);

        await _unitOfWork.SaveChangesAsync();
        _logger.LogInfo(LogMessages.UserUpdated, id);

        return existingUser;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, LogMessages.ErrorUpdatingUser, id);
        throw;
    }
}
```

### 7.2. AuthService 

```csharp
// AuthService şifre değiştirme örneği
public async Task<AuthResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto, int userId)
{
    _logger.LogInfo(LogMessages.ChangePasswordAttempt, userId);
    
    var spec = new EntityByIdSpecification<User>(userId);
    var user = await _unitOfWork.Users.FirstOrDefaultAsync(spec);
    
    if (user == null)
    {
        _logger.LogWarn(LogMessages.UserNotFoundById, userId);
        return new AuthResponseDto
        {
            Success = false,
            Message = string.Format(ErrorMessages.UserNotFoundById, userId)
        };
    }
    
    // Mevcut şifre kontrolü
    if (!_passwordHasher.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
    {
        _logger.LogWarn(LogMessages.InvalidPassword, userId);
        return new AuthResponseDto
        {
            Success = false,
            Message = ErrorMessages.InvalidCurrentPassword
        };
    }
    
    // Yeni şifre hash'leme
    var newPasswordHash = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
    
    // Şifre değiştirme (davranış metodu)
    var passwordResult = user.ChangePassword(newPasswordHash);
    if (!passwordResult.IsSuccess)
    {
        _logger.LogWarn(LogMessages.InvalidDomainOperation, passwordResult.Error);
        return new AuthResponseDto
        {
            Success = false,
            Message = passwordResult.Error
        };
    }
    
    await _unitOfWork.SaveChangesAsync();
    
    _logger.LogInfo(LogMessages.PasswordChanged, userId);
    return new AuthResponseDto
    {
        Success = true,
        Message = SuccessMessages.PasswordChanged
    };
}
```

## 8. Domain Exception Sınıfları

DDD yaklaşımında domain exception'ların özel olarak ele alınması önemlidir. Bu amaçla DuplicateEntityException sınıfı eklendi:

```csharp
public class DuplicateEntityException : Exception
{
    public DuplicateEntityException() : base("Varlık zaten mevcut.")
    {
    }

    public DuplicateEntityException(string message) : base(message)
    {
    }

    public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public static DuplicateEntityException Create(string entityName, string propertyName, string value)
    {
        return new DuplicateEntityException($"{entityName} entity'si için aynı {propertyName} değerine ({value}) sahip kayıt zaten mevcut.");
    }
}
```

## 9. Öğrenilen Dersler

1. **DDD ve EF Core Entegrasyonu:** EF Core ile DDD prensiplerini uygularken, pure domain modelinden biraz ödün vermek gerekebilir. Özellikle boş constructor ve navigation property'ler gibi konularda.

2. **Pragmatik DDD:** Her zaman tam Value Object yapısı kullanmak gereksiz karmaşıklık getirebilir. Bazen primitive tipler ile daha pragmatik bir DDD yaklaşımı, kodun anlaşılırlığını ve bakım kolaylığını artırabilir.

3. **Result Pattern:** Entity metotlarının `Result<T>` veya `Result` dönmesi, hata yönetimini daha açık ve tip güvenli hale getirir.

4. **Specification Pattern:** User ve ilişkili entity'lerin sorgulanması için specification pattern, karmaşık sorguların encapsulation'ını sağlar.

5. **Domain Exception'lar:** Özel domain exception'lar tanımlamak, hatalarla ilgili daha anlamlı bilgiler sağlar ve hata yönetimini iyileştirir.

6. **Davranış Metotları:** Property'lerin direkt değiştirilmesi yerine davranış metotları kullanmak, iş kurallarının entity içinde zorunlu kılınmasını ve tutarlılığı sağlar.

7. **Service Katmanı Entegrasyonu:** Service katmanındaki metotların, domain entity'lerinin property'lerini doğrudan değiştirmesi yerine davranış metotlarını çağırması, DDD prensiplerine daha uygundur.

## 10. Sonraki Adımlar

- Domain Events implementasyonu
- Daha kapsamlı validasyon kuralları
- Role ve Permission aggregate'leri ile daha güçlü bağlantılar
- Test coverage'ının artırılması
- Diğer entity'ler için benzer DDD prensiplerinin uygulanması

## 11. Sonuç

`User` entity'sine pragmatik bir DDD yaklaşımı uygulandı. Value Object'ler yerine primitive tipler kullanılmasına rağmen, encapsulation, factory metotları, domain davranışları ve validasyon kuralları ile DDD'nin temel faydaları korundu. Bu yaklaşım, EF Core ile daha iyi entegrasyon ve mevcut kodla daha iyi uyum sağladı.

Uygulama katmanında (CQRS Command Handler'lar) ve servis katmanında (UserService, AuthService) gerekli güncellemeler yapılarak, entity'nin domain davranışlarının kullanılması sağlandı. Ayrıca, domain exception'lar tanımlanarak daha domain odaklı hata yönetimi gerçekleştirildi.

Bu değişiklikler, kodun daha iyi encapsule edilmesini, iş kurallarının merkezi bir şekilde yönetilmesini ve domain modelinin daha zengin olmasını sağladı. 