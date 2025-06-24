# Role Entity DDD Düzenlemeleri

**Son Güncelleme: 08.03.2025**
**Versiyon: 1.0**

## Genel Bakış

Domain-Driven Design (DDD) prensiplerine uygun olarak `Role` entity'si üzerinde yapılan düzenlemeler bu belge içerisinde detaylandırılmıştır. Bu düzenlemeler, domain mantığının entity içerisinde encapsulation edilmesini, iş kurallarının korunmasını ve domain bütünlüğünün sağlanmasını amaçlamaktadır.

## Yapılan Değişiklikler

### 1. Domain Error Mesajlarının Eklenmesi

`DomainErrors.Role` sınıfına role ile ilgili domain-specific hata mesajları eklendi:

```csharp
public static class Role
{
    public const string NameEmpty = "Role.NameEmpty: Role name cannot be empty.";
    public const string NameTooLong = "Role.NameTooLong: Role name cannot exceed 50 characters.";
    public const string DescriptionTooLong = "Role.DescriptionTooLong: Role description cannot exceed 200 characters.";
    public static string NotFound(int id) => $"Role.NotFound: Role with Id '{id}' not found.";
    public static string NameAlreadyExists(string name) => $"Role.NameAlreadyExists: Role with name '{name}' already exists.";
}
```

### 2. Role Entity Dönüşümü

Role entity'si DDD prensiplerine uygun olarak aşağıdaki şekilde düzenlendi:

1. **Private Setter'lar**: Property'ler private setter'lar ile tanımlandı
2. **Protected/Private Constructor**: Entity Framework için protected ve domain mantığı için private constructor tanımlandı
3. **Factory Metotları**: Entity oluşturma işlemi için factory metotları eklendi
4. **Domain Davranış Metotları**: İş kurallarını içeren davranış metotları eklendi
5. **İlişki Yönetimi Metotları**: Entity ilişkilerini yöneten metotlar eklendi

```csharp
public class Role : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; private set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; private set; } = string.Empty;

    public virtual ICollection<User> Users { get; private set; } = new HashSet<User>();
    
    public virtual ICollection<RolePermission> RolePermissions { get; private set; } = new HashSet<RolePermission>();

    // Entity Framework için protected constructor
    protected Role()
    {
        Users = new HashSet<User>();
        RolePermissions = new HashSet<RolePermission>();
    }

    // Private constructor ile nesne oluşturma kontrolü
    private Role(string name, string description)
    {
        Name = name;
        Description = description;
        Users = new HashSet<User>();
        RolePermissions = new HashSet<RolePermission>();
    }

    // Factory metodu - Entity oluşturmayı kontrol ediyor
    public static Result<Role> Create(string name, string description)
    {
        // Validasyon kuralları
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Role>.Failure(DomainErrors.Role.NameEmpty);
        }

        if (name.Length > 50)
        {
            return Result<Role>.Failure(DomainErrors.Role.NameTooLong);
        }

        if (description != null && description.Length > 200)
        {
            return Result<Role>.Failure(DomainErrors.Role.DescriptionTooLong);
        }

        var role = new Role(name, description ?? string.Empty);
        return Result<Role>.Success(role);
    }

    // Domain davranışları - Entity'nin iş kurallarını zorunlu kılıyor
    public Result UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            return Result.Failure(DomainErrors.Role.NameEmpty);
        }

        if (newName.Length > 50)
        {
            return Result.Failure(DomainErrors.Role.NameTooLong);
        }

        Name = newName;
        return Result.Success();
    }

    public Result UpdateDescription(string newDescription)
    {
        if (newDescription != null && newDescription.Length > 200)
        {
            return Result.Failure(DomainErrors.Role.DescriptionTooLong);
        }

        Description = newDescription ?? string.Empty;
        return Result.Success();
    }

    // İlişki yönetimi metotları
    public void AddPermission(RolePermission permission)
    {
        RolePermissions.Add(permission);
    }

    public void RemovePermission(RolePermission permission)
    {
        RolePermissions.Remove(permission);
    }

    public void ClearPermissions()
    {
        RolePermissions.Clear();
    }
}
```

### 3. Entity Framework Core Yapılandırması

EF Core'un private setter'lara erişebilmesi için `RoleConfiguration` sınıfında gerekli yapılandırmalar yapıldı:

```csharp
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .HasMaxLength(50)
            .IsRequired();
            
        builder.Property(r => r.Description)
            .HasMaxLength(200);
            
        // Private setter'lara erişim için PropertyAccessMode.Property ayarı
        builder.UsePropertyAccessMode(PropertyAccessMode.Property);
        
        // Diğer yapılandırmalar...
    }
}
```

### 4. Command Handler'ların Güncellenmesi

Command handler'lar, Role entity'sinin Factory ve Domain Behavior metotlarını kullanacak şekilde güncellendi:

