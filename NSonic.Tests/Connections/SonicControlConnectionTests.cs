using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicControlConnectionTests : SonicConnectionTestBase
    {
        protected override string Mode => "control";

        private SonicControlConnection connection;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new SonicControlConnection(this.SessionFactoryProvider, Hostname, Port, Secret);
            this.SetupSuccessfulConnect(new MockSequence());
        }

        [TestMethod]
        public void ShouldBeAbleToConnect()
        {
            this.connection.Connect();
        }

        [TestMethod]
        public void InfoShouldReturnServerInfo()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("INFO"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("RESULT example")
                ;

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual("RESULT example", this.connection.Info());
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void InfoShouldThrowAssertionExceptionIfResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("INFO"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("NOT_RESULT example");

            // Act

            this.connection.Connect();
            this.connection.Info();
        }

        [TestMethod]
        public void PingShouldDoNothingIfSuccessful()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("PING"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("PONG")
                ;

            // Act

            this.connection.Connect();
            this.connection.Ping();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void PingShouldThrowAssertionExceptionIfUnsuccessful()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("PING"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("WRONG")
                ;

            // Act

            this.connection.Connect();
            this.connection.Ping();
        }

        [TestMethod]
        public void TriggerShouldTriggerAnAction()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("TRIGGER", "testing", "test_data"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("OK")
                ;

            // Act

            this.connection.Connect();
            this.connection.Trigger("testing", "test_data");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void TriggerShouldThrowAssertionExceptionIfResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("TRIGGER", "testing", "test_data"))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("NOT_OK")
                ;

            // Act

            this.connection.Connect();
            this.connection.Trigger("testing", "test_data");
        }
    }
}
