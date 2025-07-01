using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Rapor oluşturma işlemleri için servis arayüzü.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Ürün listesi raporunu asenkron olarak oluşturur.
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Rapor oluşturma işleminin tamamlandığını belirten bir Task.</returns>
        Task GenerateProductsReportAsync(CancellationToken cancellationToken);
    }
} 