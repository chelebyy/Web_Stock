# Mikroservis Mimarisine Geçiş Hazırlıkları

## Genel Bakış

Mikroservis mimarisi, büyük ve karmaşık uygulamaları daha küçük, bağımsız olarak dağıtılabilen ve ölçeklendirilebilen hizmetlere bölen bir yazılım geliştirme yaklaşımıdır. Bu belge, mevcut monolitik Stock uygulamasının mikroservis mimarisine geçiş hazırlıklarını detaylandırmaktadır.

## Neden Mikroservis Mimarisi?

1. **Ölçeklenebilirlik**: Her servis bağımsız olarak ölçeklendirilebilir.
2. **Esneklik**: Farklı servisler için farklı teknolojiler kullanılabilir.
3. **Dayanıklılık**: Bir servisin çökmesi tüm uygulamayı etkilemez.
4. **Bağımsız Dağıtım**: Servisler bağımsız olarak geliştirilebilir ve dağıtılabilir.
5. **Takım Organizasyonu**: Farklı takımlar farklı servislere odaklanabilir.

## Mevcut Durum Analizi

Mevcut Stock uygulaması, aşağıdaki ana modüllerden oluşan monolitik bir yapıdadır:

1. **Kullanıcı Yönetimi**: Kullanıcı, rol ve izin yönetimi
2. **Stok Yönetimi**: Ürün, kategori ve stok işlemleri
3. **Sipariş İşleme**: Sipariş oluşturma, takip ve yönetimi
4. **Raporlama**: Çeşitli raporlar ve analizler

## Mikroservis Mimarisine Geçiş Stratejisi

### 1. Domain-Driven Design ile Bounded Context'lerin Belirlenmesi

Domain-Driven Design prensipleri kullanılarak, uygulamanın bounded context'leri belirlenmiştir:

- **Kimlik Servisi (Identity Service)**
  - Kullanıcı yönetimi
  - Rol ve izin yönetimi
  - Kimlik doğrulama ve yetkilendirme

- **Katalog Servisi (Catalog Service)**
  - Ürün yönetimi
  - Kategori yönetimi
  - Ürün arama ve filtreleme

- **Envanter Servisi (Inventory Service)**
  - Stok yönetimi
  - Stok hareketleri
  - Stok uyarıları

- **Sipariş Servisi (Order Service)**
  - Sipariş oluşturma
  - Sipariş takibi
  - Sipariş geçmişi

- **Raporlama Servisi (Reporting Service)**
  - İş zekası raporları
  - Performans analizleri
  - Özel raporlar

### 2. Servis Sınırlarının Belirlenmesi

Her bir mikroservis için:

- **Veri Modeli**: Her servisin kendi veritabanı olacak
- **API Sınırları**: Her servisin sunacağı API'ler
- **Servisler Arası İletişim**: Senkron (HTTP/gRPC) ve asenkron (Mesaj Kuyrukları) iletişim yöntemleri

### 3. Teknoloji Seçimleri

#### Ortak Teknolojiler

- **Geliştirme Platformu**: .NET Core
- **API Gateway**: Ocelot
- **Servis Keşfi**: Consul
- **Mesaj Kuyrukları**: RabbitMQ
- **Konteynerizasyon**: Docker
- **Orkestrasyon**: Kubernetes
- **Dağıtık İzleme**: OpenTelemetry
- **Loglama**: ELK Stack (Elasticsearch, Logstash, Kibana)
- **Kimlik Doğrulama**: IdentityServer4

#### Servis Özelinde Teknolojiler

- **Kimlik Servisi**: ASP.NET Core Identity, IdentityServer4
- **Katalog Servisi**: ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- **Envanter Servisi**: ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- **Sipariş Servisi**: ASP.NET Core Web API, Entity Framework Core, PostgreSQL
- **Raporlama Servisi**: ASP.NET Core Web API, Dapper, PostgreSQL

### 4. Veritabanı Stratejisi

- **Veritabanı Per Servis**: Her mikroservis kendi veritabanını yönetecek
- **Veri Tutarlılığı**: Eventual Consistency modeli kullanılacak
- **Veri Replikasyonu**: Gerektiğinde servisler arası veri replikasyonu için CDC (Change Data Capture) kullanılacak

### 5. API Gateway Yapılandırması

