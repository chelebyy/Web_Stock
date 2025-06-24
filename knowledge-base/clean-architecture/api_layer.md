# API Katmanı Geçişi

## Genel Bakış

API katmanı, Clean Architecture'ın en dış katmanıdır ve kullanıcı arayüzü ile etkileşimi sağlar. Bu katman, HTTP isteklerini karşılar, Application katmanındaki komutları ve sorguları çağırır ve sonuçları istemciye döndürür. API katmanı, Application ve Infrastructure katmanlarına bağımlıdır.

## Taşınacak Bileşenler

1. **Controller'lar**
   - UserController
   - RoleController
   - AuthController
   - Diğer controller'lar

2. **Middleware'ler**
   - ExceptionHandlingMiddleware
   - RequestLoggingMiddleware
   - JwtMiddleware

3. **Program.cs ve Startup Yapılandırması**
   - Dependency Injection
   - Authentication/Authorization
   - CORS yapılandırması
   - Swagger/OpenAPI yapılandırması

## Geçiş Adımları

### 1. NuGet Paketleri

API katmanı için gerekli NuGet paketlerini ekleyelim:

```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Microsoft.AspNetCore.OpenApi
dotnet add package Swashbuckle.AspNetCore
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File
```

### 2. Program.cs Yapılandırması

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Stock.API.Middleware;
using Stock.Application;
using Stock.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/stock-api-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Add Application Layer
builder.Services.AddApplication();

// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
    
    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Add custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
```

### 3. appsettings.json Yapılandırması

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=stock;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "your-secret-key-with-at-least-32-characters",
    "Issuer": "stock-api",
    "Audience": "stock-client",
    "ExpiryInDays": 7
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    }
  },
  "AllowedHosts": "*"
}
```

### 4. Middleware'ler

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = context.Response.StatusCode,
                message = "An error occurred while processing your request.",
                detailedMessage = exception.Message
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} started");

            var originalBodyStream = context.Response.Body;
            try
            {
                await _next(context);
            }
            finally
            {
                _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} completed with status code {context.Response.StatusCode}");
            }
        }
    }
}
```

### 5. Controller'lar

```csharp
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Models.DTOs;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest();
                
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var query = new LoginQuery { LoginDto = loginDto };
            var result = await _mediator.Send(query);
            
            if (!result.Success)
                return Unauthorized(result);
                
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var command = new RegisterCommand { RegisterDto = registerDto };
            var result = await _mediator.Send(command);
            
            if (!result.Success)
                return BadRequest(result);
                
            return Ok(result);
        }
    }
}
```

## Dikkat Edilecek Noktalar

1. **CQRS Entegrasyonu**
   - Controller'lar MediatR kullanarak CQRS komutlarını ve sorgularını çağırmalı
   - Her endpoint için uygun komut veya sorgu kullanılmalı

2. **Authentication/Authorization**
   - JWT tabanlı kimlik doğrulama kullanılmalı
   - Role tabanlı yetkilendirme uygulanmalı
   - Hassas endpoint'ler için yetkilendirme kontrolleri yapılmalı

3. **Hata Yönetimi**
   - Global exception handling middleware kullanılmalı
   - Anlamlı hata mesajları döndürülmeli
   - Loglama yapılmalı

4. **Swagger/OpenAPI**
   - API dokümantasyonu için Swagger kullanılmalı
   - JWT kimlik doğrulama için Swagger yapılandırılmalı

5. **CORS Yapılandırması**
   - Frontend uygulamasının API'ye erişebilmesi için CORS yapılandırılmalı

## Sonraki Adımlar

API katmanı tamamlandıktan sonra:

1. Unit ve integration testler yazılacak
2. Frontend uygulaması yeni API'ye uyarlanacak
3. Performans testleri yapılacak
4. Güvenlik testleri yapılacak 