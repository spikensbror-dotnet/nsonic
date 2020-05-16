using NSonic.Impl.Net;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface IClientConnector
    {
        EnvironmentResponse Connect(IClient client, ITcpClient tcpClient, Configuration configuration);
        Task<EnvironmentResponse> ConnectAsync(IClient client, ITcpClient tcpClient, Configuration configuration);
        void Disconnect(IClient client);
    }
}
