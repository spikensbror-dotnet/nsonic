using NSonic.Impl.Net;

namespace NSonic.Impl
{
    interface ISessionFactory
    {
        ISession Create(IClient client);
    }
}
