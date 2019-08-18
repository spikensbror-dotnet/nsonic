using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl.Net;
using System.Net;
using System.Net.Sockets;

namespace NSonic.Tests.Net
{
    [TestClass]
    public class TcpClientAdapterTests
    {
        private const int Port = 14995;

        private TcpListener listener;

        [TestInitialize]
        public void Initialize()
        {
            this.listener = new TcpListener(IPAddress.Any, Port);
            this.listener.Start();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.listener.Stop();
        }

        [TestMethod]
        public void ShouldProvideAccessToInternalNetworkStream()
        {
            using (var client = new TcpClientAdapter("localhost", Port))
            {
                Assert.IsNotNull(client.GetStream());
            }
        }
    }
}
