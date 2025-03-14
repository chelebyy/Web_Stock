# Çalışan Sistemi Koruyarak Kod İyileştirme Planı

Bu plan, mevcut çalışan sistemi bozmadan kod kalitesini artırmayı hedeflemektedir. Her aşama, risk seviyesine göre planlanmış ve kademeli bir iyileştirme stratejisi izlenmektedir.

## Genel Yaklaşım Prensipleri

- **Kademeli Değişiklik**: Tüm sistem yerine küçük, izole edilmiş parçalar üzerinde çalışılacak
- **Sürekli Test**: Her değişiklik sonrası kapsamlı test yapılacak
- **Feature Branch Kullanımı**: Her iyileştirme ayrı bir branch'te geliştirilecek
- **Geri Dönüş Stratejisi**: Her değişikliğin açık bir rollback planı olacak

## Aşama 1: Düşük Risk İyileştirmeleri (1-2 Sprint)

Bu aşamada, sistemin davranışını değiştirmeden yapılabilecek basit iyileştirmelere odaklanacağız:

### 1.1. Frontend Temizlik İşlemleri
- Üretim kodundan gereksiz `console.log` ifadelerinin temizlenmesi
- Kullanılmayan importların ve değişkenlerin kaldırılması
- Magic string/number'ların sabit (constant) değerlere dönüştürülmesi

### 1.2. Backend Temizlik İşlemleri
- Kullanılmayan using direktiflerinin kaldırılması
- Kod biçimlendirmesi ve düzenlemesi
- Yorum satırlarının iyileştirilmesi ve gereksizlerin temizlenmesi

### 1.3. Kod Belgeleme (Dokümantasyon)
- Kritik metotların XML belgelendirmesinin tamamlanması
- Karmaşık iş mantığı için açıklayıcı yorumlar eklenmesi

**Teknik Detaylar:**
```typescript
// ÖNCE
if (error.status === 401) {
  console.error('Yetkilendirme hatası (401). Token geçersiz veya eksik olabilir.');
}

// SONRA
export const ErrorCodes = {
  UNAUTHORIZED: 401,
  NOT_FOUND: 404,
  SERVER_ERROR: 500
};

if (error.status === ErrorCodes.UNAUTHORIZED) {
  console.error('Yetkilendirme hatası. Token geçersiz veya eksik olabilir.');
}
```

### Aşama 1 Risk Değerlendirmesi
- **Risk Seviyesi**: Düşük
- **Potansiyel Etkiler**: Minimal, sistem davranışını değiştirmez
- **Geri Alma Stratejisi**: Her değişiklik için basit revert commit yeterli

## Aşama 2: Orta Risk İyileştirmeleri (2-4 Sprint)

Bu aşamada, biraz daha kapsamlı ancak yine de izole edilebilir değişiklikler yapacağız:

### 2.1. Global Exception Handling (Backend)
- Mevcut try-catch bloklarını koruyarak global exception middleware eklenmesi
- Önce sadece loglama yapacak şekilde aktif edilmesi
- Stabil çalıştığından emin olduktan sonra try-catch bloklarının kademeli olarak kaldırılması

**Teknik Detaylar:**
```csharp
// Program.cs'e eklenecek middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// GlobalExceptionHandlingMiddleware sınıfı
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Global hata yakalama: {Message}", ex.Message);
            
            // İlk aşamada sadece loglama yapıp, mevcut hata işlemeye izin ver
            // İkinci aşamada aşağıdaki kodları aktif ederek global hata yönetimi sağla
            /*
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ValidationException => StatusCodes.Status400BadRequest,
                AuthenticationException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
            
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message,
                statusCode = context.Response.StatusCode
            });
            */
            
            // Hatayı yeniden fırlat (ilk aşamada)
            throw;
        }
    }
}
```

### 2.2. Shared Components (Frontend)
- En çok tekrarlanan UI bileşenlerinin shared klasörüne taşınması (başlangıçta: dialog, confirmation dialog)
- Bir seferde bir bileşen üzerinde çalışarak risk en aza indirilecek

