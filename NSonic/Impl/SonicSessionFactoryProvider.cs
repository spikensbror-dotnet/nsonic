using NSonic.Impl.Net;

namespace NSonic.Impl
{
    class SonicSessionFactoryProvider : ISonicSessionFactoryProvider
    {
        public ISonicSessionFactory Create(string hostname, int port)
        {
            return new SonicSessionFactory(new TcpClientAdapter(hostname, port));
        }
    }
}
