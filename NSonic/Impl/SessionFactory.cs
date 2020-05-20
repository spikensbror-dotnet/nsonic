namespace NSonic.Impl
{
    class SessionFactory : ISessionFactory
    {
        public ISession Create(IClient client)
        {
            return new RetryingSession(new LockingSession(new Session(client), client));
        }
    }
}
