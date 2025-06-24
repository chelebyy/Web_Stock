# Permission Entity DDD Düzenlemeleri

Bu belge, Stock projesindeki Permission (İzin) entity'sinin Domain-Driven Design (DDD) prensiplerine göre yeniden düzenlenmesini açıklamaktadır.

## Yapılan Değişiklikler

### 1. Domain Errors Sınıfına Eklemeler

`DomainErrors.cs` dosyasına Permission entity'si için hata kodları eklenmiştir:

```csharp
public static class Permission
{
    public const string NameEmpty = "Permission.NameEmpty: Permission name cannot be empty.";
    public const string NameTooLong = "Permission.NameTooLong: Permission name cannot exceed 100 characters.";
    public const string DescriptionTooLong = "Permission.DescriptionTooLong: Permission description cannot exceed 200 characters.";
    public const string ResourceTypeTooLong = "Permission.ResourceTypeTooLong: ResourceType cannot exceed 50 characters.";
    public const string GroupTooLong = "Permission.GroupTooLong: Group cannot exceed 50 characters.";
    public static string NotFound(int id) => $"Permission.NotFound: Permission with Id '{id}' not found.";
    public static string NameAlreadyExists(string name) => $"Permission.NameAlreadyExists: Permission with name '{name}' already exists.";
}
```

### 2. Permission Entity'sinin Yeniden Yapılandırılması

`Permission.cs` dosyası aşağıdaki DDD prensiplerine göre yeniden düzenlenmiştir:

1. **Encapsulation**: Tüm property'ler için private setter'lar kullanıldı
2. **Factory Method**: `Create` static metodu ile nesne oluşturma
3. **Domain Validation**: İş kurallarının entity içinde uygulanması
4. **Value Objects**: Primitive Obsession'dan kaçınmak ve iş kurallarını encapsulate etmek
5. **Result Pattern**: Operasyon sonuçlarının başarılı/başarısız durumlarını döndürmek

Yeni yapı:

```csharp
public class Permission : BaseEntity
{
    // Private setter'lar ile encapsulation sağlanır
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ResourceType { get; private set; }
    public string ResourceName { get; private set; }
    public string Action { get; private set; }
    public string Group { get; private set; }

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; private set; }
    public ICollection<UserPermission> UserPermissions { get; private set; }

    // EF Core için protected constructor
    protected Permission() { }

    // Primary constructor - private, sadece factory metodu üzerinden erişim
    private Permission(string name, string description, string resourceType, string resourceName, string action, string group)
    {
        Name = name;
        Description = description;
        ResourceType = resourceType;
        ResourceName = resourceName;
        Action = action;
        Group = group;
        RolePermissions = new List<RolePermission>();
        UserPermissions = new List<UserPermission>();
    }

    // Factory metodu - yeni Permission oluşturur
    public static Result<Permission> Create(
        string name,
        string description = "",
        string resourceType = "",
        string resourceName = "",
        string action = "",
        string group = "")
    {
        // İş kuralları ve doğrulama kontrolleri
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Permission>.Failure(DomainErrors.Permission.NameEmpty);
        }

        if (name.Length > 100)
        {
            return Result<Permission>.Failure(DomainErrors.Permission.NameTooLong);
        }

        if (description?.Length > 200)
        {
            return Result<Permission>.Failure(DomainErrors.Permission.DescriptionTooLong);
        }

        if (resourceType?.Length > 50)
        {
            return Result<Permission>.Failure(DomainErrors.Permission.ResourceTypeTooLong);
        }

        if (group?.Length > 50)
        {
            return Result<Permission>.Failure(DomainErrors.Permission.GroupTooLong);
        }

        var permission = new Permission(
            name,
            description ?? string.Empty,
            resourceType ?? string.Empty,
            resourceName ?? string.Empty,
            action ?? string.Empty,
            group ?? string.Empty);

        return Result<Permission>.Success(permission);
    }

    // Davranış metotları
    public Result UpdateDescription(string newDescription)
    {
        if (newDescription?.Length > 200)
        {
            return Result.Failure(DomainErrors.Permission.DescriptionTooLong);
        }

        Description = newDescription ?? string.Empty;
        return Result.Success();
    }

    public Result UpdateGroup(string newGroup)
    {
        if (newGroup?.Length > 50)
        {
            return Result.Failure(DomainErrors.Permission.GroupTooLong);
        }

        Group = newGroup ?? string.Empty;
        return Result.Success();
    }

    public Result UpdateResourceType(string newResourceType)
    {
        if (newResourceType?.Length > 50)
        {
            return Result.Failure(DomainErrors.Permission.ResourceTypeTooLong);
        }

        ResourceType = newResourceType ?? string.Empty;
        return Result.Success();
    }

    public void UpdateResourceName(string newResourceName)
    {
        ResourceName = newResourceName ?? string.Empty;
    }

    public void UpdateAction(string newAction)
    {
        Action = newAction ?? string.Empty;
    }
}
```

### 3. İlişkili Entity'lerin Güncellenmesi

`RolePermission` ve `UserPermission` entity'leri de DDD prensiplerine göre güncellendi:

