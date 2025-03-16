using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stock.Application.Common.Models;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Rapor işlemleri için arayüz
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Yeni bir rapor oluşturma isteği ekler
        /// </summary>
        /// <param name="reportType">Rapor tipi</param>
        /// <param name="parameters">Rapor parametreleri</param>
        /// <param name="userId">İsteği yapan kullanıcı ID</param>
        /// <returns>Oluşturulan rapor isteğinin ID'si</returns>
        Task<string> QueueReportAsync(string reportType, Dictionary<string, string> parameters, string userId);
        
        /// <summary>
        /// Bekleyen rapor isteklerini işler
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı</param>
        Task ProcessPendingReportsAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Rapor durumunu getirir
        /// </summary>
        /// <param name="reportId">Rapor ID</param>
        /// <returns>Rapor durumu</returns>
        Task<ReportStatus> GetReportStatusAsync(string reportId);
        
        /// <summary>
        /// Kullanıcının raporlarını getirir
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <returns>Kullanıcının raporları</returns>
        Task<IEnumerable<ReportInfo>> GetUserReportsAsync(string userId);
        
        /// <summary>
        /// Rapor dosyasını indirir
        /// </summary>
        /// <param name="reportId">Rapor ID</param>
        /// <returns>Rapor dosyası</returns>
        Task<ReportFile> DownloadReportAsync(string reportId);
    }
} 