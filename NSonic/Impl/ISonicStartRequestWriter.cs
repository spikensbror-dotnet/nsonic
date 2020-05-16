using NSonic.Impl.Net;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicStartRequestWriter
    {
        EnvironmentResponse WriteStart(ISonicSession session, ConnectionMode mode, string secret);
        Task<EnvironmentResponse> WriteStartAsync(ISonicSession session, ConnectionMode mode, string secret);
    }
}
