using System;
using System.Runtime.Remoting.Messaging;
using Autofac;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;

namespace Canvas001
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri("rabbitmq://localhost/"),
                        h =>
                        {
                            h.Username("admin");
                            h.Password("admin");
                            h.Heartbeat(30);
                        });
                    cfg.ConfigureSend(p => { p.UseSendExecute(sendContext => { sendContext.Headers.Set("ID", (string) CallContext.LogicalGetData("ID")); }); });
                });
                return busControl;
            })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            builder.Register(context =>
            {
                var bus = context.Resolve<IBusControl>();
                var serviceAddress = new Uri("rabbitmq://localhost/request_service");
                var client = bus.CreateRequestClient<CurrencyRequest, CurrencyResponse>(serviceAddress, TimeSpan.FromSeconds(100000));
                return client;
            })
                .As<IRequestClient<CurrencyRequest, CurrencyResponse>>();
        }
    }
}