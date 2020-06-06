using System.Threading.Tasks;

namespace NSonic.Impl
{
    class LockingSession : ISession
    {
        private readonly ISession session;
        private readonly IClient client;

        public LockingSession(ISession session, IClient client)
        {
            this.session = session;
            this.client = client;
            this.client.Semaphore.Wait();
        }

        public void Dispose()
        {
            this.client.Semaphore.Release();
            this.session.Dispose();
        }

        public string Read()
        {
            return this.session.Read();
        }

        public Task<string> ReadAsync()
        {
            return this.session.ReadAsync();
        }

        public void Write(params string[] args)
        {
            this.session.Write(args);
        }

        public Task WriteAsync(params string[] args)
        {
            return this.session.WriteAsync(args);
        }
    }
}
