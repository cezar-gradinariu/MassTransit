using Topshelf;

namespace Worker
{
    internal class Program
    {
        private static void Main()
        {
            HostFactory.Run(x => x.Service<RequestService>());
        }
    }
}