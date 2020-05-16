using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Impl.Net;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class ConnectionTests : TestBase
    {
        private Fixture connection;

        internal override ConnectionMode Mode => ConnectionMode.Ingest;
        protected override bool Async => false;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new Fixture(this.SessionFactory
                , this.RequestWriter
                , this.Client
                , StubConstants.Configuration
                );
        }

        [TestMethod]
        public async Task Connect_ShouldBeAbleToConnect()
        {
            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();
            }
            else
            {
                this.connection.Connect();
            }

            // Assert

            Assert.AreEqual(StubConstants.ConnectedEnvironment, this.connection.client.Environment);
        }

        [TestMethod]
        public void Dispose_ShouldDoNothingIfNotConnected()
        {
            // Act

            this.connection.Dispose();
        }

        [TestMethod]
        public async Task Dispose_ShouldQuitIfConnected()
        {
            // Arrange

            this.Session
                .SetupWrite(this.Sequence, false, "QUIT")
                .SetupRead(this.Sequence, false, "ENDED quit")
                ;

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();
            }
            else
            {
                this.connection.Connect();
            }

            this.connection.Dispose();

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task ShouldFailSilentlyWhenQuittingOnDispose()
        {
            // Arrange

            this.Session
                .SetupWrite(this.Sequence, false, "QUIT")
                .SetupRead(this.Sequence, false, "NOT_ENDED quit")
                ;

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();
            }
            else
            {
                this.connection.Connect();
            }

            this.connection.Dispose();

            // Assert

            this.VerifyAll();
        }

        class Fixture : Connection
        {
            public Fixture(ISessionFactory sessionFactory
                , IRequestWriter requestWriter
                , IDisposableSonicClient tcpClient
                , Configuration configuration
                )
                : base(sessionFactory, requestWriter, tcpClient, configuration)
            {
                //
            }

            protected override ConnectionMode Mode => ConnectionMode.Ingest;
        }
    }
}
