using NSonic.Impl.Net;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicClientConnector
    {
        EnvironmentResponse Connect(ISonicClient client, ITcpClient tcpClient, Configuration configuration);
        Task<EnvironmentResponse> ConnectAsync(ISonicClient client, ITcpClient tcpClient, Configuration configuration);
        void Disconnect(ISonicClient client);
    }
}
