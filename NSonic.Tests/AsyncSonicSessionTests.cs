using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    [TestClass]
    public class AsyncSonicSessionTests
    {
        private Mock<ITcpClient> client;

        private EnvironmentResponse environment;
        private MemoryStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private SonicSession session;

        [TestInitialize]
        public void Initialize()
        {
            this.client = new Mock<ITcpClient>();

            this.environment = new EnvironmentResponse(1, 20000);
            this.stream = new MemoryStream();
            this.writer = new StreamWriter(this.stream);
            this.reader = new StreamReader(this.stream);

            this.client
                .Setup(c => c.GetStream())
                .Returns(() => this.stream);

            this.session = new SonicSession(this.client.Object, new SemaphoreSlim(1, 1), environment);
        }

        [TestMethod]
        public async Task ShouldBeAbleToReadFromStream()
        {
            // Arrange

            var expected = "THIS IS A TEST";

            this.writer.WriteLine(expected);
            this.writer.Flush();

            this.stream.Seek(0, SeekOrigin.Begin);

            // Act

            var result = await this.session.ReadAsync();

            // Assert

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ShouldBeAbleToWriteToStream()
        {
            // Arrange

            var expected = "THIS IS EXPECTED";

            // Act

            await this.session.WriteAsync(expected);

            this.stream.Seek(0, SeekOrigin.Begin);
            var result = this.reader.ReadLine();

            // Assert

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task ShouldThrowAssertionExceptionIfAttemptingToWriteTooLongMessage()
        {
            // Arrange

            var expected = "".PadRight(this.environment.MaxBufferStringLength + 1, '+');

            // Act

            await this.session.WriteAsync(expected);
        }
    }
}
