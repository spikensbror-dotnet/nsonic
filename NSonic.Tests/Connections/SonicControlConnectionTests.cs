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

            this.connection = new SonicControlConnection(this.SessionFactoryProvider
                , this.RequestWriter.Object
                , Hostname
                , Port
                , Secret
                );

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

            this.RequestWriter
                .Setup(rw => rw.WriteResult(this.Session.Object, "INFO"))
                .Returns("example");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual("example", this.connection.Info());
        }

        [TestMethod]
        public void TriggerShouldTriggerAnAction()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOk(this.Session.Object, "TRIGGER", "testing", "test_data"))
                .Verifiable()
                ;

            // Act

            this.connection.Connect();
            this.connection.Trigger("testing", "test_data");

            // Assert

            this.RequestWriter.Verify();
        }
    }
}