**Teknik Detaylar:**
```typescript
// YENİ: frontend/src/app/shared/components/confirmation-dialog/confirmation-dialog.component.ts
@Component({
  selector: 'app-confirmation-dialog',
  template: `
    <p-dialog [visible]="visible" [style]="style" [modal]="true" 
              header="{{header}}" [closable]="closable" 
              (onHide)="onCancel()">
      <div class="confirmation-content">
        <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem"></i>
        <span>{{message}}</span>
      </div>
      <ng-template pTemplate="footer">
        <button pButton label="{{cancelLabel}}" icon="pi pi-times" 
                class="p-button-text" (click)="onCancel()"></button>
        <button pButton label="{{confirmLabel}}" icon="pi pi-check" 
                class="p-button-danger" (click)="onConfirm()"></button>
      </ng-template>
    </p-dialog>
  `,
  standalone: true,
  imports: [DialogModule, ButtonModule]
})
export class ConfirmationDialogComponent {
  @Input() visible: boolean = false;
  @Input() header: string = 'Onay';
  @Input() message: string = 'Bu işlemi yapmak istediğinizden emin misiniz?';
  @Input() confirmLabel: string = 'Evet';
  @Input() cancelLabel: string = 'Hayır';
  @Input() closable: boolean = true;
  @Input() style: any = { width: '450px' };
  
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();
  
  onConfirm(): void {
    this.confirm.emit();
  }
  
  onCancel(): void {
    this.cancel.emit();
  }
}
```

### 2.3. Base Service Oluşturma (Frontend)
- Ortak HTTP istekleri için BaseHttpService oluşturulması
- Öncelikle yeni servisler için kullanılmaya başlanması
- Çalışır hale geldiğinde mevcut servislerin kademeli olarak refactor edilmesi

**Teknik Detaylar:**
```typescript
// YENİ: frontend/src/app/core/services/base-http.service.ts
@Injectable()
export abstract class BaseHttpService<T> {
  protected constructor(
    protected http: HttpClient,
    protected authService: AuthService,
    protected endpoint: string
  ) {}

  protected getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  getAll(): Observable<T[]> {
    const options = { headers: this.getHeaders() };
    return this.http.get<T[]>(`${environment.apiUrl}/api/${this.endpoint}`, options)
      .pipe(
        catchError(this.handleError<T[]>('getAll', []))
      );
  }

  getById(id: number): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.get<T>(`${environment.apiUrl}/api/${this.endpoint}/${id}`, options)
      .pipe(
        catchError(this.handleError<T>(`getById id=${id}`))
      );
  }

  create(item: T): Observable<T> {
    const options = { headers: this.getHeaders() };
    return this.http.post<T>(`${environment.apiUrl}/api/${this.endpoint}`, item, options)
      .pipe(
        catchError(this.handleError<T>('create'))
      );
  }

  update(id: number, item: T): Observable<any> {
    const options = { headers: this.getHeaders() };
    return this.http.put(`${environment.apiUrl}/api/${this.endpoint}/${id}`, item, options)
      .pipe(
        catchError(this.handleError<any>(`update id=${id}`))
      );
  }

  delete(id: number): Observable<any> {
    const options = { headers: this.getHeaders() };
    return this.http.delete(`${environment.apiUrl}/api/${this.endpoint}/${id}`, options)
      .pipe(
        catchError(this.handleError<any>(`delete id=${id}`))
      );
  }

  protected handleError<R>(operation = 'operation', result?: R) {
    return (error: any): Observable<R> => {
      // Hata loglama
      console.error(`${operation} başarısız: ${error.message}`);

      // Kullanıcıya gösterilen hata mesajı
      let errorMessage = 'Bir hata oluştu. Lütfen daha sonra tekrar deneyin.';
      
      if (error.status === 401) {
        errorMessage = 'Oturum süreniz dolmuş olabilir. Lütfen tekrar giriş yapın.';
      } else if (error.status === 403) {
        errorMessage = 'Bu işlemi gerçekleştirmek için yetkiniz bulunmuyor.';
      } else if (error.status === 404) {
        errorMessage = 'İstenen kaynak bulunamadı.';
      }

      // Özel hata nesnesi
      const customError = new Error(errorMessage);
      (customError as any).originalError = error;
      
      // Boş sonuç döndür ya da hatayı fırlat
      if (result) {
        return of(result);
      }
      return throwError(() => customError);
    };
  }
}

// Örnek kullanım:
@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService<User> {
  constructor(http: HttpClient, authService: AuthService) {
    super(http, authService, 'users');
  }
  
  // Sadece UserService'e özgü metotlar buraya eklenebilir...
}
```

