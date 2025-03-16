using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = Stock.Application.Common.Exceptions.ValidationException;

namespace Stock.Application.Common.Behaviors
{
    /// <summary>
    /// Validasyon davranışı
    /// </summary>
    /// <typeparam name="TRequest">İstek tipi</typeparam>
    /// <typeparam name="TResponse">Yanıt tipi</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Yeni bir ValidationBehavior örneği oluşturur
        /// </summary>
        /// <param name="validators">Validatörler</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
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
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
} 