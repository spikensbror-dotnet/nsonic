namespace NSonic.Impl
{
    interface ISonicSessionFactoryProvider
    {
        ISonicSessionFactory Create(string hostname, int port);
    }
}
