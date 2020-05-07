using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using System;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    [TestClass]
    public class AsyncSonicRequestWriterTests
    {
        private Mock<ISonicSession> session;

        private SonicRequestWriter writer;

        [TestInitialize]
        public void Initialize()
        {
            this.session = new Mock<ISonicSession>(MockBehavior.Strict);

            this.writer = new SonicRequestWriter();
        }
        
        [TestMethod]
        public async Task WriteOkShouldWriteAndAssertOkResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("TEST", "HELLO", "WORLD"))
                .Returns(Task.CompletedTask)
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("OK"))
                ;

            // Act

            await this.writer.WriteOkAsync(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteOkShouldThrowAssertionExceptionIfOkResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("TEST", "HELLO", "WORLD"))
                .Returns(Task.CompletedTask)
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("NOT_OK"))
                ;

            // Act

            await this.writer.WriteOkAsync(this.session.Object, "TEST", "HELLO", "WORLD");
        }

        [TestMethod]
        public async Task WriteResultShouldWriteAndAssertResultResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("TEST", "HELLO", "WORLD"))
                .Returns(Task.CompletedTask)
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("RESULT hello sir"))
                ;

            // Act

            var result = await this.writer.WriteResultAsync(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            Assert.AreEqual("hello sir", result);

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteResultShouldThrowAssertionExceptionIfResultResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("TEST", "HELLO", "WORLD"))
                .Returns(Task.CompletedTask)
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("NOT_RESULT testing"))
                ;

            // Act

            await this.writer.WriteResultAsync(this.session.Object, "TEST", "HELLO", "WORLD");
        }

        [TestMethod]
        public async Task WriteStartShouldStartSessionAndReturnEnvironmentResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("CONNECTED <sonic-server v1.00>"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("START", "TEST", "HELLO"))
                .Returns(Task.CompletedTask)
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult($"STARTED TEST protocol(1) buffer(20000)"))
                ;

            // Act

            var result = await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");

            // Assert

            Assert.AreEqual(1, result.Protocol);
            Assert.AreEqual(20000, result.Buffer);
            Assert.AreEqual((int)Math.Floor(20000 * 0.5 / 4), result.MaxBufferStringLength);
        }


        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteStartShouldThrowAssertionExceptionIfServerDoesNotStartBySendingConnectionConfirmation()
        {
            // Arrange

            this.session
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult("NOT_CONNECTED"));

            // Act

            await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteStartShouldThrowAssertionExceptionIfStartCommandDoesNotRespondWithConfiguration()
        {
            // Arrange

            this.session
                .SetupSequence(s => s.ReadAsync())
                .Returns(Task.FromResult("CONNECTED <sonic-server v1.00>"))
                .Returns(Task.FromResult("NOT_STARTED"))
                ;

            this.session
                .Setup(s => s.WriteAsync("START", "TEST", "HELLO"))
                .Returns(Task.CompletedTask)
                ;

            // Act

            await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");
        }
    }
}
