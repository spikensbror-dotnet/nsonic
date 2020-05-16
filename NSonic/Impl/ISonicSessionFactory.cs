using NSonic.Impl.Net;

namespace NSonic.Impl
{
    interface ISonicSessionFactory
    {
        ISonicSession Create(ITcpClient tcpClient, EnvironmentResponse environment);
    }
}
