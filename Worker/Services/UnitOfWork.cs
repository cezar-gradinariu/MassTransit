using System;
using Autofac;
using Worker.Interfaces;

namespace Worker.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        public static int X;
        private ILifetimeScope _lifetimeScope;

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