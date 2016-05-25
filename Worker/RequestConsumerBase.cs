using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Autofac;
using Contracts.Responses;
using FluentValidation;
using MassTransit;
using Serilog;

namespace Worker
{
    public abstract class RequestConsumerBase<TRequest, TResponse> : IConsumer<TRequest>
        where TRequest : class
        where TResponse : ResponseBase, new()
    {
        protected readonly ILifetimeScope LifetimeScope;
        protected readonly ILogger Logger;

        protected RequestConsumerBase(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
            Logger = lifetimeScope.Resolve<ILogger>();
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
            CallContext.LogicalSetData("callId", context.Headers.Get("callId", (Guid?) Guid.Empty));
        }
    }
}