API Gateway, istemciler ile mikroservisler arasında bir ara katman olarak görev yapacak ve aşağıdaki işlevleri sağlayacak:

- **Rota Yönetimi**: İstemci isteklerini ilgili servislere yönlendirme
- **Kimlik Doğrulama**: Merkezi kimlik doğrulama
- **Yetkilendirme**: Rol tabanlı erişim kontrolü
- **Hız Sınırlama**: API rate limiting
- **Önbellek**: Yanıt önbelleği
- **Yük Dengeleme**: Servis örnekleri arasında yük dengeleme
- **Devre Kesici**: Hata durumlarında servisleri koruma

### 6. Servisler Arası İletişim

#### Senkron İletişim (Request/Response)

- **HTTP/REST**: Basit ve yaygın kullanım için
- **gRPC**: Yüksek performans gerektiren servisler arası iletişim için

#### Asenkron İletişim (Event-Driven)

- **RabbitMQ**: Mesaj kuyrukları ile asenkron iletişim
- **Event Sourcing**: Domain olaylarının kaydedilmesi ve işlenmesi

### 7. Geçiş Planı

Mikroservis mimarisine geçiş, aşağıdaki aşamalı yaklaşımla gerçekleştirilecektir:

#### Aşama 1: Altyapı Hazırlığı

1. **Konteynerizasyon**: Mevcut uygulamanın Docker konteynerlerine taşınması
2. **CI/CD Pipeline**: Sürekli entegrasyon ve dağıtım süreçlerinin kurulması
3. **Servis Keşfi ve Yapılandırma**: Consul ve Vault entegrasyonu
4. **Merkezi Loglama ve İzleme**: ELK Stack ve OpenTelemetry kurulumu

#### Aşama 2: Strangler Pattern Uygulaması

1. **API Gateway Kurulumu**: Ocelot ile API Gateway kurulumu
2. **Kimlik Servisi Ayrıştırması**: İlk olarak Kimlik Servisi'nin ayrıştırılması
3. **Katalog Servisi Ayrıştırması**: Ürün ve kategori yönetiminin ayrıştırılması
4. **Envanter Servisi Ayrıştırması**: Stok yönetiminin ayrıştırılması
5. **Sipariş Servisi Ayrıştırması**: Sipariş işlemlerinin ayrıştırılması
6. **Raporlama Servisi Ayrıştırması**: Raporlama işlevlerinin ayrıştırılması

#### Aşama 3: Servisler Arası İletişim

1. **Mesaj Kuyrukları Kurulumu**: RabbitMQ kurulumu ve yapılandırması
2. **Event Bus Implementasyonu**: Servisler arası olay iletimi için event bus oluşturulması
3. **Saga Pattern Uygulaması**: Dağıtık işlemler için saga pattern uygulanması

#### Aşama 4: Ölçeklendirme ve Dayanıklılık

1. **Kubernetes Kurulumu**: Kubernetes cluster kurulumu
2. **Otomatik Ölçeklendirme**: Horizontal Pod Autoscaler yapılandırması
3. **Devre Kesici Uygulaması**: Polly ile devre kesici pattern uygulanması
4. **Yük Testi ve Optimizasyon**: Performans testleri ve optimizasyonlar

## Örnek Mikroservis Yapısı

### Kimlik Servisi (Identity.API)

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // IdentityServer4 yapılandırması
    services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        .AddInMemoryApiResources(Config.GetApiResources())
        .AddInMemoryClients(Config.GetClients())
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddAspNetIdentity<ApplicationUser>();

    // CORS, Authentication, Authorization
    services.AddCors();
    services.AddAuthentication();
    services.AddAuthorization();
    
    // MVC ve API Controller'lar
    services.AddControllers();
}
```

### API Gateway (Gateway.API)

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Ocelot API Gateway
    services.AddOcelot(Configuration)
        .AddConsul()
        .AddCacheManager(x => x.WithDictionaryHandle());
        
    // Authentication ve Authorization
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            // JWT yapılandırması
        });
        
    services.AddAuthorization();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Middleware pipeline
    app.UseRouting();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
    });
    
    // Ocelot middleware
    app.UseOcelot().Wait();
}
```

