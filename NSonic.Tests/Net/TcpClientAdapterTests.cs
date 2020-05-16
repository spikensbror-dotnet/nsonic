using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl.Net;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

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
        public void ShouldBeAbleToConnectSynchronously()
        {
            using var client = new TcpClientAdapter();

            Assert.IsFalse(client.Connected);

            client.Connect("localhost", Port);

            Assert.IsTrue(client.Connected);
        }

        [TestMethod]
        public async Task ShouldBeAbleToConnectAsynchronously()
        {
            using var client = new TcpClientAdapter();

            Assert.IsFalse(client.Connected);

            await client.ConnectAsync("localhost", Port);

            Assert.IsTrue(client.Connected);
        }

        [TestMethod]
        public void ShouldProvideAccessToInternalNetworkStream()
        {
            using var client = new TcpClientAdapter();

            client.Connect("localhost", Port);

            Assert.IsNotNull(client.GetStream());
        }

        [TestMethod]
        public void ShouldProvideSemaphore()
        {
            using var client = new TcpClientAdapter();

            Assert.AreEqual(1, client.Semaphore.CurrentCount);
        }
    }
}
