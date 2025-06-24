using System.Collections.Generic;

namespace Stock.Application.Models
{
    /// <summary>
    /// Sayfalanmış verileri standart bir formatta döndürmek için kullanılan yanıt sınıfı.
    /// </summary>
    /// <typeparam name="T">Döndürülecek veri tipini belirtir.</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Sayfalanmış verileri içeren koleksiyon.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Toplam kayıt sayısı.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Toplam sayfa sayısı.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Şu anki sayfa numarası (1'den başlar).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Sayfa başına kayıt sayısı.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Önceki sayfa var mı?
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// Sonraki sayfa var mı?
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;

        public PagedResponse(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)System.Math.Ceiling(totalCount / (double)pageSize);
        }
    }
} 