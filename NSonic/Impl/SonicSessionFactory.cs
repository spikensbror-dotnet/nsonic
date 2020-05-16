using NSonic.Impl.Net;

namespace NSonic.Impl
{
    class SonicSessionFactory : ISonicSessionFactory
    {
        public ISonicSession Create(ITcpClient tcpClient, EnvironmentResponse environment)
        {
            return new SonicSession(tcpClient, environment);
        }
    }
}
