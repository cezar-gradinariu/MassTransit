using Worker.Interfaces;

namespace Worker.Services
{
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
}