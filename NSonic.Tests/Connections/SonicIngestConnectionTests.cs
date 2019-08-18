using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicIngestConnectionTests : SonicConnectionTestBase
    {
        protected override string Mode => "ingest";

        private SonicIngestConnection connection;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new SonicIngestConnection(this.SessionFactoryProvider, Hostname, Port, Secret);
            this.SetupSuccessfulConnect(new MockSequence());
        }

        [TestMethod]
        public void ShouldBeAbleToConnect()
        {
            this.connection.Connect();
        }
    }
}
