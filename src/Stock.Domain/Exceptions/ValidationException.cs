using System;
using System.Collections.Generic;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Doğrulama hataları için özel istisna sınıfı.
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        // FluentValidation gibi kütüphanelerle entegrasyon için constructor eklenebilir
        public ValidationException(IDictionary<string, string[]> errors) : this()
        {
            Errors = errors;
        }
    }
} 