### Katalog Servisi (Catalog.API)

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Veritabanı bağlantısı
    services.AddDbContext<CatalogContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("CatalogConnection")));
        
    // Repository ve servis kayıtları
    services.AddScoped<IProductRepository, ProductRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<IProductService, ProductService>();
    
    // MediatR ve CQRS
    services.AddMediatR(typeof(Startup).Assembly);
    
    // Event Bus
    services.AddSingleton<IEventBus, RabbitMQEventBus>();
    
    // API Controller'lar
    services.AddControllers();
    
    // Swagger
    services.AddSwaggerGen();
}
```

### Envanter Servisi (Inventory.API)

```csharp
// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Veritabanı bağlantısı
    services.AddDbContext<InventoryContext>(options =>
        options.UseNpgsql(Configuration.GetConnectionString("InventoryConnection")));
        
    // Repository ve servis kayıtları
    services.AddScoped<IStockRepository, StockRepository>();
    services.AddScoped<IStockService, StockService>();
    
    // MediatR ve CQRS
    services.AddMediatR(typeof(Startup).Assembly);
    
    // Event Bus
    services.AddSingleton<IEventBus, RabbitMQEventBus>();
    services.AddTransient<ProductStockChangedEventHandler>();
    
    // API Controller'lar
    services.AddControllers();
    
    // Swagger
    services.AddSwaggerGen();
}
```

## Servisler Arası İletişim Örneği

### Event Bus Arayüzü

```csharp
// EventBus/IEventBus.cs
public interface IEventBus
{
    void Publish(IntegrationEvent @event);
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
}
```

### RabbitMQ Event Bus Implementasyonu

```csharp
// EventBus/RabbitMQEventBus.cs
public class RabbitMQEventBus : IEventBus
{
    private readonly IRabbitMQPersistentConnection _persistentConnection;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RabbitMQEventBus> _logger;
    
    // Constructor ve diğer metotlar...
    
    public void Publish(IntegrationEvent @event)
    {
        // RabbitMQ'ya mesaj yayınlama
    }
    
    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        // Event handler'ı abone etme
    }
    
    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        // Event handler aboneliğini kaldırma
    }
}
```

### Entegrasyon Olayı Örneği

```csharp
// Events/ProductStockChangedEvent.cs
public class ProductStockChangedIntegrationEvent : IntegrationEvent
{
    public int ProductId { get; private set; }
    public int NewStockQuantity { get; private set; }
    
    public ProductStockChangedIntegrationEvent(int productId, int newStockQuantity)
    {
        ProductId = productId;
        NewStockQuantity = newStockQuantity;
    }
}
```

### Olay İşleyici Örneği

```csharp
// EventHandlers/ProductStockChangedEventHandler.cs
public class ProductStockChangedEventHandler : IIntegrationEventHandler<ProductStockChangedIntegrationEvent>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<ProductStockChangedEventHandler> _logger;
    
    public ProductStockChangedEventHandler(
        IStockRepository stockRepository,
        ILogger<ProductStockChangedEventHandler> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }
    
    public async Task Handle(ProductStockChangedIntegrationEvent @event)
    {
        _logger.LogInformation($"Handling ProductStockChanged event: {JsonSerializer.Serialize(@event)}");
        
        // Stok bilgisini güncelleme
        await _stockRepository.UpdateStockQuantityAsync(@event.ProductId, @event.NewStockQuantity);
    }
}
```

## Docker ve Kubernetes Yapılandırması

### Docker Compose Örneği

```yaml
# docker-compose.yml
version: '3.4'

