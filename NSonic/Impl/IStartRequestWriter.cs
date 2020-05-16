using NSonic.Impl.Net;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface IStartRequestWriter
    {
        EnvironmentResponse WriteStart(ISession session, ConnectionMode mode, string secret);
        Task<EnvironmentResponse> WriteStartAsync(ISession session, ConnectionMode mode, string secret);
    }
}
