using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Stock.Application.Common.Interfaces;

namespace Stock.Application.Common.Behaviors
{
    /// <summary>
    /// Loglama davranışı
    /// </summary>
    /// <typeparam name="TRequest">İstek tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt tipi</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Yeni bir LoggingBehavior örneği oluşturur
        /// </summary>
        /// <param name="logger">Logger</param>
        public LoggingBehavior(ILoggerManager logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// İsteği işler
        /// </summary>
        /// <param name="request">İstek</param>
        /// <param name="next">Sonraki handler</param>
        /// <param name="cancellationToken">İptal token'ı</param>
        /// <returns>Yanıt</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogInfo($"İşleniyor: {requestName}");

            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            _logger.LogInfo($"İşlendi: {requestName} ({stopwatch.ElapsedMilliseconds}ms)");

            return response;
        }
    }
} 