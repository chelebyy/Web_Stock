using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.Products
{
    /// <summary>
    /// Ürünleri kategori bilgisiyle birlikte getiren specification. Filtreleme, sıralama ve sayfalama özelliklerine sahiptir.
    /// </summary>
    public class ProductsWithCategorySpecification : BaseSpecification<Product>
    {
        /// <summary>
        /// Tüm filtreleme, sıralama ve sayfalama parametrelerini kullanarak specification oluşturur.
        /// </summary>
        /// <param name="nameFilter">Ürün adı filtresi (boş ise tüm ürünler)</param>
        /// <param name="categoryId">Kategori ID filtresi (null ise tüm kategoriler)</param>
        /// <param name="sortBy">Sıralama alanı</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <param name="pageNumber">Sayfa numarası (1'den başlar)</param>
        /// <param name="pageSize">Sayfa başına kayıt sayısı</param>
        public ProductsWithCategorySpecification(
            string? nameFilter = null, 
            int? categoryId = null, 
            string sortBy = "Name",
            string sortDirection = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            // Kriterleri birleştirmek için bir başlangıç noktası
            Expression<Func<Product, bool>>? criteria = null;

            // Adına göre filtreleme
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                var nameFilterLower = nameFilter.ToLower();
                criteria = p => p.Name.Value.ToLower().Contains(nameFilterLower);
            }
            
            // Kategoriye göre filtreleme
            if (categoryId.HasValue)
            {
                Expression<Func<Product, bool>> categoryCriteria = p => p.CategoryId == categoryId.Value;
                criteria = criteria == null ? categoryCriteria : criteria.And(categoryCriteria);
            }
            
            if (criteria != null)
            {
                SetCriteria(criteria);
            }

            // Kategori bilgisini dahil et
            AddInclude(p => p.Category);
            
            // Sıralama
            ApplySorting(sortBy, sortDirection);
            
            // Değişiklik izleme devre dışı (sadece görüntüleme için)
            AsNoTracking();
        }

        /// <summary>
        /// Tek bir ürünü ID'ye göre getiren specification oluşturur.
        /// </summary>
        /// <param name="id">Ürün ID'si</param>
        public ProductsWithCategorySpecification(int id) 
            : base(p => p.Id == id)
        {
            AddInclude(p => p.Category);
            // Değişiklik izleme devre dışı bırakılmadı (düzenleme amaçlı olabilir)
        }
        
        /// <summary>
        /// Filtreleme ve sıralama parametrelerini ekleyen specification oluşturur (sayfalama yok).
        /// GetTotalCount için kullanılır.
        /// </summary>
        /// <param name="nameFilter">Ürün adı filtresi (boş ise tüm ürünler)</param>
        /// <param name="categoryId">Kategori ID filtresi (null ise tüm kategoriler)</param>
        public ProductsWithCategorySpecification(string? nameFilter, int? categoryId) 
            : this(nameFilter, categoryId, "Name", "asc", 0, 0) // Sayfalama olmadan aynı filtreleri kullan
        {
            // Sayfalama parametrelerini sıfır vererek sayfalamayı devre dışı bırakıyoruz
        }
        
        /// <summary>
        /// Sıralama kriterlerini uygular.
        /// </summary>
        private void ApplySorting(string sortBy, string sortDirection)
        {
            // Sıralama alanının ve yönün geçerli olduğunu kontrol etmek burada önemli
            switch (sortBy.ToLower())
            {
                case "name":
                    if (sortDirection.ToLower() == "desc")
                        ApplyOrderByDescending(p => p.Name.Value);
                    else
                        ApplyOrderBy(p => p.Name.Value);
                    break;
                // Price alanı Product entity'sinde bulunmadığından kaldırıldı
                case "stocklevel":
                    if (sortDirection.ToLower() == "desc")
                        ApplyOrderByDescending(p => p.StockLevel.Value);
                    else
                        ApplyOrderBy(p => p.StockLevel.Value);
                    break;
                case "categoryname":
                    if (sortDirection.ToLower() == "desc")
                        ApplyOrderByDescending(p => p.Category.Name);
                    else
                        ApplyOrderBy(p => p.Category.Name);
                    break;
                default:
                    // Varsayılan olarak ürün adına göre sırala
                    ApplyOrderBy(p => p.Name.Value);
                    break;
            }
        }
    }

    // Expression'ları birleştirmek için bir yardımcı sınıf
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var leftBody = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var rightBody = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(leftBody, rightBody), parameter);
        }
    }

    internal class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression Visit(Expression node)
        {
            if (node == _oldValue)
                return _newValue;
            return base.Visit(node);
        }
    }
} 