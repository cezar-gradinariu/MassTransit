using System.Reflection;
using Autofac;
using Module = Autofac.Module;

namespace ApiHost.IocModules
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerDependency();

            base.Load(builder);
        }
    }
}