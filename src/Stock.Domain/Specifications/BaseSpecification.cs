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
        /// String tabanlı alan adı ve sıralama yönüne göre sıralama uygular.
        /// </summary>
        /// <param name="sortField">Sıralanacak alanın adı.</param>
        /// <param name="sortOrder">Sıralama yönü ("asc" veya "desc").</param>
        public virtual void AddOrderBy(string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField)) return;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortField);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            if (string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
            {
                ApplyOrderByDescending(lambda);
            }
            else
            {
                ApplyOrderBy(lambda);
            }
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

        /// <summary>
        /// Mevcut kriterlere yeni bir kriter ekler (AND ile birleştirir).
        /// </summary>
        /// <param name="criteria">Eklenecek kriter ifadesi</param>
        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            if (Criteria == null)
            {
                Criteria = criteria;
            }
            else
            {
                var param = Expression.Parameter(typeof(T), "x");
                var left = Expression.Invoke(Criteria, param);
                var right = Expression.Invoke(criteria, param);
                var combined = Expression.AndAlso(left, right);
                Criteria = Expression.Lambda<Func<T, bool>>(combined, param);
            }
        }

        /// <summary>
        /// Artan sıralama kriterini ayarlar (yeni API).
        /// </summary>
        /// <param name="orderByExpression">Sıralama ifadesi.</param>
        protected virtual void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            OrderByDescending = null;
        }

        /// <summary>
        /// Azalan sıralama kriterini ayarlar (yeni API).
        /// </summary>
        /// <param name="orderByDescendingExpression">Sıralama ifadesi.</param>
        protected virtual void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            OrderBy = null;
        }

        /// <summary>
        /// Sayfalama parametrelerini ayarlar (sayfa number bazlı).
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası (1'den başlar)</param>
        /// <param name="pageSize">Sayfa boyutu</param>
        public virtual void ApplyPaging(int pageNumber, int pageSize, bool isPageBased = true)
        {
            if (isPageBased)
            {
                Skip = (pageNumber - 1) * pageSize;
                Take = pageSize;
            }
            else
            {
                Skip = pageNumber;
                Take = pageSize;
            }
            IsPagingEnabled = true;
        }
    }
} 