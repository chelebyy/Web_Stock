# Project Rules

## Basic Operating Principles

1. **Receiving and Understanding Instructions**
   - Carefully interpret user instructions.
   - Ask specific questions when clarification is needed.
   - Clearly understand technical constraints and requirements.

2. **Deep Analysis and Planning**
   - Task Analysis: The ultimate goal of the task.
   - Technical Requirements: Technology stack and constraints.
   - Implementation Steps: Specific steps.
   - Risks: Potential issues.
   - Quality Standards: Standards to be met.

3. **Developing an Implementation Plan**
   - Specific steps and expected challenges.

4. **Incremental Implementation and Verification**
   - Verification after each step is completed.
   - Immediate response to issues.
   - Comparison with quality standards.

5. **Continuous Feedback**
   - Regular reporting of implementation progress.
   - Approval at critical decision points.
   - Rapid reporting of issues.

## Technology Stack and Constraints

- **Core Technologies**: .NET Core 8, PostgreSQL 17.3, Angular 19.1.2, PrimeNG 19.0.6,Tailwind CSS 3.4.1.
- **Development Tools**: npm, ESLint.

## Project Structure (Clean Architecture)

```
Solution
├── src/
│   ├── Stock.Domain/              # Domain Layer
│   │   ├── Entities/             # Domain entities
│   │   ├── Interfaces/           # Repository interfaces
│   │   ├── Events/              # Domain events
│   │   ├── Exceptions/          # Domain exceptions
│   │   └── Specifications/      # Query specifications
│   │
│   ├── Stock.Application/         # Application Layer
│   │   ├── Common/              # Shared components
│   │   ├── Features/            # Use cases
│   │   │   ├── Auth/           # Authentication
│   │   │   ├── Users/          # User management
│   │   │   └── Roles/          # Role management
│   │   ├── Interfaces/         # Service interfaces
│   │   ├── Models/             # DTOs
│   │   └── Services/           # Application services
│   │
│   ├── Stock.Infrastructure/      # Infrastructure Layer
│   │   ├── Data/               # Data access
│   │   │   ├── Config/        # EF configurations
│   │   │   ├── Migrations/    # EF migrations
│   │   │   └── Repositories/  # Repository implementations
│   │   ├── Identity/          # Authentication
│   │   ├── Logging/           # Logging implementations
│   │   └── Services/          # External services
│   │
│   └── Stock.API/                # API Layer
│       ├── Controllers/         # API endpoints
│       ├── Filters/            # Action filters
│       ├── Middleware/         # Custom middleware
│       └── Extensions/         # Service extensions
│
├── tests/                        # Test Projects
│   ├── Stock.UnitTests/         # Unit tests
│   ├── Stock.IntegrationTests/  # Integration tests
│   └── Stock.FunctionalTests/   # E2E tests
│
├── frontend/                     # Angular Frontend
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/    # UI components
│   │   │   ├── services/      # API services
│   │   │   ├── models/        # TypeScript interfaces
│   │   │   └── guards/        # Route guards
│   │   ├── assets/           # Static files
│   │   └── environments/     # Environment configs
│   └── tests/                # Frontend tests
│
└── docs/                        # Documentation
    ├── guides/               # User guides
    └── architecture/        # Architecture docs
```

## Local System Requirements

1. **Development Environment**
   - Visual Studio 2022
   - Visual Studio Code
   - PostgreSQL 17.3
   - Node.js 18.x
   - npm 9.x

2. **Hardware Requirements**
   - Minimum 8GB RAM
   - 100GB disk space
   - x64 processor

3. **Network Requirements**
   - Local network connectivity
   - Standard HTTP/HTTPS ports
   - No internet dependency

## Implementation Guidelines

1. **Database Access**
```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
```

2. **Service Layer**
```csharp
public interface IBaseService<T, TDto>
{
    Task<TDto> GetByIdAsync(int id);
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> CreateAsync(TDto dto);
    Task UpdateAsync(TDto dto);
    Task DeleteAsync(int id);
}
```

3. **Local Logging**
```csharp
public interface ILocalLogger
{
    void LogInformation(string message);
    void LogError(Exception ex, string message);
    void LogAudit(string userId, string action);
}
```

## Quality Management Protocol

