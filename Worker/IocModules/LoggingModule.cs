using System;
using System.Runtime.Remoting.Messaging;
using Autofac;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Worker.IocModules
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(InitLogger()).SingleInstance();
        }

        private static ILogger InitLogger()
        {
            var log = new LoggerConfiguration()
                .Enrich.WithProperty("Application", "Worker")
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.With(new CorrelationIdEnricher())
                .Enrich.WithProperty("OperatingSystem", Environment.OSVersion.VersionString)
                .WriteTo.MongoDB("mongodb://localhost:27017/logs")
                .WriteTo.File("logs.txt", LogEventLevel.Error)
                .CreateLogger();
            return log;
        }
    }

    public class CorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null)
            {
                throw new ArgumentException($"serilog requires a not null {nameof(logEvent)}");
            }
            var correlationId = CallContext.LogicalGetData("callId");
            var logEventProperty = new LogEventProperty("CorrelationId", new ScalarValue(correlationId));
            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}