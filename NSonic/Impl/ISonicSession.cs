using System;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicSession : IDisposable
    {
        string Read();
        Task<string> ReadAsync();
        void Write(params string[] args);
        Task WriteAsync(params string[] args);
    }
}
