using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl.Net
{
    class TcpClientAdapter : IDisposableTcpClient
    {
        private readonly TcpClient client;

        public TcpClientAdapter()
        {
            this.client = new TcpClient();

            this.Semaphore = new SemaphoreSlim(1, 1);
        }

        public SemaphoreSlim Semaphore { get; }

        public bool Connected => this.client.Connected;

        public void Connect(string hostname, int port)
        {
            this.client.Connect(hostname, port);
        }

        public async Task ConnectAsync(string hostname, int port)
        {
            await this.client.ConnectAsync(hostname, port);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        public Stream GetStream()
        {
            return this.client.GetStream();
        }
    }
}
