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
    public class SonicSessionTests
    {
        private Mock<ITcpClient> client;

        private EnvironmentResponse environment;
        private MemoryStream stream;
        private StreamWriter writer;
        private StreamReader reader;

        private SonicSession session;

        protected virtual bool Async => false;

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
        public async Task Read_ShouldBeAbleToReadFromStream()
        {
            // Arrange

            var expected = "THIS IS A TEST";

            this.writer.WriteLine(expected);
            this.writer.Flush();

            this.stream.Seek(0, SeekOrigin.Begin);

            // Act / Assert

            if (this.Async)
            {
                Assert.AreEqual(expected, await this.session.ReadAsync());
            }
            else
            {
                Assert.AreEqual(expected, this.session.Read());
            }
        }

        [TestMethod]
        public async Task Write_ShouldBeAbleToWriteToStream()
        {
            // Arrange

            var expected = "THIS IS EXPECTED";

            // Act

            if (this.Async)
            {
                await this.session.WriteAsync(expected);
            }
            else
            {
                this.session.Write(expected);
            }

            // Assert

            this.stream.Seek(0, SeekOrigin.Begin);
            var result = this.reader.ReadLine();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Write_ShouldThrowAssertionExceptionIfAttemptingToWriteTooLongMessage()
        {
            // Arrange

            var expected = "".PadRight(this.environment.MaxBufferStringLength + 1, '+');

            // Act

            if (this.Async)
            {
                await this.session.WriteAsync(expected);
            }
            else
            {
                this.session.Write(expected);
            }
        }
    }
}