1. **Code Quality**
   - Clean code principles
   - SOLID principles
   - Code reviews
   - Static code analysis

2. **Testing**
   - Unit testing
   - Integration testing
   - Manual testing
   - Performance testing

3. **Documentation**
   - Code comments
   - API documentation
   - User guides
   - System guides

4. **Performance**
   - Query optimization
   - Local caching
   - Memory management
   - Response time monitoring

## Security Protocol (Local)

1. **Authentication**
   - JWT implementation
   - Password hashing
   - Role-based access
   - Session management

2. **Data Protection**
   - Input validation
   - SQL injection prevention
   - XSS prevention
   - Local data encryption

3. **Audit**
   - User action logging
   - System event logging
   - Error logging
   - Performance logging

## Backup and Recovery

1. **Backup Strategy**
   - Daily database backups
   - Configuration backups
   - Log file backups
   - System state backups

2. **Recovery Procedures**
   - Database restore
   - System restore
   - Log recovery
   - Point-in-time recovery

## Error Management Protocol

1. **Problem Identification**
   - Analysis of error messages.
   - Definition of impact scope.
   - Root cause analysis.

2. **Solution Development**
   - Evaluation of multiple response options.
   - Risk assessment.
   - Selection of the most appropriate solution.

3. **Implementation and Verification**
   - Solution implementation.
   - Test verification.
   - Verification of side effects.

4. **Documentation**
   - Recording of problems and solutions.
   - Suggestion of preventive measures.
   - Sharing of learning points.

## SOLID Principles

1. **Single Responsibility Principle (SRP)**
   - Each class should have only one responsibility.

2. **Open/Closed Principle (OCP)**
   - Software entities should be open for extension but closed for modification.

3. **Liskov Substitution Principle (LSP)**
   - Derived classes should be substitutable for their base classes.

4. **Interface Segregation Principle (ISP)**
   - Clients should not be forced to depend on interfaces they do not use.

5. **Dependency Inversion Principle (DIP)**
   - High-level modules should not depend on low-level modules. Both should depend on abstractions.

## Backend Rules

### Development Environment and Workflow
- All execution, debugging, and testing should be done in Visual Studio Enterprise.
- Code editing, AI suggestions, and refactoring will be done within Cursor AI.
- Assume Visual Studio is installed and will be used to compile and launch the application.

### Code Style and Structure
- Write idiomatic and efficient C# code.
- Adhere to .NET Core and Angular conventions.
- Separate complex business logic into service classes.
- Provide non-blocking operations using async/await throughout the application.

### Naming Conventions
- Use PascalCase for component names, method names, and public members.
- Use camelCase for private fields and local variables.
- Prefix interface names with "I" (e.g., IUserService).

### Error Management and Validation
- Implement proper error management for API calls.
- Use FluentValidation or DataAnnotations for form validation.

### API Design and Integration
- Communicate with external APIs or your own backend using HttpClient or appropriate services.
- Implement error management using try-catch for API calls and provide appropriate feedback in the user interface.

### Security and Authentication
- Implement authentication and authorization using ASP.NET Identity or JWT tokens where necessary.
- Use HTTPS for all web communications and implement appropriate CORS policies.

### API Documentation and Swagger
- Create API documentation using Swagger/OpenAPI for your backend API services.
- Provide XML documentation for models and API methods to enhance Swagger documentation.

## Best Practices for Angular, SASS, and TypeScript

### Core Principles
- **Provide Clear and Precise Examples**: Share Angular and TypeScript examples with clear explanations.
- **Immutability and Pure Functions**: Apply immutability principles and pure functions within services and state management.
- **Component Composition**: Prefer component composition to enhance modularity.
- **Meaningful Naming**: Use descriptive variable names like `isUserLoggedIn`, `userPermissions`, `fetchData()`.
- **File Naming**: Use kebab-case for files (e.g., `user-profile.component.ts`).

### TypeScript and Angular
- **Type Safety with Interfaces**: Define data models with interfaces for explicit types and avoid using `any`.
- **Full Use of TypeScript**: Enhance code reliability by using TypeScript's type system instead of `any`.
- **Code Structure**: Structure files with imports at the top, followed by class definition, properties, methods, and finally exports.
- **Optional Chaining and Nullish Coalescing**: Use optional chaining (`?.`) and nullish coalescing (`??`) to prevent null/undefined errors.
- **Standalone Components**: Use standalone components to increase code reusability without relying on Angular modules.
- **Reactive State Management with Signals**: Use Angular's signal system for efficient and reactive programming.
- **Direct Service Injection**: Inject services directly within component, directive, or service logic using the `inject` function.

