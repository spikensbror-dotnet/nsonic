using NSonic.Impl.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class Client : IDisposableSonicClient
    {
        private readonly IClientConnector connector;
        private readonly ITcpClient tcpClient;

        private Configuration configuration;

        public Client(IClientConnector connector
            , ITcpClient tcpClient
            )
        {
            this.connector = connector;
            this.tcpClient = tcpClient;
        }

        public EnvironmentResponse Environment { get; private set; } = EnvironmentResponse.Default;
        public SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1, 1);

        public void Configure(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void Connect()
        {
            if (!this.tcpClient.Connected)
            {
                this.Environment = this.connector.Connect(this, this.tcpClient, this.configuration);
            }
        }

        public async Task ConnectAsync()
        {
            if (!this.tcpClient.Connected)
            {
                this.Environment = await this.connector.ConnectAsync(this, this.tcpClient, this.configuration);
            }
        }

        public void Dispose()
        {
            if (this.tcpClient.Connected)
            {
                this.connector.Disconnect(this);
            }

            this.tcpClient.Dispose();
        }

        public Stream GetStream()
        {
            this.Connect();

            return this.tcpClient.GetStream();
        }

        public async Task<Stream> GetStreamAsync()
        {
            await this.ConnectAsync();

            return this.tcpClient.GetStream();
        }
    }
}
