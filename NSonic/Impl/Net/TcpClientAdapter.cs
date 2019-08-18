using System.IO;
using System.Net.Sockets;

namespace NSonic.Impl.Net
{
    class TcpClientAdapter : ITcpClient
    {
        private readonly TcpClient client;

        public TcpClientAdapter(string hostname, int port)
        {
            this.client = new TcpClient(hostname, port);
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
