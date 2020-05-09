using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicConnectionTests : TestBase
    {
        private Fixture connection;

        protected override string Mode => "fixture";
        protected override bool Async => false;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new Fixture(this.SessionFactoryProvider
                , new SonicRequestWriter()
                , StubConstants.Hostname
                , StubConstants.Port
                , StubConstants.Secret
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

            Assert.AreEqual(StubConstants.ConnectedEnvironment, this.connection.Environment);
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

        class Fixture : SonicConnection
        {
            public Fixture(ISonicSessionFactoryProvider sessionFactoryProvider
                , ISonicRequestWriter requestWriter
                , string hostname
                , int port
                , string secret
                )
                : base(sessionFactoryProvider, requestWriter, hostname, port, secret)
            {
                //
            }

            protected override string Mode => "fixture";
        }
    }
}
