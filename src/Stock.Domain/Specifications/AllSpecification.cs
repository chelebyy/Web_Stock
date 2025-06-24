using System;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir türdeki tüm entity'leri getirmek için kullanılan somut specification.
    /// </summary>
    /// <typeparam name="T">Entity türü.</typeparam>
    public sealed class AllSpecification<T> : BaseSpecification<T>
    {
        /// <summary>
        /// Kriter olmadan yeni bir AllSpecification örneği oluşturur.
        /// </summary>
        public AllSpecification() : base() // Kriter yok
        {
        }
    }
} 