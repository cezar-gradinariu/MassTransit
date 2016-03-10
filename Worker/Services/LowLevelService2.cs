using Autofac;
using System;
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

    public class LowLevelService1 : ILowLevelService1
    {
        private readonly IUnitOfWork _uow;

        public LowLevelService1(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Do()
        {
            _uow.Do();
        }
    }


    public class UnitOfWork : IUnitOfWork
    {
        private ILifetimeScope _lifetimeScope;


        public static int X;

        public UnitOfWork(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
            X++;
            x = X;
        }

        public int x { get; set; }

        public void Do()
        {
            Console.WriteLine(x);
        }
    }
}
