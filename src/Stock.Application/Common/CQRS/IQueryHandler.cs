using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Query handler arayüzü - query'leri işleyen sınıflar için
    /// </summary>
    /// <typeparam name="TQuery">İşlenecek query tipi</typeparam>
    /// <typeparam name="TResponse">Query'nin döndüreceği yanıt tipi</typeparam>
    public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Query'i işler
        /// </summary>
        /// <param name="query">İşlenecek query</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
    }
} 