using System;
using System.IO;

namespace NSonic.Impl.Net
{
    interface ITcpClient : IDisposable
    {
        Stream GetStream();
    }
}
