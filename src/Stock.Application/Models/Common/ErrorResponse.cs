using System.Collections.Generic;

namespace Stock.Application.Models.Common
{
    /// <summary>
    /// API hatalarını standart bir formatta temsil eden sınıf.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Hatanın genel açıklaması.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Hata detayları dictionary'si. Anahtar genellikle bir alan adı, değer ise o alanla ilgili hata mesajlarının listesidir.
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; }
    }
} 