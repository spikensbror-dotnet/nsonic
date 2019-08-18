using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests
{
    [TestClass]
    public class NSonicTests
    {
        private const string Hostname = "localhost";
        private const int Port = 1491;
        private const string Secret = "SecretPassword";

        [TestMethod]
        public void ShouldBeAbleToCreateSearchConnection()
        {
            using (var connection = NSonic.Search(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToCreateControlConnection()
        {
            using (var connection = NSonic.Control(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToCreateIngestConnection()
        {
            using (var connection = NSonic.Ingest(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }
    }
}
