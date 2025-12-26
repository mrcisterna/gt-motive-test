using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Behaviors
{
    /// <summary>
    /// MediatR pipeline behavior for validating requests using FluentValidation.
    /// </summary>
    /// <typeparam name="TRequest">The request type.</typeparam>
    /// <typeparam name="TResponse">The response type.</typeparam>
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        /// <summary>
        /// Handles the request by validating it before passing to the handler.
        /// </summary>
        /// <param name="request">The request to validate.</param>
        /// <param name="next">The next handler in the pipeline.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The response from the handler if validation passes.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(next);

            if (!_validators.Any())
            {
                return await next().ConfigureAwait(false);
            }

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)))
                .ConfigureAwait(false);

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            return failures.Count > 0
                ? throw new ValidationException(
                    $"Validation failed for {typeof(TRequest).Name}",
                    failures)
                : await next().ConfigureAwait(false);
        }
    }
}