### Aşama 2 Risk Değerlendirmesi
- **Risk Seviyesi**: Orta
- **Potansiyel Etkiler**: Belirli bileşenlerde işlevsellik sorunları
- **Geri Alma Stratejisi**: Değişiklik öncesi yedek, etkilenen bileşenlerin izole edilmesi

## Aşama 3: Yapısal İyileştirmeler (4-6 Sprint)

Bu aşamada, sistemin belirli bölümlerinde daha kapsamlı değişiklikler yapacağız:

### 3.1. Büyük Bileşenlerin Bölünmesi (Frontend)
- İlk hedef: user-management.component.ts (1349 satır) bölünmesi
- Sırayla:
  1. Kullanıcı listeleme/tablo işlevselliği ayrı bir bileşene çıkarılacak
  2. Form işlevselliği ayrı bir bileşene çıkarılacak
  3. Filtreleme/arama işlevselliği ayrı bir bileşene çıkarılacak

**Teknik Yaklaşım:**
```typescript
// 1. Adım: UserListComponent oluşturma
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  standalone: true,
  imports: [/* Gerekli modüller */]
})
export class UserListComponent {
  @Input() users: User[] = [];
  @Output() editUser = new EventEmitter<User>();
  @Output() deleteUser = new EventEmitter<User>();
  
  // Sadece listeleme ve tablo işlemleriyle ilgili kod buraya taşınacak
}

// 2. Adım: UserFormComponent oluşturma
@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  standalone: true,
  imports: [/* Gerekli modüller */]
})
export class UserFormComponent {
  @Input() user: User | null = null;
  @Input() roles: Role[] = [];
  @Output() save = new EventEmitter<User>();
  @Output() cancel = new EventEmitter<void>();
  
  // Form işlemleriyle ilgili kod buraya taşınacak
}

// 3. Adım: Ana bileşeni güncelleme
@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  standalone: true,
  imports: [
    UserListComponent,
    UserFormComponent,
    // Diğer gerekli modüller
  ]
})
export class UserManagementComponent {
  // Ana bileşen sadece koordinasyon yapacak
  // Alt bileşenlerin etkileşimini yönetecek
}
```

### 3.2. Base Controller Oluşturma (Backend)
- Temel CRUD işlemleri için BaseController oluşturulması
- Önce yeni controller'larda kullanılması, sonra mevcut controller'ların refactor edilmesi

**Teknik Detaylar:**
```csharp
// YENİ: BaseController sınıfı
public abstract class BaseController<TEntity, TDto> : ControllerBase 
    where TEntity : BaseEntity
{
    protected readonly IMediator _mediator;
    protected readonly ILogger _logger;

    protected BaseController(IMediator mediator, ILogger logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        try
        {
            var query = new GetAllEntitiesQuery<TEntity>();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all entities");
            return StatusCode(500, "İşlem sırasında bir hata oluştu");
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(int id)
    {
        try
        {
            var query = new GetEntityByIdQuery<TEntity>(id);
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting entity by id {Id}", id);
            return StatusCode(500, "İşlem sırasında bir hata oluştu");
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create(CreateEntityCommand<TEntity, TDto> command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity");
            return StatusCode(500, "İşlem sırasında bir hata oluştu");
        }
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(int id, UpdateEntityCommand<TEntity, TDto> command)
    {
        if (id != command.Id)
            return BadRequest("ID değerleri eşleşmiyor");
            
        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity {Id}", id);
            return StatusCode(500, "İşlem sırasında bir hata oluştu");
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteEntityCommand<TEntity>(id);
            await _mediator.Send(command);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entity {Id}", id);
            return StatusCode(500, "İşlem sırasında bir hata oluştu");
        }
    }
}
```

### 3.3. State Yönetimi Standardizasyonu (Frontend)
- Angular Signals yaklaşımının standart olarak benimsenmesi
- Yeni bileşenlerde doğrudan Signals kullanılması
- Mevcut bileşenlerin kademeli olarak Signals yaklaşımına geçirilmesi

