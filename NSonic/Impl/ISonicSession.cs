using System;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface ISonicSession : IDisposable
    {
        string Read();
        void Write(params string[] args);
    }
}
