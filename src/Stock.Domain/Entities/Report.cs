using System;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Rapor entity sınıfı
    /// </summary>
    public class Report
    {
        /// <summary>
        /// Rapor ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Rapor tipi
        /// </summary>
        public string ReportType { get; set; }
        
        /// <summary>
        /// Rapor parametreleri (JSON formatında)
        /// </summary>
        public string Parameters { get; set; }
        
        /// <summary>
        /// İsteği yapan kullanıcı ID
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Tamamlanma tarihi
        /// </summary>
        public DateTime? CompletedAt { get; set; }
        
        /// <summary>
        /// Rapor durumu
        /// 0: Bekliyor, 1: İşleniyor, 2: Tamamlandı, 3: Hata
        /// </summary>
        public int Status { get; set; }
        
        /// <summary>
        /// Hata mesajı
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string FileName { get; set; }
    }
} 