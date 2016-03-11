using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using MassTransit;

namespace Worker
{
    public abstract class RequestConsumerBase<TRequest, TResponse> : IConsumer<TRequest>
        where TRequest : class
        where TResponse : class, new()
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
                context.Respond(new TResponse());
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
}