using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using ApiHost.Filters;
using ApiHost.IocModules;
using ApiHost.MediaTypeFormatters;
using ApiHost.Validators;
using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using MassTransit;

namespace ApiHost
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);
            builder.Register(c => new ValidateActionFilter(c.Resolve<ILifetimeScope>()))
                .AsWebApiActionFilterFor<ApiController>()
                .InstancePerRequest();

            //Register mass transit
            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ValidationModule>();

            config.Formatters.RemoveAt(0);
            config.Formatters.Insert(0, new JilFormatter());
            //config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            builder.RegisterAssemblyTypes(typeof (ComplexRequestValidator).Assembly)
                .AsClosedTypesOf(typeof (AbstractValidator<>))
                .AsImplementedInterfaces();


            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var busControl = container.Resolve<IBusControl>();
            busControl.Start();
        }

        protected void Application_BeginRequest()
        {
            var x = Guid.NewGuid().ToString();
            CallContext.LogicalSetData("ID", x);
        }
    }
}