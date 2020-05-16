using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    interface IClient
    {
        EnvironmentResponse Environment { get; }
        SemaphoreSlim Semaphore { get; }

        void Configure(Configuration configuration);
        void Connect();
        Task ConnectAsync();
        Stream GetStream();
        Task<Stream> GetStreamAsync();
    }
}
