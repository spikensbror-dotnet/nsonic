using NSonic.Impl.Net;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class Client : IDisposableClient
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
        public SemaphoreSlim Semaphore => this.tcpClient.Semaphore;

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

        public StreamReader GetStreamReader()
        {
            this.Connect();

            return this.tcpClient.StreamReader;
        }

        public async Task<StreamReader> GetStreamReaderAsync()
        {
            await this.ConnectAsync();

            return this.tcpClient.StreamReader;
        }

        public StreamWriter GetStreamWriter()
        {
            this.Connect();

            return this.tcpClient.StreamWriter;
        }

        public async Task<StreamWriter> GetStreamWriterAsync()
        {
            await this.ConnectAsync();

            return this.tcpClient.StreamWriter;
        }
    }
}
