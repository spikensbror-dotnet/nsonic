namespace NSonic.Impl
{
    class NonLockingSessionFactory : INonLockingSessionFactory
    {
        public ISession Create(IClient client)
        {
            return new Session(client);
        }
    }
}
