using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests
{
    [TestClass]
    public class NSonicFactoryTests
    {
        private const string Hostname = "localhost";
        private const int Port = 1491;
        private const string Secret = "SecretPassword";

        [TestMethod]
        public void ShouldBeAbleToCreateSearchConnection()
        {
            using (var connection = NSonicFactory.Search(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToCreateControlConnection()
        {
            using (var connection = NSonicFactory.Control(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToCreateIngestConnection()
        {
            using (var connection = NSonicFactory.Ingest(Hostname, Port, Secret))
            {
                Assert.IsNotNull(connection);
            }
        }
    }
}
