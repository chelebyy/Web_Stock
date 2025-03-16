using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;

namespace Stock.Infrastructure.BackgroundServices
{
    /// <summary>
    /// Rapor oluşturma işlemlerini arka planda çalıştıran servis
    /// </summary>
    public class ReportGenerationService : BackgroundService
    {
        private readonly ILogger<ReportGenerationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ReportGenerationService(
            ILogger<ReportGenerationService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Rapor oluşturma servisi başlatıldı.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogDebug("Bekleyen raporlar kontrol ediliyor...");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                        await reportService.ProcessPendingReportsAsync(stoppingToken).ConfigureAwait(false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Rapor işleme sırasında bir hata oluştu.");
                }

                // 5 dakika bekle
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken).ConfigureAwait(false);
            }

            _logger.LogInformation("Rapor oluşturma servisi durduruldu.");
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rapor oluşturma servisi durduruluyor...");
            await base.StopAsync(cancellationToken);
        }
    }
} 