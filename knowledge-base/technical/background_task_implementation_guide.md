# Rehber: .NET Core'da Arka Plan Görev Kuyruğu (Background Task Queue) Implementasyonu

**Son Güncelleme:** 26.06.2025

## 1. Amaç

Bu rehber, .NET Core uygulamalarında uzun süren işlemleri (örneğin, e-posta gönderimi, rapor oluşturma, harici API'lerle senkronizasyon) ana işlem akışını (request-response cycle) ve kullanıcı arayüzünü kilitlemeden, arka planda asenkron olarak çalıştırmak için bir altyapının nasıl kurulacağını açıklar.

Bu yapı, "fire-and-forget" (ateşle ve unut) türündeki görevler için idealdir ve sistemin yanıt verme kabiliyetini ve ölçeklenebilirliğini artırır.

## 2. Kullanılan Bileşenler ve Mimari

Bu implementasyon, .NET Core'un yerleşik özelliklerini kullanır ve harici bir kütüphane (Hangfire, RabbitMQ vb.) bağımlılığı olmadan hafif bir çözüm sunar.

-   **`IBackgroundTaskQueue` (Arayüz):**
    -   **Konum:** `Stock.Application/Common/Interfaces/IBackgroundTaskQueue.cs`
    -   **Sorumluluk:** Arka plan görev kuyruğunun kontratını tanımlar. Görevlerin kuyruğa eklenmesi (`QueueBackgroundWorkItem`) ve kuyruktan alınması (`DequeueAsync`) için metotlar içerir.

-   **`BackgroundTaskQueue` (Implementasyon):**
    -   **Konum:** `Stock.Infrastructure/Services/BackgroundTaskQueue.cs`
    -   **Sorumluluk:** `IBackgroundTaskQueue` arayüzünü implemente eder. Görevleri thread-safe bir şekilde depolamak için `ConcurrentQueue<T>` kullanır. Kuyruğa yeni bir iş eklendiğinde bekleyen işleyiciyi haberdar etmek için bir `SemaphoreSlim` sinyal mekanizması kullanır.

-   **`QueuedHostedService` (Arka Plan Servisi):**
    -   **Konum:** `Stock.Infrastructure/Services/QueuedHostedService.cs`
    -   **Sorumluluk:** .NET Core'un `IHostedService` mekanizmasının bir parçası olan `BackgroundService` sınıfından türer. Uygulama başladığında arka planda otomatik olarak çalışmaya başlar. Sürekli bir döngü içinde `IBackgroundTaskQueue`'dan görevleri çeker (`DequeueAsync`) ve bunları güvenli bir şekilde (`try-catch` bloğu içinde) yürütür.

## 3. Kurulum ve Yapılandırma

Bu bileşenlerin uygulama tarafından tanınması ve kullanılması için Dependency Injection (DI) container'ına kaydedilmesi gerekir.

-   **Konum:** `Stock.Infrastructure/DependencyInjection.cs`
-   **Yapılandırma:**
    ```csharp
    // IBackgroundTaskQueue ve implementasyonunu Singleton olarak kaydet.
    // Bu, uygulama boyunca tek bir görev kuyruğunun olmasını sağlar.
    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

    // QueuedHostedService'i barındırılan bir hizmet olarak kaydet.
    // Bu, uygulama başladığında servisin arka planda otomatik olarak başlamasını sağlar.
    services.AddHostedService<QueuedHostedService>();
    ```

## 4. Kullanım Örneği

Altyapı kurulduktan sonra, herhangi bir servise veya command handler'a `IBackgroundTaskQueue` arayüzünü inject ederek yeni arka plan görevleri kolayca eklenebilir.

-   **Örnek Konum:** `Stock.Application/Features/Products/Commands/CreateProduct/CreateProductCommandHandler.cs`
-   **Uygulama:**
    1.  `CreateProductCommandHandler`'ın constructor'ına `IBackgroundTaskQueue backgroundTaskQueue` parametresi eklenir ve inject edilir.
    2.  Ana işlem (ürünü veritabanına kaydetme) tamamlandıktan sonra, yeni bir arka plan görevi kuyruğa eklenir.

    ```csharp
    public class CreateProductCommandHandler 
    {
        // ... (constructor ve diğer metotlar)

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // ... (Ürün oluşturma ve veritabanına kaydetme işlemleri)

            // İşlem başarılı olduktan sonra arka plan görevini kuyruğa ekle
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                // Bu kod bloğu arka planda, ayrı bir scope'ta çalışacaktır.
                // UZUN SÜREN İŞLEMLER BURAYA YAZILIR:
                
                // Örnek: Loglama ve simüle edilmiş bir gecikme
                _logger.LogInformation("BACKGROUND JOB: Yeni ürün eklendi: {ProductName}", product.Name.Value);
                await Task.Delay(TimeSpan.FromSeconds(5), token);
                _logger.LogInformation("BACKGROUND JOB: Bildirim görevi tamamlandı.");

                // Not: Eğer bu blok içinde veritabanı gibi scoped servisler kullanılacaksa,
                // IServiceProvider üzerinden yeni bir scope oluşturulmalıdır.
            });

            // ... (DTO'yu oluştur ve sonucu dön)
        }
    }
    ```

Bu yapı, projedeki asenkron işlemleri yönetmek için standart ve yeniden kullanılabilir bir desen sağlar. 