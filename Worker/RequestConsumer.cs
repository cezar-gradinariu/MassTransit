using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Contracts;
using MassTransit;
using System.Runtime.Remoting.Messaging;
using FluentValidation;
using Worker.Interfaces;

namespace Worker
{

    public abstract class RequestConsumerBase<TRequest, TResponse> : IConsumer<TRequest> 
        where TRequest : class
        where TResponse : class, new()
    {
        protected readonly ILifetimeScope _lifetimeScope;

        public RequestConsumerBase(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            SetCallId(context);

            var validator = _lifetimeScope.Resolve<AbstractValidator<TRequest>>();
            var validation = await validator.ValidateAsync(context.Message);
            if (!validation.IsValid)
            {
                context.Respond(new CurrencyResponse());
            }
            else
            {
                await Consume1(context);
            }
        }

        protected virtual async Task Consume1(ConsumeContext<TRequest> context)
        {
            await Task.Factory.StartNew(() => { });
        }

        private static void SetCallId(ConsumeContext<TRequest> context)
        {
            CallContext.LogicalSetData("call id", context.Headers.Get("ID", (Guid?)Guid.Empty));
        }

    }

    public class RequestConsumer : RequestConsumerBase<CurrencyRequest, CurrencyResponse>
    {
        public RequestConsumer(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {

        }

        protected override async Task Consume1(ConsumeContext<CurrencyRequest> context)
        {
            var uow1 = _lifetimeScope.Resolve<ILowLevelService1>();
            var uow2 = _lifetimeScope.Resolve<ILowLevelService2>();

            uow1.Do();
            uow2.Do();

            Console.WriteLine("ID:" + context.Headers.Get("ID", (Guid?)Guid.Empty));
            context.Respond(new CurrencyResponse
            {
                Currencies = new List<CurrencyInfo>
                    {
                        new CurrencyInfo {IsoCode = "CAD"},
                        new CurrencyInfo {IsoCode = "AUD"},
                        new CurrencyInfo {IsoCode = "USD"}
                    }
            });
        }
    }

}