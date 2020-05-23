using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    abstract class Connection : ISonicConnection
    {
        private readonly ISessionFactory sessionFactory;
        internal readonly IDisposableClient client;
        private readonly Configuration configuration;

        protected Connection(ISessionFactory sessionFactory
            , IRequestWriter requestWriter
            , IDisposableClient client
            , Configuration configuration
            )
        {
            this.sessionFactory = sessionFactory;
            this.RequestWriter = requestWriter;
            this.client = client;
            this.configuration = configuration;

            this.client.Configure(this.configuration.WithMode(this.Mode));
        }

        protected abstract ConnectionMode Mode { get; }

        protected IRequestWriter RequestWriter { get; }

        public void Connect()
        {
            this.client.Connect();
        }

        public async Task ConnectAsync()
        {
            await this.client.ConnectAsync();
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        protected ISession CreateSession()
        {
            return this.sessionFactory.Create(this.client);
        }
    }
}
