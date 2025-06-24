using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// ISpecification<T> arayüzünü implemente eden soyut temel sınıf.
    /// Tekrarlanan kodları azaltmaya yardımcı olur.
    /// </summary>
    /// <typeparam name="T">Specification'ın uygulandığı entity türü.</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// BaseSpecification sınıfının yapıcı metodu.
        /// </summary>
        protected BaseSpecification() { }

        /// <summary>
        /// Belirli bir kriterle BaseSpecification sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="criteria">Sorgu için filtreleme kriteri.</param>
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        /// <inheritdoc/>
        public Expression<Func<T, bool>>? Criteria { get; private set; }

        /// <inheritdoc/>
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        /// <inheritdoc/>
        public List<string> IncludeStrings { get; } = new List<string>();

        /// <inheritdoc/>
        public Expression<Func<T, object>>? OrderBy { get; private set; }

        /// <inheritdoc/>
        public Expression<Func<T, object>>? OrderByDescending { get; private set; }

        /// <summary>
        /// İkincil artan sıralama kriteri (ThenBy).
        /// Sadece OrderBy veya OrderByDescending ayarlandıysa geçerlidir.
        /// </summary>
        public Expression<Func<T, object>>? ThenBy { get; private set; }

        /// <summary>
        /// İkincil azalan sıralama kriteri (ThenByDescending).
        /// Sadece OrderBy veya OrderByDescending ayarlandıysa geçerlidir.
        /// </summary>
        public Expression<Func<T, object>>? ThenByDescending { get; private set; }

        /// <inheritdoc/>
        public int Take { get; private set; }

        /// <inheritdoc/>
        public int Skip { get; private set; }

        /// <inheritdoc/>
        public bool IsPagingEnabled { get; private set; }

        /// <inheritdoc/>
        public bool IsAsNoTracking { get; private set; }

        /// <inheritdoc/>
        public bool IgnoreQueryFilters { get; private set; }

        /// <summary>
        /// Sorguya bir Include ifadesi ekler.
        /// </summary>
        /// <param name="includeExpression">Eklenecek Include ifadesi.</param>
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Sorguya string tabanlı bir Include ifadesi ekler (örn. "Posts.Comments").
        /// ThenInclude gibi zincirleme durumlar için kullanışlıdır.
        /// </summary>
        /// <param name="includeString">Eklenecek Include ifadesi.</param>
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        /// <summary>
        /// Artan sıralama kriterini ayarlar.
        /// </summary>
        /// <param name="orderByExpression">Sıralama ifadesi.</param>
        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            OrderByDescending = null; // Ensure only one order is set
        }

        /// <summary>
        /// Azalan sıralama kriterini ayarlar.
        /// </summary>
        /// <param name="orderByDescendingExpression">Sıralama ifadesi.</param>
        protected virtual void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            OrderBy = null; // Ensure only one order is set
        }

        /// <summary>
        /// İkincil artan sıralama kriterini ayarlar (ThenBy).
        /// Bu metodun çağrılabilmesi için önce ApplyOrderBy veya ApplyOrderByDescending çağrılmış olmalıdır.
        /// </summary>
        /// <param name="thenByExpression">İkincil sıralama ifadesi.</param>
        /// <exception cref="InvalidOperationException">Eğer ana sıralama (OrderBy/OrderByDescending) ayarlanmamışsa fırlatılır.</exception>
        protected virtual void ApplyThenBy(Expression<Func<T, object>> thenByExpression)
        {
            if (OrderBy == null && OrderByDescending == null)
            {
                throw new InvalidOperationException("Ana sıralama (OrderBy/OrderByDescending) ayarlanmadan ikincil sıralama (ThenBy) ayarlanamaz.");
            }
            ThenBy = thenByExpression;
            ThenByDescending = null; // Ensure only one secondary order is set
        }

        /// <summary>
        /// İkincil azalan sıralama kriterini ayarlar (ThenByDescending).
        /// Bu metodun çağrılabilmesi için önce ApplyOrderBy veya ApplyOrderByDescending çağrılmış olmalıdır.
        /// </summary>
        /// <param name="thenByDescendingExpression">İkincil sıralama ifadesi.</param>
        /// <exception cref="InvalidOperationException">Eğer ana sıralama (OrderBy/OrderByDescending) ayarlanmamışsa fırlatılır.</exception>
        protected virtual void ApplyThenByDescending(Expression<Func<T, object>> thenByDescendingExpression)
        {
            if (OrderBy == null && OrderByDescending == null)
            {
                throw new InvalidOperationException("Ana sıralama (OrderBy/OrderByDescending) ayarlanmadan ikincil sıralama (ThenByDescending) ayarlanamaz.");
            }
            ThenByDescending = thenByDescendingExpression;
            ThenBy = null; // Ensure only one secondary order is set
        }

        /// <summary>
        /// Sayfalama parametrelerini ayarlar.
        /// </summary>
        /// <param name="skip">Atlanacak kayıt sayısı.</param>
        /// <param name="take">Alınacak maksimum kayıt sayısı.</param>
        public virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        /// <summary>
        /// Bu specification için EF Core değişiklik izlemesini devre dışı bırakır.
        /// Zincirleme (fluent) kullanım için kendini döndürür.
        /// </summary>
        protected virtual BaseSpecification<T> AsNoTracking()
        {
            IsAsNoTracking = true;
            return this;
        }

        /// <summary>
        /// Bu specification için global sorgu filtrelerini (varsa) yok sayar.
        /// Zincirleme (fluent) kullanım için kendini döndürür.
        /// </summary>
        protected virtual BaseSpecification<T> WithIgnoredQueryFilters()
        {
            IgnoreQueryFilters = true;
            return this;
        }

        /// <summary>
        /// Alt sınıflar tarafından Criteria özelliğini ayarlamak için kullanılır.
        /// </summary>
        /// <param name="criteria">Yeni kriter ifadesi</param>
        protected void SetCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
} 