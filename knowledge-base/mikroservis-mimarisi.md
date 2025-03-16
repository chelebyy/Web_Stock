# Mikroservis Mimarisine Geçiş Hazırlığı

## Genel Bakış

Bu belge, mevcut monolitik Stock uygulamasının mikroservis mimarisine geçiş hazırlığını detaylandırmaktadır. Mikroservis mimarisi, büyük ve karmaşık uygulamaları daha küçük, bağımsız olarak dağıtılabilen ve ölçeklendirilebilen hizmetlere bölmeyi amaçlayan bir yazılım mimarisi yaklaşımıdır.

## Neden Mikroservis Mimarisi?

1. **Ölçeklenebilirlik**: Her bir servis bağımsız olarak ölçeklendirilebilir.
2. **Esneklik**: Farklı teknolojiler ve diller kullanılabilir.
3. **Dayanıklılık**: Bir servisin çökmesi tüm sistemi etkilemez.
4. **Bağımsız Dağıtım**: Servisler bağımsız olarak geliştirilebilir ve dağıtılabilir.
5. **Takım Organizasyonu**: Takımlar belirli servisler üzerinde uzmanlaşabilir.

## Mevcut Durum Analizi

Mevcut Stock uygulaması, aşağıdaki bileşenlerden oluşan monolitik bir yapıya sahiptir:

1. **Kullanıcı Yönetimi**: Kullanıcı, rol ve izin yönetimi
2. **Ürün Kataloğu**: Ürün ve kategori yönetimi
3. **Stok Yönetimi**: Stok hareketleri ve envanter takibi
4. **Sipariş İşleme**: Sipariş oluşturma ve takibi
5. **Raporlama**: Çeşitli raporlar ve analizler

## Geçiş Stratejisi

### 1. Bounded Context'lerin Belirlenmesi

Domain-Driven Design prensipleri kullanılarak aşağıdaki bounded context'ler belirlenmiştir:

1. **Identity Service**: Kullanıcı, rol ve izin yönetimi
2. **Catalog Service**: Ürün ve kategori yönetimi
3. **Inventory Service**: Stok hareketleri ve envanter takibi
4. **Order Service**: Sipariş oluşturma ve takibi
5. **Reporting Service**: Raporlama ve analiz

### 2. Servis Sınırlarının Tanımlanması

Her bir servis için aşağıdaki sınırlar tanımlanmıştır:

#### 2.1. Veri Modeli Sınırları

Her servis kendi veritabanına sahip olacak ve sadece kendi domain'ine ait verileri yönetecektir:

- **Identity Service**: User, Role, Permission
- **Catalog Service**: Product, Category
- **Inventory Service**: StockMovement, Warehouse
- **Order Service**: Order, OrderItem
- **Reporting Service**: Report, ReportTemplate

#### 2.2. API Sınırları

Her servis, kendi domain'ine ait işlemleri gerçekleştiren API'ler sunacaktır:

- **Identity Service**: `/api/users`, `/api/roles`, `/api/permissions`
- **Catalog Service**: `/api/products`, `/api/categories`
- **Inventory Service**: `/api/stock`, `/api/warehouses`
- **Order Service**: `/api/orders`
- **Reporting Service**: `/api/reports`

#### 2.3. İletişim Yöntemleri

Servisler arası iletişim için aşağıdaki yöntemler kullanılacaktır:

1. **Senkron İletişim**: REST API veya gRPC
2. **Asenkron İletişim**: Message Queue (RabbitMQ)

### 3. Teknoloji Seçimleri

#### 3.1. Ortak Teknolojiler

- **Backend**: .NET Core
- **API Gateway**: Ocelot
- **Message Queue**: RabbitMQ
- **Containerization**: Docker
- **Orchestration**: Kubernetes
- **Service Discovery**: Consul
- **Monitoring**: Prometheus, Grafana
- **Logging**: ELK Stack (Elasticsearch, Logstash, Kibana)

#### 3.2. Servis Özelinde Teknolojiler

- **Identity Service**: IdentityServer4
- **Reporting Service**: Hangfire (Background Jobs)

### 4. Veritabanı Stratejisi

Her servis kendi veritabanına sahip olacaktır:

1. **Database per Service**: Her servis kendi veritabanını yönetir.
2. **Polyglot Persistence**: Servisler, ihtiyaçlarına göre farklı veritabanı teknolojileri kullanabilir.
   - Identity Service: PostgreSQL
   - Catalog Service: PostgreSQL
   - Inventory Service: PostgreSQL
   - Order Service: PostgreSQL
   - Reporting Service: PostgreSQL + MongoDB (analitik veriler için)

### 5. API Gateway Yapılandırması

Ocelot API Gateway, aşağıdaki işlevleri sağlayacaktır:

