using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl;
using System.Net;
using System.Net.Sockets;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicSessionFactoryProviderTests
    {
        private const int Port = 14996;

        private TcpListener listener;

        private SonicSessionFactoryProvider provider;

        [TestInitialize]
        public void Initialize()
        {
            this.listener = new TcpListener(IPAddress.Any, Port);
            this.listener.Start();

            this.provider = new SonicSessionFactoryProvider();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.listener.Stop();
        }

        [TestMethod]
        public void ShouldCreateSonicSessionFactoryAndSessionsForProvidedEndpoint()
        {
            using (var sessionFactory = this.provider.Create("localhost", Port))
            {
                Assert.IsNotNull(sessionFactory);
                Assert.IsInstanceOfType(sessionFactory, typeof(SonicSessionFactory));
            }
        }
    }
}