#### 4.1. CreateRoleCommandHandler

```csharp
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Rol adının benzersiz olduğunu kontrol et
        var existingRoleSpec = new RoleByNameSpecification(request.Name);
        var existingRole = await _unitOfWork.Roles.FirstOrDefaultAsync(existingRoleSpec);
        
        if (existingRole != null)
        {
            throw new DuplicateEntityException(DomainErrors.Role.NameAlreadyExists(request.Name));
        }

        // Factory metodu ile yeni Role entity'si oluştur
        var roleResult = Role.Create(request.Name, request.Description);
        
        if (!roleResult.IsSuccess)
        {
            throw new BadRequestException(roleResult.Error);
        }

        var role = roleResult.Value;

        await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }
}
```

#### 4.2. UpdateRoleCommandHandler

```csharp
public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // Güncellenecek rolü bul
        var spec = new EntityByIdSpecification<Role>(request.Id);
        var role = await _unitOfWork.Roles.FirstOrDefaultAsync(spec);
        
        if (role == null)
        {
            throw new NotFoundException(DomainErrors.Role.NotFound(request.Id));
        }

        // Rol adı değişiyorsa benzersizliğini kontrol et
        if (role.Name != request.Name)
        {
            var existingRoleSpec = new RoleByNameSpecification(request.Name);
            var existingRole = await _unitOfWork.Roles.FirstOrDefaultAsync(existingRoleSpec);
            
            if (existingRole != null && existingRole.Id != request.Id)
            {
                throw new DuplicateEntityException(DomainErrors.Role.NameAlreadyExists(request.Name));
            }
            
            // Domain davranış metodu ile adı güncelle
            var nameUpdateResult = role.UpdateName(request.Name);
            if (!nameUpdateResult.IsSuccess)
            {
                throw new BadRequestException(nameUpdateResult.Error);
            }
        }

        // Domain davranış metodu ile açıklamayı güncelle
        var descriptionUpdateResult = role.UpdateDescription(request.Description);
        if (!descriptionUpdateResult.IsSuccess)
        {
            throw new BadRequestException(descriptionUpdateResult.Error);
        }

        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<RoleDto>(role);
    }
}
```

## Uygulanan DDD Prensipleri

1. **Encapsulation (Kapsülleme)**: Entity state'i private setter'lar ile korundu, sadece entity'nin kendi metotları üzerinden değiştirilebilir hale getirildi.

2. **Factory Method Pattern**: Entity oluşturma işlemi, validasyon kurallarını içeren statik factory metotları ile yapılarak, entity'nin her zaman geçerli durumda olması sağlandı.

3. **Domain Behaviors (Entity Davranışları)**: Entity içinde iş kurallarını barındıran davranış metotları tanımlandı. Bu sayede iş kurallarının her durumda uygulanması garantilendi.

4. **Result Pattern**: Domain validasyonları için Result/Result\<T> nesneleri kullanılarak, validasyon hatalarının açık ve anlaşılır bir şekilde yönetilmesi sağlandı.

5. **Invariants (Değişmezler)**: Entity'nin oluşturulması ve değiştirilmesi sırasında iş kurallarının (invariant'ların) korunması sağlandı.

## Sonuç ve Öğrenilen Dersler

- **Domain Logic in Entity**: İş kurallarını entity içinde tutmak, bu kuralların her zaman uygulanmasını sağlar.
- **Factory Methods**: Factory metotları, entity'nin geçerli durumda oluşturulmasını garantiler.
- **EF Core Configuration**: EF Core, PropertyAccessMode.Property ayarı ile private setter'lara erişebilir.
- **Result Pattern**: Result/Result\<T> tipleri, domain hatalarını yönetmek için etkili bir yöntemdir.
- **Command Handlers**: Command handler'lar, domain entity'lerinin metotlarını kullanarak business logic'i uygular.

Domain-Driven Design prensiplerine uygun olarak Role entity'sinin düzenlenmesi, domain mantığının daha iyi encapsulation edilmesini, iş kurallarının her zaman uygulanmasını ve kodun daha sürdürülebilir olmasını sağlamıştır. Bu yaklaşım, diğer entity'ler için de uygulanabilir ve projenin domain bütünlüğünü güçlendirir.

## Gelecekteki İyileştirmeler

1. Rol-izin ilişkisinin daha sıkı bir şekilde modellenmesi için ValueObject veya EntityId kullanımı değerlendirilebilir.

2. Rol adının benzersizliğini korumak için domain servisi oluşturulabilir.

3. Result<T> ve Result tiplerinin daha gelişmiş versiyonları (OneOf, ErrorOr gibi) değerlendirilebilir.

## Değişiklik Geçmişi

- **v1.0 (08.03.2025)**: İlk sürüm oluşturuldu. 