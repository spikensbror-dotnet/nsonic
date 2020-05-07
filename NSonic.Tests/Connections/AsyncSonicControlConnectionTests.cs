using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class AsyncSonicControlConnectionTests : SonicConnectionTestBase
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

            this.SetupSuccessfulConnectAsync(new MockSequence());
        }

        [TestMethod]
        public async Task ShouldBeAbleToConnect()
        {
            await this.connection.ConnectAsync();
        }

        [TestMethod]
        public async Task InfoShouldReturnServerInfo()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteResultAsync(this.Session.Object, "INFO"))
                .Returns(Task.FromResult("example"));

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual("example", await this.connection.InfoAsync());
        }

        [TestMethod]
        public async Task TriggerShouldTriggerAnAction()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOkAsync(this.Session.Object, "TRIGGER", "testing", "test_data"))
                .Returns(Task.CompletedTask);
                ;

            // Act

            await this.connection.ConnectAsync();
            await this.connection.TriggerAsync("testing", "test_data");

            // Assert

            this.RequestWriter.VerifyAll();
        }
    }
}
