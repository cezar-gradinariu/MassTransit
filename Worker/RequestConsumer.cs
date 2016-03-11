using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Worker.Interfaces;

namespace Worker
{
    public class RequestConsumer : RequestConsumerBase<CurrencyRequest, CurrencyResponse>
    {
        public RequestConsumer(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        protected override async Task ConsumeRequest(ConsumeContext<CurrencyRequest> context)
        {
            var uow1 = LifetimeScope.Resolve<ILowLevelService1>();
            var uow2 = LifetimeScope.Resolve<ILowLevelService2>();

            uow1.Do();
            uow2.Do();

            Console.WriteLine("ID:" + context.Headers.Get("ID", (Guid?) Guid.Empty));
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