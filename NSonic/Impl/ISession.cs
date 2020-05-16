using System;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISession : IDisposable
    {
        string Read();
        Task<string> ReadAsync();
        void Write(params string[] args);
        Task WriteAsync(params string[] args);
    }
}
