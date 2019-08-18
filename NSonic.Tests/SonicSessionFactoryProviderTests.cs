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

        [TestMethod]
        public void ShouldCreateSonicSessionFactoryAndSessionsForProvidedEndpoint()
        {
            using (var sessionFactory = this.provider.Create("localhost", Port))
            {
                Assert.IsNotNull(sessionFactory);

                using (var session = sessionFactory.Create())
                {
                    Assert.IsNotNull(session);
                }
            }
        }
    }
}
