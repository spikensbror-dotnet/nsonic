using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface IRequestWriter : IStartRequestWriter
    {
        string WriteResult(ISession session, params string[] args);
        Task<string> WriteResultAsync(ISession session, params string[] args);

        void WriteOk(ISession session, params string[] args);
        Task WriteOkAsync(ISession session, params string[] args);
    }
}
