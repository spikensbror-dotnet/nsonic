namespace NSonic.Impl
{
    class SessionFactory : ISessionFactory
    {
        public ISession Create(IClient client)
        {
            return new LockingSession(new Session(client), client);
        }
    }
}
