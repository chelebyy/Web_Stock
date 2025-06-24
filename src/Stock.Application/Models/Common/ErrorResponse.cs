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
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public ErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        /// <summary>
        /// Hata detayları dictionary'si. Anahtar genellikle bir alan adı, değer ise o alanla ilgili hata mesajlarının listesidir.
        /// </summary>
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }

    public class ValidationErrorResponse : ErrorResponse
    {
        public new Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

        public ValidationErrorResponse(int statusCode, string message, Dictionary<string, string[]> errors) 
            : base(statusCode, message)
        {
            Errors = errors;
        }
    }
} 