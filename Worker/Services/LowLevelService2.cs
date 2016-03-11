using Autofac;
using Worker.Interfaces;

namespace Worker.Services
{
    public class LowLevelService2 : ILowLevelService2
    {
        private readonly ILifetimeScope _lifetimeScope;

        public LowLevelService2(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void Do()
        {
            var uow = _lifetimeScope.Resolve<IUnitOfWork>();
            uow.Do();
        }
    }
}