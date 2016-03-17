using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Autofac;
using Contracts.Responses;
using FluentValidation;
using MassTransit;
using FluentValidation.Results;

namespace Worker
{
    public abstract class RequestConsumerBase<TRequest, TResponse> : IConsumer<TRequest>
        where TRequest : class
        where TResponse : ResponseBase, new()
    {
        protected readonly ILifetimeScope LifetimeScope;

        protected RequestConsumerBase(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            SetCallId(context);

            var validator = LifetimeScope.Resolve<AbstractValidator<TRequest>>();
            var validation = await validator.ValidateAsync(context.Message);
            if (!validation.IsValid)
            {
                var response = new TResponse();
                response.Validation = validation.AsError();
                context.Respond(response);
            }
            else
            {
                await ConsumeRequest(context);
            }
        }


        protected abstract Task ConsumeRequest(ConsumeContext<TRequest> context);

        private static void SetCallId(ConsumeContext<TRequest> context)
        {
            CallContext.LogicalSetData("call id", context.Headers.Get("ID", (Guid?) Guid.Empty));
        }
    }

    public static class FluentValidationExtensions
    {
        public static Error AsError(this ValidationResult validation)
        {
            return new Error
            {
                ErrorCode = validation.Errors.First().ErrorCode,
                ErrorMessage = validation.Errors.First().ErrorMessage,
                Errors = validation.Errors.Select(v => new ErrorInfo
                {
                    ErrorCode = v.ErrorCode,
                    ErrorMessage = v.ErrorMessage,
                    PropertyName = v.PropertyName
                }).ToList()
            };
        }
    }
}