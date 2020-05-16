using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    abstract class SonicConnection : ISonicConnection
    {
        private readonly ISonicSessionFactory sessionFactory;
        internal readonly IDisposableSonicClient client;
        private readonly string hostname;
        private readonly int port;
        private readonly string secret;

        protected SonicConnection(ISonicSessionFactory sessionFactory
            , ISonicRequestWriter requestWriter
            , IDisposableSonicClient client
            , string hostname
            , int port
            , string secret
            )
        {
            this.sessionFactory = sessionFactory;
            this.RequestWriter = requestWriter;
            this.client = client;
            this.hostname = hostname;
            this.port = port;
            this.secret = secret;
        }

        protected abstract ConnectionMode Mode { get; }

        protected ISonicRequestWriter RequestWriter { get; }

        public void Connect()
        {
            this.client.Configure(new Configuration(this.hostname, this.port, this.secret, this.Mode));

            this.client.Connect();
        }

        public async Task ConnectAsync()
        {
            this.client.Configure(new Configuration(this.hostname, this.port, this.secret, this.Mode));

            await this.client.ConnectAsync();
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        protected ISonicSession CreateSession()
        {
            return this.sessionFactory.Create(this.client);
        }
    }
}
