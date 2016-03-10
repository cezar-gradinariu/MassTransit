using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using FluentValidation;
using FluentValidation.WebApi;
using MassTransit;

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

            //Register mass transit
            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ValidationModule>();

            //Fluent validation - register own lbrary
            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration,
                p =>
                {
                    p.ValidatorFactory = new AutofacValidatorFactory();
                });

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var busControl = container.Resolve<IBusControl>();
            busControl.Start();

            Container.Instance = container;
        }

        protected void Application_BeginRequest()
        {
            var x = Guid.NewGuid().ToString();
            CallContext.LogicalSetData("ID", x);
        }
        
        public class AutofacValidatorFactory : ValidatorFactoryBase
        {
            public override IValidator CreateInstance(Type validatorType)
            {
                object instance;
                if (!Container.Instance.TryResolve(validatorType, out instance))
                {
                    return null;
                }
                return instance as IValidator;
            }
        }
    }
}