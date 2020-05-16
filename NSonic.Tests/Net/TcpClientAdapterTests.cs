using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl.Net;
using System.IO;
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
        public void ShouldBeAbleToConnectAndReconnectSynchronously()
        {
            using var client = new TcpClientAdapter();
            var semaphore = client.Semaphore;

            Assert.IsFalse(client.Connected);

            client.Connect("localhost", Port);

            Assert.AreNotSame(semaphore, client.Semaphore);
            Assert.IsTrue(client.Connected);

            client.Connect("localhost", Port);

            Assert.IsTrue(client.Connected);
        }

        [TestMethod]
        public async Task ShouldBeAbleToConnectAndReconnectAsynchronously()
        {
            using var client = new TcpClientAdapter();
            var semaphore = client.Semaphore;

            Assert.IsFalse(client.Connected);

            await client.ConnectAsync("localhost", Port);

            Assert.IsTrue(client.Connected);
            Assert.AreNotSame(semaphore, client.Semaphore);

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
        public void ShouldProvideConnectionStatus()
        {
            using var client = new TcpClientAdapter();

            client.Connect("localhost", Port);

            Assert.IsTrue(client.Connected);

            this.listener.Stop();

            try
            {
                // Since Connected only reports as of the last operation, we must send or receive data to
                // get the state as of now.
                client.GetStream().Write(new byte[0], 0, 0);

                // To deal with reconnection in real scenarios, the resulting IOException should be caught and
                // then attempt a retry with the exact same client.
            }
            catch (IOException)
            {
                //
            }

            Assert.IsFalse(client.Connected);

            this.listener.Start();
        }
    }
}
