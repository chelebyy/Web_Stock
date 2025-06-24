using FluentValidation;
using MediatR;
using Stock.Domain.Exceptions; // ValidationException için using
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next(); // Validator yoksa devam et
            }

            var context = new ValidationContext<TRequest>(request);

            // Tüm validator'ları çalıştır ve sonuçları topla
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Hataları filtrele ve birleştir
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                // Hataları uygun formata dönüştür
                var errorsDictionary = failures
                    .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                    .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

                // Eğer hata varsa ValidationException fırlat
                throw new Stock.Domain.Exceptions.ValidationException(errorsDictionary);
            }

            // Hata yoksa sonraki adıma geç
            return await next();
        }
    }
} 