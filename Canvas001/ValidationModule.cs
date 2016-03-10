using System.Reflection;
using Autofac;
using FluentValidation;
using Module = Autofac.Module;

namespace Canvas001
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            // builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();

            builder.RegisterType<WebApiApplication.AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();

            base.Load(builder);
        }
    }
}