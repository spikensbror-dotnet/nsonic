using System;
using System.IO;
using System.Threading.Tasks;

namespace NSonic.Impl.Net
{
    interface ITcpClient : IDisposable
    {
        bool Connected { get; }
        void Connect(string hostname, int port);
        Task ConnectAsync(string hostname, int port);
        Stream GetStream();
    }
}