**Teknik Detaylar:**
```typescript
// ÖNCE: Klasik yaklaşım
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  selectedUser: User | null = null;
  
  // ...
  
  loadUsers() {
    this.userService.getUsers().subscribe(users => {
      this.users = users;
    });
  }
}

// SONRA: Signals yaklaşımı
export class UserManagementComponent implements OnInit {
  users = signal<User[]>([]);
  selectedUser = signal<User | null>(null);
  
  // ...
  
  loadUsers() {
    this.userService.getUsers().subscribe(users => {
      this.users.set(users);
    });
  }
}
```

### Aşama 3 Risk Değerlendirmesi
- **Risk Seviyesi**: Yüksek
- **Potansiyel Etkiler**: Önemli bileşenlerdeki değişiklikler sistem davranışını etkileyebilir
- **Geri Alma Stratejisi**: Her büyük değişiklik öncesi feature branch, detaylı test ve onay süreci

## Aşama 4: Mimari İyileştirmeler (6+ Sprint)

Bu aşama, en kapsamlı ve riskli değişiklikleri içerir:

### 4.1. Mimari Tutarlılık (Backend)
- Standart pattern olarak MediatR/CQRS benimsenmesi
- Prototip olarak bir controller'ın dönüştürülmesi ve test edilmesi
- Diğer controller'ların kademeli olarak dönüştürülmesi

**Teknik Detaylar:**
```csharp
// ÖNCE: Doğrudan DbContext kullanımı
[HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetRoles()
{
    var roles = await _context.Roles.AsNoTracking().Select(/*...*/).ToListAsync();
    return Ok(roles);
}

// SONRA: MediatR/CQRS yaklaşımı
[HttpGet]
public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
{
    var query = new GetAllRolesQuery();
    var result = await _mediator.Send(query);
    return Ok(result);
}

// Query Handler
public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}
```

### 4.2. Validation Logic'in Standardizasyonu
- İş mantığı validator sınıflarına taşınacak
- Mevcut validasyon korunurken, yeni validator sınıfları eklenecek
- Tüm validasyon logic'i kademeli olarak bu sınıflara taşınacak

**Teknik Detaylar:**
```csharp
// YENİ: Validator sınıfı (FluentValidation kullanarak)
public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IApplicationDbContext _context;
    
    public CreateUserValidator(IApplicationDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .Length(3, 100).WithMessage("Kullanıcı adı 3-100 karakter arasında olmalıdır")
            .MustAsync(BeUniqueUsername).WithMessage("Bu kullanıcı adı zaten kullanımda");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır");
            
        RuleFor(x => x.Sicil)
            .NotEmpty().WithMessage("Sicil numarası boş olamaz")
            .MustAsync(BeUniqueSicil).WithMessage("Bu sicil numarası zaten kullanımda");
            
        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Geçerli bir rol seçilmelidir");
    }
    
    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return !await _context.Users
            .AnyAsync(u => u.Username == username, cancellationToken);
    }
    
    private async Task<bool> BeUniqueSicil(string sicil, CancellationToken cancellationToken)
    {
        return !await _context.Users
            .AnyAsync(u => u.Sicil == sicil, cancellationToken);
    }
}

// Kullanım: Handler içinde
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<CreateUserCommand> _validator;
    
    public CreateUserCommandHandler(
        IApplicationDbContext context,
        IValidator<CreateUserCommand> validator)
    {
        _context = context;
        _validator = validator;
    }
    
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Validasyon
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        // İşlemlere devam et...
    }
}
```

### 4.3. Domain-Driven Design'a Geçiş
- İş mantığının entity'lere taşınması (başlangıçta bir entity ile)
- Çalışan bir modelle başlayıp, sonra diğer entity'lere uygulama