#### RolePermission
```csharp
public class RolePermission : BaseEntity
{
    public int RoleId { get; private set; }
    public int PermissionId { get; private set; }

    // Navigation properties
    public Role Role { get; private set; }
    public Permission Permission { get; private set; }

    // EF Core için protected constructor
    protected RolePermission() { }

    // Primary constructor
    private RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }

    // Factory metodu
    public static RolePermission Create(int roleId, int permissionId)
    {
        return new RolePermission(roleId, permissionId);
    }
}
```

#### UserPermission
```csharp
public class UserPermission : BaseEntity
{
    public int UserId { get; private set; }
    public int PermissionId { get; private set; }
    public bool IsGranted { get; private set; } = true;

    // Navigation properties
    public User User { get; private set; }
    public Permission Permission { get; private set; }

    // EF Core için protected constructor
    protected UserPermission() { }

    // Primary constructor
    private UserPermission(int userId, int permissionId, bool isGranted)
    {
        UserId = userId;
        PermissionId = permissionId;
        IsGranted = isGranted;
    }

    // Factory metodu
    public static UserPermission Create(int userId, int permissionId, bool isGranted = true)
    {
        return new UserPermission(userId, permissionId, isGranted);
    }

    // Davranış metotları
    public void UpdateGrantStatus(bool isGranted)
    {
        IsGranted = isGranted;
    }
}
```

### 4. Entity Framework Core Configuration Güncellemeleri

Entity'lerin private setter'lar ile çalışabilmesi için EF Core configuration sınıflarına `UsePropertyAccessMode(PropertyAccessMode.Property)` eklendi.

```csharp
// Permission configuration
builder.UsePropertyAccessMode(PropertyAccessMode.Property);

// RolePermission configuration
builder.UsePropertyAccessMode(PropertyAccessMode.Property);

// UserPermission configuration
builder.UsePropertyAccessMode(PropertyAccessMode.Property);
```

### 5. Servis ve Controller Güncellemeleri

Tüm servis ve controller metotları, yeni factory metotlarını ve davranış metotlarını kullanacak şekilde güncellendi.

#### Controller Örneği
```csharp
private async Task AddPermissionIfNotExistsAsync(string name, string description, string group, string resourceType, string resourceName, string action)
{
    var permissionExists = await _context.Permissions.AnyAsync(p => p.Name == name);
    if (!permissionExists)
    {
        // Factory metodu kullan
        var permissionResult = Permission.Create(
            name: name,
            description: description,
            resourceType: resourceType,
            resourceName: resourceName,
            action: action,
            group: group);

        if (permissionResult.IsSuccess)
        {
            _context.Permissions.Add(permissionResult.Value);
            _logger.LogInformation(string.Format(LogMessages.PermissionAdded, name));
        }
        else
        {
            _logger.LogWarning(string.Format("İzin eklenemedi: {0}. Hata: {1}", name, permissionResult.Error));
        }
    }
}
```

#### Servis Örneği
```csharp
if (permissionIds != null && permissionIds.Count > 0)
{
    var newPermissions = permissionIds.Select(pid => 
        RolePermission.Create(roleId, pid)).ToList();
    
    await rolePermissionRepo.AddRangeAsync(newPermissions);
    _logger.LogDebug("{Count} adet yeni izin ilişkisi ekleniyor: RoleId {RoleId}", newPermissions.Count, roleId);
}
```

## DDD Prensipleri ve Nedenler

1. **Domain Entity'lerin Self-Contained Olması**: Tüm iş kuralları ve validasyon logic'i domain entity içinde tutulur.

2. **Immutability Prensibi**: Entity'ler mümkün olduğunca değişmez (immutable) yapılır ve değişiklikler kontrollü metotlar aracılığıyla yapılır.

3. **Ubiquitous Language**: Domain nesneleri, iş jargonunu yansıtan, anlaşılır isimlendirme ile tanımlanır.

4. **Strong Typing ve Encapsulation**: İç durumu korumak için encapsulation, primitive obsession'dan kaçınmak için güçlü veri tipleri.

5. **Consistency Boundaries**: Entity'ler, iş kurallarını uygulayarak tutarlı durumlarını korur. Geçersiz bir duruma geçişe izin verilmez.

6. **Domain Events**: Domain içindeki önemli değişiklikler, olay (event) olarak yayınlanabilir (bu implementasyonda aktif olarak kullanılmamaktadır).

## Uygulanan İyileştirmeler

1. **Problem Çözümü**: Önceki yapıda, iş kuralları dağınık şekilde yer alıyor ve bazı validasyonlar application logic içinde uygulanıyordu. Bu durum, kodun tekrarlanmasına ve mantığın dağılmasına neden oluyordu.

2. **Ölçülebilir İyileştirmeler**:
   - İş kuralları tek bir yerde toplandı (Permission entity'si)
   - Validation logic domain'e taşındı
   - Factory metotları ile her zaman geçerli nesneler oluşturulması garanti edildi
   - Domaine özel davranışlar entity içine taşındı

3. **Uzun Vadeli Faydalar**:
   - Bakım kolaylığı (Maintainability)
   - Kod tekrarının azaltılması (DRY Principle)
   - Daha sağlam ve güvenilir sistem
   - Daha anlaşılır ve okunaklı kod yapısı

## Sonuç

Bu değişikliklerle Permission entity'si ve ilişkili tüm yapılar Domain-Driven Design prensiplerine uygun olarak düzenlenmiştir. Bu düzenleme, kodun daha anlaşılır, bakımı daha kolay ve daha sağlam olmasını sağlamıştır. 