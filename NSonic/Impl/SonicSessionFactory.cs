using NSonic.Impl.Net;

namespace NSonic.Impl
{
    class SonicSessionFactory : ISonicSessionFactory
    {
        private readonly ITcpClient client;

        public SonicSessionFactory(ITcpClient client)
        {
            this.client = client;
        }

        public ISonicSession Create()
        {
            return new SonicSession(this.client);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }
    }
}