**Teknik Detaylar:**
```csharp
// ÖNCE: Anemic Domain Model
public class User : BaseEntity
{
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Sicil { get; set; }
    public bool IsAdmin { get; set; }
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
}

// SONRA: Rich Domain Model
public class User : BaseEntity
{
    // Private setters için backing fields
    private string _username;
    private string _passwordHash;
    private string _sicil;
    
    // Sadece entity framework için protected constructor
    protected User() { }
    
    // Domain logic için public constructor
    public User(string username, string sicil, int roleId)
    {
        SetUsername(username);
        SetSicil(sicil);
        RoleId = roleId;
        IsActive = true;
        IsDeleted = false;
        CreatedAt = DateTime.UtcNow;
    }
    
    public string Username 
    { 
        get => _username; 
        private set => _username = value; 
    }
    
    public string PasswordHash 
    { 
        get => _passwordHash; 
        private set => _passwordHash = value; 
    }
    
    public string Sicil 
    { 
        get => _sicil; 
        private set => _sicil = value; 
    }
    
    public bool IsAdmin { get; private set; }
    public bool IsActive { get; private set; }
    public int RoleId { get; private set; }
    public virtual Role Role { get; private set; }
    
    // Domain metodları
    public void SetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Kullanıcı adı boş olamaz");
            
        if (username.Length < 3 || username.Length > 100)
            throw new DomainException("Kullanıcı adı 3-100 karakter arasında olmalıdır");
            
        _username = username;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetSicil(string sicil)
    {
        if (string.IsNullOrWhiteSpace(sicil))
            throw new DomainException("Sicil numarası boş olamaz");
            
        _sicil = sicil;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void SetPassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Şifre hash'i boş olamaz");
            
        _passwordHash = passwordHash;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AssignRole(int roleId)
    {
        if (roleId <= 0)
            throw new DomainException("Geçerli bir rol ID'si belirtilmelidir");
            
        RoleId = roleId;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void MarkAsAdmin()
    {
        IsAdmin = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveAdminRights()
    {
        IsAdmin = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Delete()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

### Aşama 4 Risk Değerlendirmesi
- **Risk Seviyesi**: Çok Yüksek
- **Potansiyel Etkiler**: Temel mimari değişiklikler sistemin her yerini etkileyebilir
- **Geri Alma Stratejisi**: Sıkı sınır kontrolleri, detaylı testler, incremental deployment, blue-green deployment stratejisi

## Uygulama Stratejisi ve Risk Yönetimi

### Feature Branch Yaklaşımı
- Her iyileştirme için ayrı bir feature branch oluşturulacak
- Branch'ler küçük, mantıklı işlere bölünecek
- Pull Request (PR) incelemesi ile kodun kalitesi kontrol edilecek

### Test Stratejisi
- Her değişiklik için birim testleri yazılacak/güncellenecek
- Entegrasyon testleriyle sistem davranışının değişmediği doğrulanacak
- Manuel testlerle kullanıcı deneyiminin korunduğu teyit edilecek
- Mümkünse otomatik testler eklenecek (özellikle yüksek riskli değişiklikler için)

### Rollback Planı
- Her değişiklik için net bir geri alma planı olacak
- Her kritik değişiklik öncesi veritabanı yedekleri alınacak
- Sorun durumunda önceki sürüme hızlı dönüş için hazırlık yapılacak

### Zamanlama
- İyileştirmeler normal sprint/proje planlama sürecine entegre edilecek
- Yüksek riskli değişiklikler için düşük trafik zamanları seçilecek
- Bir sprintte çok fazla refactoring işlemi planlanmayacak (yeni özelliklerle dengeli olacak)

## Zaman Çizelgesi

**Kısa Vadeli (1-2 Sprint):**
- Aşama 1'deki düşük riskli iyileştirmeler
- Global exception handling middleware eklenmesi (sadece loglama)
- En çok tekrarlanan UI yapılarının shared component'e çıkarılması

**Orta Vadeli (2-4 Sprint):**
- Aşama 2'nin kalan kısmı
- En büyük bileşenlerin bölünmeye başlanması
- BaseService implementasyonu ve bazı servislerin dönüştürülmesi

**Uzun Vadeli (4-6+ Sprint):**
- Aşama 3 ve 4'ün kademeli uygulanması
- Mimari tutarlılık için dönüşümler

## Metrikler ve Takip

Her aşamanın başarısını ölçmek için aşağıdaki metrikler takip edilecek:

1. **Kod Kalite Metrikleri**:
   - Kod satır sayısı (LOC) azalması
   - Siklomat kompleksite düşüşü
   - Tekrarlanan kod miktarı azalması

2. **Performans Metrikleri**:
   - Sayfa yükleme süreleri
   - API yanıt süreleri
   - Veritabanı sorgu süreleri

3. **Hata Metrikleri**:
   - Hata oranı değişimi
   - Yeniden açılan hata sayısı
   - Çözüm süresi

4. **Geliştirme Metrikleri**:
   - Yeni özellik geliştirme hızı
   - Test kapsamı (coverage) artışı
   - Teknik borç azalması
</rewritten_file> 