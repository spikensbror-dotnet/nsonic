using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicRequestWriter : ISonicStartRequestWriter
    {
        string WriteResult(ISonicSession session, params string[] args);
        Task<string> WriteResultAsync(ISonicSession session, params string[] args);

        void WriteOk(ISonicSession session, params string[] args);
        Task WriteOkAsync(ISonicSession session, params string[] args);
    }
}
