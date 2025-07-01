using System;
using System.Linq.Expressions;
using Stock.Domain.Entities; // BaseEntity için
using System.Collections.Generic;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir ID'ye sahip entity'yi getirmek için kullanılan genel somut specification.
    /// </summary>
    /// <typeparam name="T">Entity türü (BaseEntity'den türemeli).</typeparam>
    public sealed class EntityByIdSpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        /// <summary>
        /// Belirtilen ID'ye göre filtreleyen yeni bir EntityByIdSpecification örneği oluşturur.
        /// </summary>
        /// <param name="id">Aranacak entity ID'si.</param>
        /// <param name="includes">İlişkili verileri dahil etmek için ifadeler.</param>
        public EntityByIdSpecification(int id, params Expression<Func<T, object>>[] includes)
        {
            AddCriteria(entity => entity.Id == id);

            foreach (var include in includes)
            {
                AddInclude(include);
            }
        }
    }
} 