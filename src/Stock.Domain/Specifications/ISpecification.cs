using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Bir sorguyu veya sorgu kriterlerini tanımlayan specification deseni için temel arayüz.
    /// </summary>
    /// <typeparam name="T">Specification'ın uygulandığı entity türü.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Sorgu için filtreleme kriteri (WHERE koşulu).
        /// </summary>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Sorgu sonucuna dahil edilecek ilişkili entity'leri belirten ifadelerin listesi (Include ifadeleri).
        /// </summary>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Dahil edilecek ilişkili entity'leri string olarak belirten liste (ThenInclude gibi zincirleme durumlar için).
        /// </summary>
        List<string> IncludeStrings { get; }

        /// <summary>
        /// Artan sıralama kriteri.
        /// </summary>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// Azalan sıralama kriteri.
        /// </summary>
        Expression<Func<T, object>>? OrderByDescending { get; }

        /// <summary>
        /// İkincil artan sıralama kriteri (ThenBy).
        /// </summary>
        Expression<Func<T, object>>? ThenBy { get; }

        /// <summary>
        /// İkincil azalan sıralama kriteri (ThenByDescending).
        /// </summary>
        Expression<Func<T, object>>? ThenByDescending { get; }

        /// <summary>
        /// Sayfalama için alınacak maksimum kayıt sayısı.
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Sayfalama için atlanacak kayıt sayısı.
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Sayfalama etkin mi?
        /// </summary>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Sorgu sonucunda EF Core değişiklik izlemesini devre dışı bırakmak için kullanılır (salt okunur sorgular için).
        /// </summary>
        bool IsAsNoTracking { get; }

        /// <summary>
        /// Varsa, global sorgu filtrelerinin (örn. IsDeleted) bu specification için yok sayılıp sayılmayacağını belirtir.
        /// </summary>
        bool IgnoreQueryFilters { get; }
    }
} 