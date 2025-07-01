using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// Arka plan görev kuyruğunu dinleyen ve işleri belirli bir paralellikte yürüten barındırılan hizmet (hosted service).
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger<QueuedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private const int MaxConcurrentTasks = 4; // Aynı anda çalışacak maksimum görev sayısı.

        public QueuedHostedService(IServiceProvider serviceProvider, ILogger<QueuedHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting with a max concurrency of {MaxConcurrentTasks} tasks.", MaxConcurrentTasks);

            var taskQueue = _serviceProvider.GetRequiredService<IBackgroundTaskQueue>();
            using var semaphore = new SemaphoreSlim(MaxConcurrentTasks);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Bir sonraki görevi kuyruktan al.
                var workItem = await taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    // Paralel çalışma için bir slot boşalmasını bekle.
                    await semaphore.WaitAsync(stoppingToken);

                    // Görevi thread pool üzerinde yeni bir Task olarak başlat.
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            _logger.LogInformation("Starting background task.");
                            await workItem(stoppingToken);
                            _logger.LogInformation("Finished background task.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error occurred executing a background task.");
                        }
                        finally
                        {
                            // Görev tamamlandığında veya hata aldığında slot'u serbest bırak.
                            semaphore.Release();
                        }
                    }, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // Servis durdurulduğunda bu exception fırlatılır, görmezden gel.
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while dispatching a background task.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }
} 