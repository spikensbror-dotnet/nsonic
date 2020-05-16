using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Net;
using NSonic.Tests.Stubs;
using System.IO;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    /// <summary>
    /// Most code of this unit is covered via other tests, these unit tests only serve
    /// to cover the cases not covered elsewhere.
    /// </summary>
    [TestClass]
    public class ClientTests
    {
        private Mock<IClientConnector> connector;
        private Mock<ITcpClient> tcpClient;

        private Configuration configuration;

        private Client client;

        [TestInitialize]
        public void Initialize()
        {
            this.connector = new Mock<IClientConnector>();
            this.tcpClient = new Mock<ITcpClient>(MockBehavior.Strict);

            this.configuration = new Configuration(StubConstants.Hostname, StubConstants.Port, StubConstants.Secret, ConnectionMode.Control);

            this.client = new Client(this.connector.Object, this.tcpClient.Object);
            this.client.Configure(this.configuration);
        }

        [TestMethod]
        public void Semaphore_ShouldProvideSingleCapacitySemaphore()
        {
            // Act

            var semaphore = this.client.Semaphore;

            // Assert

            Assert.AreEqual(1, semaphore.CurrentCount);
            Assert.AreSame(semaphore, this.client.Semaphore);
        }

        [TestMethod]
        public void GetStream_ShouldConnectIfNotConnected()
        {
            // Arrange

            var sequence = new MockSequence();
            var expectedEnvironment = new EnvironmentResponse(42, 32);
            using var expectedStream = new MemoryStream();

            this.tcpClient
                .InSequence(sequence)
                .Setup(tc => tc.Connected)
                .Returns(false);

            this.connector
                .InSequence(sequence)
                .Setup(c => c.Connect(this.client, this.tcpClient.Object, this.configuration))
                .Returns(expectedEnvironment);

            this.tcpClient
                .InSequence(sequence)
                .Setup(tc => tc.GetStream())
                .Returns(expectedStream);

            // Act

            var result = this.client.GetStream();

            // Assert

            Assert.AreSame(expectedStream, result);
            Assert.AreEqual(expectedEnvironment, this.client.Environment);
        }

        [TestMethod]
        public async Task GetStreamAsync_ShouldConnectIfNotConnected()
        {
            // Arrange

            var sequence = new MockSequence();
            var expectedEnvironment = new EnvironmentResponse(42, 32);
            using var expectedStream = new MemoryStream();

            this.tcpClient
                .InSequence(sequence)
                .Setup(tc => tc.Connected)
                .Returns(false);

            this.connector
                .InSequence(sequence)
                .Setup(c => c.ConnectAsync(this.client, this.tcpClient.Object, this.configuration))
                .Returns(Task.FromResult(expectedEnvironment));

            this.tcpClient
                .InSequence(sequence)
                .Setup(tc => tc.GetStream())
                .Returns(expectedStream);

            // Act

            var result = await this.client.GetStreamAsync();

            // Assert

            Assert.AreSame(expectedStream, result);
            Assert.AreEqual(expectedEnvironment, this.client.Environment);
        }
    }
}