1. **Routing**: İstekleri uygun servislere yönlendirme
2. **Authentication**: Merkezi kimlik doğrulama
3. **Rate Limiting**: API istek sınırlaması
4. **Load Balancing**: Yük dengeleme
5. **Caching**: Önbellek mekanizması
6. **Logging**: Merkezi loglama

```json
// ocelot.json
{
  "Routes": [
    {
      "DownstreamPathTemplate": "/api/users/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "identity-service",
          "Port": 443
        }
      ],
      "UpstreamPathTemplate": "/api/users/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
    },
    {
      "DownstreamPathTemplate": "/api/products/{everything}",
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "catalog-service",
          "Port": 443
        }
      ],
      "UpstreamPathTemplate": "/api/products/{everything}",
      "UpstreamHttpMethod": [ "GET", "POST", "PUT", "DELETE" ],
      "AuthenticationOptions": {
        "AuthenticationProviderKey": "Bearer"
      }
    }
    // Diğer servisler için benzer yapılandırmalar
  ],
  "GlobalConfiguration": {
    "BaseUrl": "https://api.stock.com",
    "RequestIdKey": "RequestId",
    "RateLimitOptions": {
      "ClientWhitelist": [],
      "EnableRateLimiting": true,
      "Period": "1s",
      "PeriodTimespan": 1,
      "Limit": 10
    }
  }
}
```

### 6. Servisler Arası İletişim

#### 6.1. Senkron İletişim

REST API veya gRPC kullanılarak servisler arası senkron iletişim sağlanacaktır:

```csharp
// OrderService'den InventoryService'e senkron istek örneği
public class OrderService
{
    private readonly HttpClient _httpClient;
    
    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> CheckInventory(int productId, int quantity)
    {
        var response = await _httpClient.GetAsync($"https://inventory-service/api/stock/check?productId={productId}&quantity={quantity}");
        
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;
        }
        
        return false;
    }
}
```

#### 6.2. Asenkron İletişim

RabbitMQ kullanılarak servisler arası asenkron iletişim sağlanacaktır:

```csharp
// OrderService'den InventoryService'e asenkron mesaj gönderme örneği
public class OrderCreatedPublisher
{
    private readonly IModel _channel;
    
    public OrderCreatedPublisher(IConnection connection)
    {
        _channel = connection.CreateModel();
        _channel.ExchangeDeclare("order_events", ExchangeType.Topic, durable: true);
    }
    
    public void PublishOrderCreated(OrderCreatedEvent @event)
    {
        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);
        
        _channel.BasicPublish(
            exchange: "order_events",
            routingKey: "order.created",
            basicProperties: null,
            body: body);
    }
}

// InventoryService'de OrderCreated olayını dinleme örneği
public class OrderCreatedConsumer : BackgroundService
{
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    
    public OrderCreatedConsumer(IConnection connection, IServiceProvider serviceProvider)
    {
        _channel = connection.CreateModel();
        _serviceProvider = serviceProvider;
        
        _channel.ExchangeDeclare("order_events", ExchangeType.Topic, durable: true);
        _channel.QueueDeclare("inventory_order_created", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind("inventory_order_created", "order_events", "order.created");
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        
        consumer.Received += async (sender, args) =>
        {
            var message = Encoding.UTF8.GetString(args.Body.ToArray());
            var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
            
            using (var scope = _serviceProvider.CreateScope())
            {
                var inventoryService = scope.ServiceProvider.GetRequiredService<IInventoryService>();
                await inventoryService.UpdateStockForOrder(@event);
            }
            
            _channel.BasicAck(args.DeliveryTag, multiple: false);
        };
        
        _channel.BasicConsume("inventory_order_created", autoAck: false, consumer: consumer);
        
        return Task.CompletedTask;
    }
}
```

### 7. Aşamalı Geçiş Planı

Mikroservis mimarisine geçiş, aşağıdaki aşamalarla gerçekleştirilecektir:

#### 7.1. Altyapı Hazırlığı (1-2 Ay)

1. Docker ve Kubernetes ortamının kurulması
2. CI/CD pipeline'larının oluşturulması
3. Monitoring ve logging altyapısının kurulması
4. API Gateway yapılandırması

#### 7.2. Strangler Pattern Uygulaması (3-6 Ay)

1. Identity Service'in ayrılması
2. Catalog Service'in ayrılması
3. Inventory Service'in ayrılması
4. Order Service'in ayrılması
5. Reporting Service'in ayrılması

#### 7.3. Servisler Arası İletişimin Kurulması (1-2 Ay)

1. Senkron iletişim mekanizmalarının kurulması
2. Asenkron iletişim mekanizmalarının kurulması
3. Distributed transaction yönetimi

