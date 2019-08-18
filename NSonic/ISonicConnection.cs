using System;

namespace NSonic
{
    public interface ISonicConnection : IDisposable
    {
        void Connect();
    }
}
