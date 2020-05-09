using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Tests.Stubs;
using System;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicRequestWriterTests
    {
        private Mock<ISonicSession> session;

        private SonicRequestWriter writer;

        protected virtual bool Async => false;

        [TestInitialize]
        public void Initialize()
        {
            this.session = new Mock<ISonicSession>(MockBehavior.Strict);

            this.writer = new SonicRequestWriter();
        }
        
        [TestMethod]
        public async Task WriteOk_ShouldWriteAndAssertOkResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupWrite(sequence, this.Async, "TEST", "HELLO", "WORLD")
                .SetupRead(sequence, this.Async, "OK")
                ;

            // Act

            if (this.Async)
            {
                await this.writer.WriteOkAsync(this.session.Object, "TEST", "HELLO", "WORLD");
            }
            else
            {
                this.writer.WriteOk(this.session.Object, "TEST", "HELLO", "WORLD");
            }

            // Assert

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteOk_ShouldThrowAssertionExceptionIfOkResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupWrite(sequence, this.Async, "TEST", "HELLO", "WORLD")
                .SetupRead(sequence, this.Async, "NOT_OK")
                ;

            // Act

            if (this.Async)
            {
                await this.writer.WriteOkAsync(this.session.Object, "TEST", "HELLO", "WORLD");
            }
            else
            {
                this.writer.WriteOk(this.session.Object, "TEST", "HELLO", "WORLD");
            }
        }

        [TestMethod]
        public async Task WriteResult_ShouldWriteAndAssertResultResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupWrite(sequence, this.Async, "TEST", "HELLO", "WORLD")
                .SetupRead(sequence, this.Async, "RESULT hello sir")
                ;

            // Act / Assert

            string result;

            if (this.Async)
            {
                result = await this.writer.WriteResultAsync(this.session.Object, "TEST", "HELLO", "WORLD");
            }
            else
            {
                result = this.writer.WriteResult(this.session.Object, "TEST", "HELLO", "WORLD");
            }

            // Assert

            Assert.AreEqual("hello sir", result);

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteResult_ShouldThrowAssertionExceptionIfResultResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupWrite(sequence, this.Async, "TEST", "HELLO", "WORLD")
                .SetupRead(sequence, this.Async, "NOT_RESULT testing")
                ;

            // Act

            if (this.Async)
            {
                await this.writer.WriteResultAsync(this.session.Object, "TEST", "HELLO", "WORLD");
            }
            else
            {
                this.writer.WriteResult(this.session.Object, "TEST", "HELLO", "WORLD");
            }
        }

        [TestMethod]
        public async Task WriteStart_ShouldStartSessionAndReturnEnvironmentResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupRead(sequence, this.Async, "CONNECTED <sonic-server v1.00>")
                .SetupWrite(sequence, this.Async, "START", "TEST", "HELLO")
                .SetupRead(sequence, this.Async, $"STARTED TEST protocol(1) buffer(20000)")
                ;

            // Act

            EnvironmentResponse result;
            if (this.Async)
            {
                result = await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");
            }
            else
            {
                result = this.writer.WriteStart(this.session.Object, "TEST", "HELLO");
            }

            // Assert

            Assert.AreEqual(1, result.Protocol);
            Assert.AreEqual(20000, result.Buffer);
            Assert.AreEqual((int)Math.Floor(20000 * 0.5 / 4), result.MaxBufferStringLength);
        }


        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteStart_ShouldThrowAssertionExceptionIfServerDoesNotStartBySendingConnectionConfirmation()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupRead(sequence, this.Async, "NOT_CONNECTED");

            // Act

            if (this.Async)
            {
                await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");
            }
            else
            {
                this.writer.WriteStart(this.session.Object, "TEST", "HELLO");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task WriteStart_ShouldThrowAssertionExceptionIfStartCommandDoesNotRespondWithConfiguration()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .SetupRead(sequence, this.Async, "CONNECTED <sonic-server v1.00>")
                .SetupWrite(sequence, this.Async, "START", "TEST", "HELLO")
                .SetupRead(sequence, this.Async, $"NOT_STARTED")
                ;

            // Act

            if (this.Async)
            {
                await this.writer.WriteStartAsync(this.session.Object, "TEST", "HELLO");
            }
            else
            {
                this.writer.WriteStart(this.session.Object, "TEST", "HELLO");
            }
        }
    }
}
