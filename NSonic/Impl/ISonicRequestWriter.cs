using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicRequestWriter
    {
        string WriteResult(ISonicSession session, params string[] args);
        Task<string> WriteResultAsync(ISonicSession session, params string[] args);

        void WriteOk(ISonicSession session, params string[] args);
        Task WriteOkAsync(ISonicSession session, params string[] args);

        EnvironmentResponse WriteStart(ISonicSession session, string mode, string secret);
        Task<EnvironmentResponse> WriteStartAsync(ISonicSession session, string mode, string secret);
    }
}
