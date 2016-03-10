using System;
using Autofac;
using MassTransit;
using Topshelf;
using MassTransit.Pipeline;
using System.Threading.Tasks;
using MassTransit.PipeConfigurators;
using MassTransit.Configurators;
using System.Linq;
using System.Collections.Generic;
using MassTransit.PipeBuilders;
using Contracts;
using FluentValidation;

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
                    //e.Consumer(typeof (RequestConsumer), container.Resolve);
                    e.Consumer(() => container.Resolve<RequestConsumer>() , m => 
                    {
                        using (var requestScope = container.BeginLifetimeScope())
                        {
                        }
                        //m.
                    });
                });

            });

            //_busControl.ConnectConsumeObserver(new ConsumeObserver());
            _busControl.Start();
        }

        public bool Stop(HostControl hostControl)
        {
            _busControl?.Stop();
            return true;
        }
    }


    //public class ConsumeObserver : IConsumeObserver
    //{
    //    public Task PreConsume<T>(ConsumeContext<T> context) where T : class
    //    {
    //        // called before the consumer's Consume method is called
    //        //return context.NotifyFaulted(TimeSpan.FromMilliseconds(1), "RequestConsumer", new Exception("tttt"));

    //        context.Respond(new CurrencyResponse
    //        {
    //            Currencies = new List<CurrencyInfo>
    //                {
    //                    new CurrencyInfo {IsoCode = "CAD1"},
    //                    new CurrencyInfo {IsoCode = "AUD1"},
    //                    new CurrencyInfo {IsoCode = "USD1"}
    //                }
    //        } );
    //        return Task.Factory.StartNew(() => { });
    //    }

    //    public Task PostConsume<T>(ConsumeContext<T> context) where T : class
    //    {
    //        // called after the consumer's Consume method is called
    //        // if an exception was thrown, the ConsumeFault method is called instead
    //        return Task.Factory.StartNew(() => { });
    //    }

    //    public Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
    //    {
    //        // called if the consumer's Consume method throws an exception
    //        return Task.Factory.StartNew(() => { });
    //    }
    //}

    //public static class ExampleMiddlewareConfiguratorExtensions
    //{
    //    public static void UseExceptionLogger<T>(this IPipeConfigurator<T> configurator)
    //        where T : class, ConsumeContext
    //    {
    //        configurator.AddPipeSpecification(new ExceptionLoggerSpecification<T>());
    //    }
    //}

    //public class ExceptionLoggerSpecification<T> : IPipeSpecification<T> where T : class, ConsumeContext
    //{
    //    public IEnumerable<ValidationResult> Validate()
    //    {
    //        return Enumerable.Empty<ValidationResult>();
    //    }

    //    public void Apply(IPipeBuilder<T> builder)
    //    {
    //        builder.AddFilter(new ExceptionLoggerFilter<T>());
    //    }
    //}

    //public class ExceptionLoggerFilter<T> : IFilter<T> where T : class, ConsumeContext
    //{
    //    long _exceptionCount;
    //    long _successCount;
    //    long _attemptCount;

    //    public void Probe(ProbeContext context)
    //    {
    //        var scope = context.CreateFilterScope("exceptionLogger");
    //        scope.Add("attempted", _attemptCount);
    //        scope.Add("succeeded", _successCount);
    //        scope.Add("faulted", _exceptionCount);
    //    }

    //    public async Task Send(T context, IPipe<T> next)
    //    {
    //        try
    //        {
    //            await next.Send(context);
    //        }
    //        catch (Exception ex)
    //        {
    //            await Console.Out.WriteLineAsync($"An exception occurred: {ex.Message}");

    //            // propagate the exception up the call stack
    //            throw;
    //        }
    //    }
    //}
}

