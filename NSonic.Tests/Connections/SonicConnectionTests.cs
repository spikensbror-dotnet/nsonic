using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicConnectionTests : SonicConnectionTestBase
    {
        private Fixture fixture;

        protected override string Mode => "FIXTUREMODE";

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.fixture = new Fixture(this.SessionFactoryProvider
                , this.RequestWriter.Object
                , Hostname
                , Port
                , Secret
                );
        }

        [TestMethod]
        public void ShouldBeAbleToConnect()
        {
            // Arrange

            this.SetupSuccessfulConnect(new MockSequence());

            // Act

            this.fixture.Connect();

            // Assert

            Assert.AreEqual(1, this.fixture.Environment.Protocol);
            Assert.AreEqual(20000, this.fixture.Environment.Buffer);
        }

        [TestMethod]
        public async Task ShouldBeAbleToConnectAsync()
        {
            // Arrange

            this.SetupSuccessfulConnectAsync(new MockSequence());

            // Act

            await this.fixture.ConnectAsync();

            // Assert

            Assert.AreEqual(1, this.fixture.Environment.Protocol);
            Assert.AreEqual(20000, this.fixture.Environment.Buffer);
        }

        [TestMethod]
        public void ShouldDoNothingOnDisposeIfNotConnected()
        {
            // Act

            this.fixture.Dispose();
        }

        [TestMethod]
        public void ShouldQuitOnDisposeIfConnected()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupSuccessfulConnect(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("QUIT"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("ENDED quit")
                ;

            // Act

            this.fixture.Connect();
            this.fixture.Dispose();

            // Assert

            this.Session.Verify(s => s.Write("QUIT"), Times.Once);
        }

        [TestMethod]
        public void ShouldFailSilentlyWhenQuittingOnDispose()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupSuccessfulConnect(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("QUIT"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("NOT_ENDED quit")
                ;

            // Act

            this.fixture.Connect();
            this.fixture.Dispose();

            // Assert

            this.Session.Verify(s => s.Write("QUIT"), Times.Once);
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

            protected override string Mode => "FIXTUREMODE";
        }
    }
}
