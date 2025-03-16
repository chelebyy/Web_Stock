using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.CQRS;

namespace Stock.Infrastructure.CQRS
{
    /// <summary>
    /// Mediator implementasyonu - command ve query'leri ilgili handler'lara yönlendirir
    /// </summary>
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Yeni bir Mediator örneği oluşturur
        /// </summary>
        /// <param name="serviceProvider">Servis sağlayıcı</param>
        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Command'i işler
        /// </summary>
        /// <typeparam name="TResponse">Command'in döndüreceği yanıt tipi</typeparam>
        /// <param name="command">İşlenecek command</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"{command.GetType().Name} için handler bulunamadı.");
            }

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method.Invoke(handler, new object[] { command, cancellationToken });

            return result;
        }

        /// <summary>
        /// Query'i işler
        /// </summary>
        /// <typeparam name="TResponse">Query'nin döndüreceği yanıt tipi</typeparam>
        /// <param name="query">İşlenecek query</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>İşlem sonucu</returns>
        public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new InvalidOperationException($"{query.GetType().Name} için handler bulunamadı.");
            }

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method.Invoke(handler, new object[] { query, cancellationToken });

            return result;
        }
    }
} 