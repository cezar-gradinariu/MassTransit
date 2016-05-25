using System;
using Autofac;
using Serilog;

namespace ApiHost.IocModules
{
    public class SerilogModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var log = new LoggerConfiguration()
                    .Enrich.WithProperty("Application", "ApiHost")
                    .Enrich.WithProperty("MachineName", Environment.MachineName)
                    .Enrich.WithProperty("OperatingSystem", Environment.OSVersion.VersionString)
                    .WriteTo.MongoDB("mongodb://localhost:27017/logs")
                    .CreateLogger();
                return log;
            })
                .SingleInstance()
                .As<ILogger>();
        }
    }
}