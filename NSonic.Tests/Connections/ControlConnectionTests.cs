using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class ControlConnectionTests : TestBase
    {
        private ControlConnection connection;

        internal override ConnectionMode Mode => ConnectionMode.Control;
        protected override bool Async => false;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new ControlConnection(this.SessionFactory
                , new RequestWriter()
                , this.Client
                , StubConstants.Hostname
                , StubConstants.Port
                , StubConstants.Secret
                );
        }

        [TestMethod]
        public async Task Info_ShouldReturnServerInfo()
        {
            // Arrange

            var expected = "This is some info";

            this.SetupWriteWithResult(expected, "INFO");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(expected, await this.connection.InfoAsync());
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(expected, this.connection.Info());
            }
        }

        [TestMethod]
        public async Task Trigger_ShouldTriggerServerAction()
        {
            // Arrange

            var action = "testing";
            var data = "This is some test data";

            this.SetupWriteWithOk("TRIGGER", action, data);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.TriggerAsync(action, data);
            }
            else
            {
                this.connection.Connect();

                this.connection.Trigger(action, data);
            }

            // Assert

            this.VerifyAll();
        }
    }
}
