using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Contracts;
using MassTransit;
using System.Runtime.Remoting.Messaging;
using FluentValidation;

namespace Worker
{

    public abstract class RequestConsumerBase<TRequest, TResponse> : IConsumer<TRequest> 
        where TRequest : class
        where TResponse : class, new()
    {
        private readonly ILifetimeScope _lifetimeScope;

        public RequestConsumerBase(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            SetCallId(context);

            var validator = _lifetimeScope.Resolve<AbstractValidator<TRequest>>();
            var validation = await validator.ValidateAsync(context.Message);
            if (!validation.IsValid)
            {
                context.Respond(new CurrencyResponse());
            }
            else
            {
                using (var messageScope = _lifetimeScope.BeginLifetimeScope())
                {
                    await Consume1(context, messageScope);
                }
            }
        }

        protected virtual async Task Consume1(ConsumeContext<TRequest> context, ILifetimeScope messageScope)
        {
            await Task.Factory.StartNew(() => { });
        }

        private static void SetCallId(ConsumeContext<TRequest> context)
        {
            CallContext.LogicalSetData("call id", context.Headers.Get("ID", (Guid?)Guid.Empty));
        }

    }

    public class RequestConsumer : RequestConsumerBase<CurrencyRequest, CurrencyResponse>
    {
        public RequestConsumer(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {

        }

        protected override async Task Consume1(ConsumeContext<CurrencyRequest> context, ILifetimeScope messageScope)
        {
            var uow1 = messageScope.Resolve<ILowLevelService1>();
            var uow2 = messageScope.Resolve<ILowLevelService2>();

            uow1.Do();
            uow2.Do();

            Console.WriteLine("ID:" + context.Headers.Get("ID", (Guid?)Guid.Empty));
            context.Respond(new CurrencyResponse
            {
                Currencies = new List<CurrencyInfo>
                    {
                        new CurrencyInfo {IsoCode = "CAD"},
                        new CurrencyInfo {IsoCode = "AUD"},
                        new CurrencyInfo {IsoCode = "USD"}
                    }
            });
        }
    }

    public class CurrencyRequestValidator : AbstractValidator<CurrencyRequest>
    {
        public CurrencyRequestValidator()
        {
            RuleFor(p => p)
                .Must(p =>
                {
                    return false;
                });
        }
    }

    #region
    public interface ILowLevelService2
    {
        void Do();
    }

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
    
    public interface ILowLevelService1
    {
        void Do();
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

    public interface IUnitOfWork
    {
        void Do();
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
    #endregion
}