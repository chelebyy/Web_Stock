using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Stock.Application.Common.Exceptions
{
    /// <summary>
    /// Validasyon hatası istisnası
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Validasyon hataları
        /// </summary>
        public IDictionary<string, string[]> Errors { get; }

        /// <summary>
        /// Yeni bir ValidationException örneği oluşturur
        /// </summary>
        public ValidationException()
            : base("Bir veya daha fazla validasyon hatası oluştu.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Validasyon hatalarıyla yeni bir ValidationException örneği oluşturur
        /// </summary>
        /// <param name="failures">Validasyon hataları</param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.ToArray());

            Errors = failureGroups;
        }
    }
} 