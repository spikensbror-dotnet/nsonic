using NSonic.Impl.Net;

namespace NSonic.Impl
{
    class SonicSessionFactory : ISonicSessionFactory
    {
        public ISonicSession Create(ISonicClient tcpClient)
        {
            return new SonicSession(tcpClient);
        }
    }
}
