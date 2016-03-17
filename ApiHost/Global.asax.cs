using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Canvas001.Attributes;
using Canvas001.Validators;
using FluentValidation;
using FluentValidation.WebApi;
using MassTransit;
using Newtonsoft.Json;

namespace Canvas001
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

            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            builder.RegisterAssemblyTypes(typeof(ComplexRequestValidator).Assembly)
                .AsClosedTypesOf(typeof(AbstractValidator<>))
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