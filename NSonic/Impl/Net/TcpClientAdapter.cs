using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl.Net
{
    class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient client;

        public TcpClientAdapter()
        {
            this.client = new TcpClient();
        }

        public bool Connected => this.client.Connected;

        public virtual void Connect(string hostname, int port)
        {
            this.client.Connect(hostname, port);
        }

        public virtual async Task ConnectAsync(string hostname, int port)
        {
            await this.client.ConnectAsync(hostname, port);
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        public virtual Stream GetStream()
        {
            return this.client.GetStream();
        }
    }
}
