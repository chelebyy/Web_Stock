using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Command handler arayüzü - command'leri işleyen sınıflar için
    /// </summary>
    /// <typeparam name="TCommand">İşlenecek command tipi</typeparam>
    /// <typeparam name="TResponse">Command'in döndüreceği yanıt tipi</typeparam>
    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        /// <summary>
        /// Command'i işler
        /// </summary>
        /// <param name="command">İşlenecek command</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Yanıt döndürmeyen command'ler için handler arayüzü
    /// </summary>
    /// <typeparam name="TCommand">İşlenecek command tipi</typeparam>
    public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
    {
    }
} 