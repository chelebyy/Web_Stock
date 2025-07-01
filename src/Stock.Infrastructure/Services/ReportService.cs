using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// Raporlama işlemlerini yürüten servis.
    /// </summary>
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(ILogger<ReportService> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Ürün listesi raporunu oluşturur.
        /// </summary>
        public async Task GenerateProductsReportAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ürün raporu oluşturma işlemi başladı...");

            // Veritabanından tüm ürünleri çek.
            var allProducts = await _unitOfWork.Products.GetAllAsync(cancellationToken).ConfigureAwait(false);
            var productCount = allProducts.Count();

            _logger.LogInformation("{Count} adet ürün bulundu. Rapor içeriği hazırlanıyor...", productCount);

            // Uzun süren bir işlemi simüle et.
            // Gerçek dünyada burada PDF/Excel oluşturma gibi karmaşık işlemler yer alabilir.
            await Task.Delay(10000, cancellationToken).ConfigureAwait(false); // 10 saniye bekle

            _logger.LogInformation("Ürün raporu başarıyla oluşturuldu ve 'reports/products.xlsx' adresine kaydedildi. (Simülasyon)");
        }
    }
} 