#### 7.4. Ölçeklenebilirlik ve Dayanıklılık (1-2 Ay)

1. Otomatik ölçeklendirme yapılandırması
2. Circuit breaker pattern uygulaması
3. Retry ve fallback mekanizmalarının eklenmesi

## Örnek Servis Yapılandırmaları

### Identity Service

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDb")));

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<ApplicationUser>();

// Middleware
var app = builder.Build();

app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### API Gateway

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://identity-service";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// Middleware
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseOcelot().Wait();

app.Run();
```

### Catalog Service

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CatalogDb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://identity-service";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    
    return factory.CreateConnection();
});

builder.Services.AddHostedService<ProductUpdatedConsumer>();

// Middleware
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

### Inventory Service

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://identity-service";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

// RabbitMQ
builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        HostName = builder.Configuration["RabbitMQ:HostName"],
        UserName = builder.Configuration["RabbitMQ:UserName"],
        Password = builder.Configuration["RabbitMQ:Password"]
    };
    
    return factory.CreateConnection();
});

builder.Services.AddHostedService<OrderCreatedConsumer>();

// Middleware
var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## Docker Compose Örneği

```yaml
# docker-compose.yml
version: '3.8'

services:
  api-gateway:
    build:
      context: ./src/ApiGateway
    ports:
      - "5000:80"
    depends_on:
      - identity-service
      - catalog-service
      - inventory-service
      - order-service
      - reporting-service
    environment:
      - ASPNETCORE_ENVIRONMENT=Development

  identity-service:
    build:
      context: ./src/IdentityService
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__IdentityDb=Host=postgres;Database=identity;Username=postgres;Password=postgres

  catalog-service:
    build:
      context: ./src/CatalogService
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__CatalogDb=Host=postgres;Database=catalog;Username=postgres;Password=postgres
      - RabbitMQ__HostName=rabbitmq
      - RabbitMQ__UserName=guest
      - RabbitMQ__Password=guest

  inventory-service:
    build:
      context: ./src/InventoryService
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__InventoryDb=Host=postgres;Database=inventory;Username=postgres;Password=postgres
      - RabbitMQ__HostName=rabbitmq
      - RabbitMQ__UserName=guest
      - RabbitMQ__Password=guest

  order-service:
    build:
      context: ./src/OrderService
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__OrderDb=Host=postgres;Database=order;Username=postgres;Password=postgres
      - RabbitMQ__HostName=rabbitmq
      - RabbitMQ__UserName=guest
      - RabbitMQ__Password=guest

  reporting-service:
    build:
      context: ./src/ReportingService
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__ReportingDb=Host=postgres;Database=reporting;Username=postgres;Password=postgres
      - ConnectionStrings__AnalyticsDb=mongodb://mongo:27017/analytics
      - RabbitMQ__HostName=rabbitmq
      - RabbitMQ__UserName=guest
      - RabbitMQ__Password=guest

  postgres:
    image: postgres:13
    ports:
      - "5432:5432"
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
    volumes:
      - postgres-data:/var/lib/postgresql/data

  mongo:
    image: mongo:4.4
    ports:
      - "27017:27017"
    volumes:
      - mongo-data:/data/db

  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq

volumes:
  postgres-data:
  mongo-data:
  rabbitmq-data:
```

## Kubernetes Deployment Örneği

```yaml
# api-gateway-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api-gateway
spec:
  replicas: 2
  selector:
    matchLabels:
      app: api-gateway
  template:
    metadata:
      labels:
        app: api-gateway
    spec:
      containers:
      - name: api-gateway
        image: stock/api-gateway:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: Production
---
apiVersion: v1
kind: Service
metadata:
  name: api-gateway
spec:
  selector:
    app: api-gateway
  ports:
  - port: 80
    targetPort: 80
  type: LoadBalancer
```

## Özet

Mikroservis mimarisine geçiş, Stock uygulamasının ölçeklenebilirliğini, esnekliğini ve dayanıklılığını artıracaktır. Bu belge, geçiş sürecinin planlanması ve uygulanması için bir yol haritası sunmaktadır. Domain-Driven Design prensipleri, bounded context'lerin belirlenmesinde ve servis sınırlarının tanımlanmasında önemli bir rol oynamaktadır.

Geçiş süreci, altyapı hazırlığı, Strangler Pattern uygulaması, servisler arası iletişimin kurulması ve ölçeklenebilirlik/dayanıklılık aşamalarından oluşmaktadır. Her aşama, belirli hedeflere ve zaman çizelgelerine sahiptir.

Mikroservis mimarisine geçiş, teknik zorluklar içermekle birlikte, uzun vadede Stock uygulamasının daha iyi bir şekilde geliştirilmesini ve işletilmesini sağlayacaktır. 