using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Mediator arayüzü - command ve query'leri ilgili handler'lara yönlendirir
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Command'i işler
        /// </summary>
        /// <typeparam name="TResponse">Command'in döndüreceği yanıt tipi</typeparam>
        /// <param name="command">İşlenecek command</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Query'i işler
        /// </summary>
        /// <typeparam name="TResponse">Query'nin döndüreceği yanıt tipi</typeparam>
        /// <param name="query">İşlenecek query</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }
} 