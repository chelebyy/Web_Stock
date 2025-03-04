# Kullanıcı Güncelleme Handler Bilgi Tabanı

Bu belge, kullanıcı güncelleme işlemi için MediatR handler'ının oluşturulması ve ilgili sorunların çözümü hakkında bilgiler içermektedir.

## Sorun

Kullanıcı güncelleme işlemi sırasında aşağıdaki hata alındı:

```
System.InvalidOperationException: No service for type 'MediatR.IRequestHandler`2[Stock.Application.Features.Users.Commands.UpdateUserCommand,Stock.Application.Models.DTOs.UserDto]' has been registered.
```

Bu hata, MediatR'ın UpdateUserCommand için bir handler bulamadığını göstermektedir. Yani UpdateUserCommandHandler sınıfı oluşturulmamış veya düzgün kaydedilmemiştir.

## Çözüm Adımları

### 1. UpdateUserCommandHandler Sınıfının Oluşturulması

```csharp
using AutoMapper;
using MediatR;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Models.DTOs;
using Stock.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            
            if (user == null)
                return null;

            user.Username = request.Username;
            user.IsAdmin = request.IsAdmin;
            user.RoleId = request.RoleId;
            user.Sicil = request.Sicil;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            return _mapper.Map<UserDto>(updatedUser);
        }
    }
}
```

### 2. Sicil Alanının Eklenmesi

Kullanıcı güncelleme işlemi sırasında sicil alanı da eklenmiştir. Bu alanın eklenmesi için aşağıdaki değişiklikler yapılmıştır:

#### 2.1. UpdateUserCommand'e Sicil Alanı Ekleme

```csharp
public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsAdmin { get; set; }
    public int? RoleId { get; set; }
    public string Sicil { get; set; } = string.Empty;
}
```

#### 2.2. User Entity'sine Sicil Alanı Ekleme

```csharp
[StringLength(50)]
public string Sicil { get; set; } = string.Empty;
```

#### 2.3. UserDto'ya Sicil Alanı Ekleme

```csharp
public string Sicil { get; set; } = string.Empty;
```

#### 2.4. MappingProfile'da Sicil Alanı Eşleştirmesi Ekleme

```csharp
CreateMap<User, UserDto>()
    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
    .ForMember(dest => dest.Sicil, opt => opt.MapFrom(src => src.Sicil));
```

## MediatR Handler Kayıt Mekanizması

MediatR, handler sınıflarını otomatik olarak tarar ve kaydeder. Bu işlem, DependencyInjection.cs dosyasında aşağıdaki kod ile gerçekleştirilir:

```csharp
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
```

Bu kod, mevcut assembly'deki tüm MediatR handler'larını tarar ve DI container'a kaydeder. Handler sınıfları, ilgili Command veya Query sınıflarıyla aynı assembly'de olmalıdır.

## Önemli Notlar

- Handler sınıfları, IRequestHandler<TRequest, TResponse> arayüzünü uygulamalıdır.
- Handler sınıfları, Handle metodunu uygulamalıdır.
- Handler sınıfları, ilgili Command veya Query sınıflarıyla aynı assembly'de olmalıdır.
- Sicil alanı eklendiğinde veritabanı şemasını güncellemek için migration oluşturulmalıdır.
- MediatR, handler sınıflarını otomatik olarak tarar ve kaydeder.

## Sonuç

UpdateUserCommandHandler sınıfının oluşturulması ve sicil alanının eklenmesi ile kullanıcı güncelleme işlemi başarıyla gerçekleştirilmiştir. Bu değişiklikler, kullanıcı yönetimi sayfasında kullanıcı güncelleme işleminin düzgün çalışmasını sağlamıştır. 