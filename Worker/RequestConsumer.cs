using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
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
            var callId = (Guid) CallContext.LogicalGetData("callId");
            for (int i = 0; i < 20000; i++)
            {

            Logger.Information("A call with id {callId} is done on worker.", callId);
            Logger.Error("Emulated error call with id {callId} is done on worker.", callId);
            }

            var uow1 = LifetimeScope.Resolve<ILowLevelService1>();
            var uow2 = LifetimeScope.Resolve<ILowLevelService2>();
            uow1.Do();
            uow2.Do();

            Console.WriteLine("ID:" + context.Headers.Get("callId", (Guid?) Guid.Empty));
            context.Respond(new CurrencyResponse
            {
                Currencies = new List<CurrencyInfo>
                {
                    new CurrencyInfo {IsoCode = "CAD"},
                    new CurrencyInfo {IsoCode = "AUD"},
                    new CurrencyInfo {IsoCode = "USD"}
                }
            });
            Logger.Information("A call with id {callId} was successfully completed on the worker.", callId);
        }
    }
}