### File Structure and Naming
- **Component Files**: `*.component.ts`
- **Service Files**: `*.service.ts`
- **Module Files**: `*.module.ts`
- **Directive Files**: `*.directive.ts`
- **Pipe Files**: `*.pipe.ts`
- **Test Files**: `*.spec.ts`
- **General Naming**: Maintain consistency and predictability by using kebab-case for all file names.

### Coding Standards
- Use single quotes (`'`).
- Indent with 2 spaces.
- Avoid unnecessary whitespace and unused variables.
- Use `const` for constants and immutable variables.
- Use template strings for string interpolation and multi-line strings.

### Angular-Specific Development Guidelines
- Use the `async` pipe for observables in templates.
- Enable lazy loading for feature modules.
- Use semantic HTML and relevant ARIA attributes to ensure accessibility.
- Use Angular's signal system for efficient reactive state management.
- Optimize image loading with `NgOptimizedImage` and prevent broken links in case of errors.
- Implement deferrable views to delay rendering of unnecessary components.

### Import Order
1. Angular core and common modules
2. RxJS modules
3. Angular-specific modules (e.g., `FormsModule`)
4. Core application imports
5. Shared module imports
6. Environment-specific imports (e.g., `environment.ts`)
7. Relative path imports

### Error Management and Validation
- Implement robust error management in services and components, using custom error types or error factories where necessary.
- Implement validation through Angular's form validation system or custom validators.

### Testing and Code Quality
- Follow the Arrange-Act-Assertiut coverage with well-defined unit tests for services, components, and utilities.

### Performance Optimization
- Use trackBy functions with `ngFor` to optimize list rendering.
- Implement pure pipes to ensure recalculations only occur on input changes.
- Avoid direct DOM manipulation, use Angular's template engine.
- Use Angular's signal system to reduce unnecessary re-renders and optimize state management.
- Use `NgOptimizedImage` for faster, more efficient image loading.

### Security Best Practices
- Prevent XSS by relying on Angular's built-in sanitization system and avoid using `innerHTML`.
- Sanitize dynamic content using Angular's trusted sanitization methods to prevent security vulnerabilities.

### Core Principles
- Facilitate service injections using Angular's dependency injection and the `inject` function.
- Focus on writing reusable, modular code that aligns with Angular's style guide and industry best practices.
- Continuously optimize Core Web Vitals, especially Largest Contentful Paint (LCP), Interaction to Next Paint (INP), and Cumulative Layout Shift (CLS).

### Reference
Refer to Angular's official documentation to maintain best practices and code quality and sustainability for components, services, and modules.

## Architecture Quality Metrics and Improvements

### Current Architecture Analysis
- **Data Access Layer**: 70%
  - Basic EF Core implementation
  - Direct database operations
  - Missing repository pattern
  - Limited query optimization

- **Business Layer**: 75%
  - Basic service implementations
  - Some business logic in controllers
  - Missing CQRS pattern
  - Limited use of interfaces

- **Presentation Layer**: 90%
  - Clean Angular implementation
  - PrimeNG integration
  - Responsive design
  - Type-safe implementations

- **Cross-Cutting Concerns**: 95%
  - Global exception handling
  - JWT authentication
  - Serilog implementation
  - Audit logging

- **Domain Layer**: 85%
  - Clear entity definitions
  - Basic validation rules
  - Some business rules in entities
  - Missing domain services

### Target Architecture Improvements

#### Data Access Layer Improvements (Target: 95%)
```csharp
public interface IRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    IQueryable<T> Query();
}

public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Product> Products { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
```

#### Business Layer Improvements (Target: 95%)
```csharp
public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(int id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> CreateProductAsync(CreateProductDto dto);
    Task UpdateProductAsync(UpdateProductDto dto);
    Task DeleteProductAsync(int id);
}

// CQRS Implementation
public interface IQuery<TResult> { }
public interface ICommand { }

public interface IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

public interface ICommandHandler<TCommand> 
    where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}
```

### Implementation Plan

