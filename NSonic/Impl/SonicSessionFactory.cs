using NSonic.Impl.Net;
using System.Threading;

namespace NSonic.Impl
{
    class SonicSessionFactory : ISonicSessionFactory
    {
        private readonly ITcpClient client;
        private readonly SemaphoreSlim semaphore;

        public SonicSessionFactory(ITcpClient client)
        {
            this.client = client;
            this.semaphore = new SemaphoreSlim(1, 1);
        }

        public ISonicSession Create(EnvironmentResponse environment)
        {
            return new SonicSession(this.client, this.semaphore, environment);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
