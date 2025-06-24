using MediatR;
using Stock.Application.Features.Products.DTOs;
using System.Collections.Generic;
using Stock.Application.Constants;
using Stock.Application.Models; // PagedResponse sınıfı için eklendi

namespace Stock.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Tüm ürünleri getiren sorgu.
    /// Sayfalama, sıralama ve filtreleme için parametreler içerir.
    /// </summary>
    public class GetAllProductsQuery : IRequest<PagedResponse<ProductListItemDto>>
    {
        /// <summary>
        /// Sayfa numarası (1'den başlar)
        /// </summary>
        public int PageNumber { get; set; } = ApiConstants.DefaultPageNumber;

        /// <summary>
        /// Sayfa başına kayıt sayısı
        /// </summary>
        public int PageSize { get; set; } = ApiConstants.DefaultPageSize;

        /// <summary>
        /// Sıralama alanı
        /// </summary>
        public string SortBy { get; set; } = "Name";

        /// <summary>
        /// Sıralama yönü (asc/desc)
        /// </summary>
        public string SortDirection { get; set; } = "asc";

        /// <summary>
        /// Ürün adı filtresi (boş ise tüm ürünler)
        /// </summary>
        public string? NameFilter { get; set; }

        /// <summary>
        /// Kategori ID filtresi (null ise tüm kategoriler)
        /// </summary>
        public int? CategoryId { get; set; }
    }
} 