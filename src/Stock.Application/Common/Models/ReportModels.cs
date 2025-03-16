using System;
using System.Collections.Generic;

namespace Stock.Application.Common.Models
{
    /// <summary>
    /// Rapor durumu
    /// </summary>
    public enum ReportStatus
    {
        /// <summary>
        /// Bekliyor
        /// </summary>
        Pending = 0,
        
        /// <summary>
        /// İşleniyor
        /// </summary>
        Processing = 1,
        
        /// <summary>
        /// Tamamlandı
        /// </summary>
        Completed = 2,
        
        /// <summary>
        /// Hata oluştu
        /// </summary>
        Failed = 3
    }
    
    /// <summary>
    /// Rapor bilgisi
    /// </summary>
    public class ReportInfo
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
        /// Rapor parametreleri
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
        
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
        /// </summary>
        public ReportStatus Status { get; set; }
        
        /// <summary>
        /// Hata mesajı
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string FileName { get; set; }
    }
    
    /// <summary>
    /// Rapor dosyası
    /// </summary>
    public class ReportFile
    {
        /// <summary>
        /// Dosya adı
        /// </summary>
        public string FileName { get; set; }
        
        /// <summary>
        /// Dosya içeriği
        /// </summary>
        public byte[] Content { get; set; }
        
        /// <summary>
        /// Dosya tipi
        /// </summary>
        public string ContentType { get; set; }
    }
} 