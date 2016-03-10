using Autofac;
using FluentValidation;
using MassTransit;
using System;
using Topshelf;
using Worker.Interfaces;
using Worker.Services;
using Worker.Validators;

namespace Worker
{
    public class RequestService : ServiceControl
    {
        private IBusControl _busControl;

        public bool Start(HostControl hostControl)
        {

            var builder = new ContainerBuilder();
            builder.RegisterType<RequestConsumer>().As<RequestConsumer>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<LowLevelService1>().As<ILowLevelService1>().InstancePerLifetimeScope();
            builder.RegisterType<LowLevelService2>().As<ILowLevelService2>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(CurrencyRequestValidator).Assembly)
                   .AsClosedTypesOf(typeof(AbstractValidator<>))
                   .AsImplementedInterfaces();


            var container = builder.Build();

            StartBus(container);

            return true;
        }

        private void StartBus(IContainer container)
        {
            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("admin");
                    h.Password("admin");
                });

                x.ReceiveEndpoint(host, "request_service", e =>
                {
                    e.Consumer(() => container.Resolve<RequestConsumer>());
                });

            });

            _busControl.Start();
        }

        public bool Stop(HostControl hostControl)
        {
            _busControl?.Stop();
            return true;
        }
    }
}

