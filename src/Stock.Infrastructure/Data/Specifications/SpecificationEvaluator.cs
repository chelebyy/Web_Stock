using System.Linq;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities; // BaseEntity'nin namespace'i
using Stock.Domain.Specifications;

namespace Stock.Infrastructure.Data.Specifications
{
    /// <summary>
    /// Bir ISpecification nesnesini IQueryable üzerine uygulayarak sorguyu oluşturan yardımcı sınıf.
    /// </summary>
    /// <typeparam name="T">Specification'ın uygulandığı entity türü (BaseEntity'den türemelidir).</typeparam>
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        /// <summary>
        /// Verilen IQueryable nesnesine ISpecification kriterlerini uygular.
        /// </summary>
        /// <param name="inputQuery">Üzerine specification uygulanacak IQueryable nesnesi.</param>
        /// <param name="spec">Uygulanacak ISpecification nesnesi.</param>
        /// <param name="evaluateCriteriaOnly">True ise, sadece filtre kriterleri ve sıralama uygulanır, Include ve sayfalama uygulanmaz.</param>
        /// <returns>Specification kriterleri uygulanmış IQueryable nesnesi.</returns>
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec, bool evaluateCriteriaOnly = false)
        {
            var query = inputQuery;

            // Filtreleme kriterini uygula (WHERE)
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // evaluateCriteriaOnly true ise, sadece filtre ve sıralama uygulanır (sayım sorguları için)
            if (!evaluateCriteriaOnly)
            {
                // İlişkili entity'leri dahil et (Include)
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

                // String tabanlı ilişkili entity'leri dahil et (ThenInclude vb. için)
                query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            }

            // Sıralamayı uygula
            bool isOrdered = false;
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
                isOrdered = true;
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
                isOrdered = true;
            }

            // İkincil sıralamayı uygula (Sadece ana sıralama varsa)
            if (isOrdered)
            {
                if (spec.ThenBy != null)
                {
                    // OrderBy sonrası ThenBy için query'yi IOrderedQueryable'a cast etmemiz gerekir.
                    // Ancak OrderBy/OrderByDescending zaten IOrderedQueryable döndürür.
                    query = ((IOrderedQueryable<T>)query).ThenBy(spec.ThenBy);
                }
                else if (spec.ThenByDescending != null)
                {
                    query = ((IOrderedQueryable<T>)query).ThenByDescending(spec.ThenByDescending);
                }
            }

            // Sayfalamayı uygula (SKIP, TAKE) - evaluateCriteriaOnly true ise uygulanmaz
            if (!evaluateCriteriaOnly && spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // Değişiklik izlemeyi devre dışı bırak (AsNoTracking)
            if (spec.IsAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            
            // IgnoreQueryFilters henüz implemente edilmedi.
            // Global filtreler varsa burada query.IgnoreQueryFilters() çağrılabilir.
            if (spec.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return query;
        }
    }
} 