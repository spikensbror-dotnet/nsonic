using NSonic.Impl.Net;

namespace NSonic.Impl
{
    interface ISonicSessionFactory
    {
        ISonicSession Create(ISonicClient client);
    }
}