services:
  # API Gateway
  gateway.api:
    image: ${REGISTRY:-stock}/gateway.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Gateway/Gateway.API/Dockerfile
    ports:
      - "80:80"
      - "443:443"
    depends_on:
      - identity.api
      - catalog.api
      - inventory.api
      - order.api
      - reporting.api

  # Kimlik Servisi
  identity.api:
    image: ${REGISTRY:-stock}/identity.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Services/Identity/Identity.API/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionString=Server=postgres;Port=5432;Database=IdentityDb;User Id=postgres;Password=postgres;
    ports:
      - "5101:80"
    depends_on:
      - postgres
      - rabbitmq

  # Katalog Servisi
  catalog.api:
    image: ${REGISTRY:-stock}/catalog.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Services/Catalog/Catalog.API/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionString=Server=postgres;Port=5432;Database=CatalogDb;User Id=postgres;Password=postgres;
    ports:
      - "5102:80"
    depends_on:
      - postgres
      - rabbitmq

  # Envanter Servisi
  inventory.api:
    image: ${REGISTRY:-stock}/inventory.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Services/Inventory/Inventory.API/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionString=Server=postgres;Port=5432;Database=InventoryDb;User Id=postgres;Password=postgres;
    ports:
      - "5103:80"
    depends_on:
      - postgres
      - rabbitmq

  # Sipariş Servisi
  order.api:
    image: ${REGISTRY:-stock}/order.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Services/Order/Order.API/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionString=Server=postgres;Port=5432;Database=OrderDb;User Id=postgres;Password=postgres;
    ports:
      - "5104:80"
    depends_on:
      - postgres
      - rabbitmq

  # Raporlama Servisi
  reporting.api:
    image: ${REGISTRY:-stock}/reporting.api:${TAG:-latest}
    build:
      context: .
      dockerfile: src/Services/Reporting/Reporting.API/Dockerfile
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionString=Server=postgres;Port=5432;Database=ReportingDb;User Id=postgres;Password=postgres;
    ports:
      - "5105:80"
    depends_on:
      - postgres
      - rabbitmq

  # PostgreSQL
  postgres:
    image: postgres:latest
    environment:
      - POSTGRES_PASSWORD=postgres
    ports:
      - "5432:5432"
    volumes:
      - postgres-data:/var/lib/postgresql/data

  # RabbitMQ
  rabbitmq:
    image: rabbitmq:3-management
    ports:
      - "15672:15672"
      - "5672:5672"
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq

  # Consul
  consul:
    image: consul:latest
    ports:
      - "8500:8500"
    volumes:
      - consul-data:/consul/data

  # Elasticsearch
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:7.9.2
    environment:
      - discovery.type=single-node
    ports:
      - "9200:9200"
    volumes:
      - elasticsearch-data:/usr/share/elasticsearch/data

  # Kibana
  kibana:
    image: docker.elastic.co/kibana/kibana:7.9.2
    ports:
      - "5601:5601"
    depends_on:
      - elasticsearch

volumes:
  postgres-data:
  rabbitmq-data:
  consul-data:
  elasticsearch-data:
```

### Kubernetes Deployment Örneği

```yaml
# kubernetes/gateway-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: gateway
  labels:
    app: gateway
spec:
  replicas: 2
  selector:
    matchLabels:
      app: gateway
  template:
    metadata:
      labels:
        app: gateway
    spec:
      containers:
      - name: gateway
        image: stock/gateway.api:latest
        ports:
        - containerPort: 80
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: Production
        resources:
          limits:
            cpu: "500m"
            memory: "512Mi"
          requests:
            cpu: "100m"
            memory: "256Mi"
        livenessProbe:
          httpGet:
            path: /health/live
            port: 80
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /health/ready
            port: 80
          initialDelaySeconds: 15
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: gateway
spec:
  selector:
    app: gateway
  ports:
  - port: 80
    targetPort: 80
  type: LoadBalancer
```

## Sonuç ve Faydalar

Mikroservis mimarisine geçiş, aşağıdaki faydaları sağlayacaktır:

1. **Ölçeklenebilirlik**: Her servis bağımsız olarak ölçeklendirilebilecek, böylece sistem kaynakları daha verimli kullanılacaktır.
2. **Esneklik**: Farklı servisler için farklı teknolojiler kullanılabilecek, böylece her servis için en uygun teknoloji seçilebilecektir.
3. **Dayanıklılık**: Bir servisin çökmesi tüm uygulamayı etkilemeyecek, böylece sistem daha dayanıklı olacaktır.
4. **Bağımsız Dağıtım**: Servisler bağımsız olarak geliştirilebilecek ve dağıtılabilecek, böylece daha hızlı ve güvenli dağıtımlar yapılabilecektir.
5. **Takım Organizasyonu**: Farklı takımlar farklı servislere odaklanabilecek, böylece geliştirme süreci daha verimli olacaktır.

Bu geçiş, uygulamanın daha modüler, ölçeklenebilir ve bakımı daha kolay bir yapıya kavuşmasını sağlayacaktır. 