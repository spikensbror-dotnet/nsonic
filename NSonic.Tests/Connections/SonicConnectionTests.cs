using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;

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
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void ShouldThrowAssertionExceptionIfServerDoesNotStartBySendingConnectionConfirmation()
        {
            // Arrange

            this.Session
                .Setup(s => s.Read())
                .Returns("NOT_CONNECTED");

            // Act

            this.fixture.Connect();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void ShouldThrowAssertionExceptionIfStartCommandDoesNotRespondWithConfiguration()
        {
            // Arrange

            this.Session
                .SetupSequence(s => s.Read())
                .Returns("CONNECTED <sonic-server v1.00>")
                .Returns("NOT_STARTED")
                ;

            this.Session
                .Setup(s => s.Write("START", this.Mode, Secret))
                ;

            // Act

            this.fixture.Connect();
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