1. **Repository Pattern Implementation**
   - Create base repository interface and class
   - Implement entity-specific repositories
   - Add caching mechanism
   - Implement query specifications

2. **Unit of Work Pattern**
   - Implement transaction management
   - Add SaveChanges with validation
   - Implement rollback mechanism
   - Add concurrency handling

3. **Service Layer Refactoring**
   - Move business logic from controllers
   - Implement service interfaces
   - Add validation layer
   - Implement business rules

4. **CQRS Implementation**
   - Create command and query models
   - Implement handlers
   - Add MediatR integration
   - Implement event sourcing

5. **Cross-Cutting Concerns Enhancement**
   - Improve logging granularity
   - Add performance monitoring
   - Enhance security measures
   - Implement caching strategy

### Quality Assurance Measures

1. **Code Quality**
   - SonarQube analysis
   - Code coverage > 80%
   - Regular code reviews
   - Performance profiling

2. **Testing Strategy**
   - Unit tests for all layers
   - Integration tests for flows
   - E2E tests for critical paths
   - Performance benchmarks

3. **Documentation**
   - API documentation
   - Architecture diagrams
   - Code comments
   - Knowledge base updates

4. **Monitoring**
   - Application metrics
   - Error tracking
   - Performance monitoring
   - User behavior analytics

@.cursorrules @scratchpad.md @Roadmap.md 

## Frontend Architecture Guidelines

### Component Architecture
```typescript
@Component({
    selector: 'app-user-list',
    standalone: true,
    imports: [CommonModule, PrimeNgModule],
    template: `...`
})
export class UserListComponent {
    private readonly userService = inject(UserService);
    users = signal<User[]>([]);
    
    ngOnInit() {
        this.loadUsers();
    }
    
    async loadUsers() {
        this.users.set(await this.userService.getUsers());
    }
}
```

### State Management
```typescript
export interface AppState {
    users: User[];
    roles: Role[];
    currentUser: User | null;
}

export const initialState: AppState = {
    users: [],
    roles: [],
    currentUser: null
};

export const appFeature = createFeature({
    name: 'app',
    reducer: createReducer(
        initialState,
        on(loadUsers, (state) => ({ ...state })),
        on(loadUsersSuccess, (state, { users }) => ({ ...state, users }))
    )
});
```

### Performance Optimization
```typescript
@Component({
    selector: 'app-user-table',
    template: `
        <p-table 
            [value]="users()" 
            [virtualScroll]="true"
            [rows]="10"
            [lazy]="true"
            (onLazyLoad)="loadUsers($event)">
            <!-- table content -->
        </p-table>
    `
})
export class UserTableComponent {
    users = signal<User[]>([]);
    
    @Input() set data(value: User[]) {
        this.users.set(value);
    }
    
    trackByFn(index: number, item: User): number {
        return item.id;
    }
}
```

## DevOps ve CI/CD Guidelines

### GitHub Actions Workflow
```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v2
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '9.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

### Docker Deployment
```dockerfile
# API Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Stock.API/Stock.API.csproj", "Stock.API/"]
RUN dotnet restore "Stock.API/Stock.API.csproj"
COPY . .
WORKDIR "/src/Stock.API"
RUN dotnet build "Stock.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stock.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stock.API.dll"]
```

## Database Management Guidelines

### Migration Strategy
```csharp
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(GetConnectionString());
        
        return new ApplicationDbContext(optionsBuilder.Options);
    }
    
    private string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            
        return configuration.GetConnectionString("DefaultConnection");
    }
}
```

### Performance Optimization
```csharp
public class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(connectionString, options => {
                options.EnableRetryOnFailure(3);
                options.CommandTimeout(30);
                options.MaxBatchSize(100);
            })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
            
        // Relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### Backup and Recovery
```powershell
# Backup script
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupPath = "C:\Backups\db_backup_$timestamp.sql"
$connectionString = "Host=localhost;Database=stockdb;Username=postgres;Password=yourpassword"

pg_dump -h localhost -U postgres -F c -b -v -f $backupPath stockdb

# Restore script
$backupFile = "db_backup_20250225_120000.sql"
$connectionString = "Host=localhost;Database=stockdb;Username=postgres;Password=yourpassword"

pg_restore -h localhost -U postgres -d stockdb -v $backupFile
```
