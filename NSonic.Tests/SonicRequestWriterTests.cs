using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using System;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicRequestWriterTests
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
        public void WriteOkShouldWriteAndAssertOkResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.Write("TEST", "HELLO", "WORLD"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("OK")
                ;

            // Act

            this.writer.WriteOk(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void WriteOkShouldThrowAssertionExceptionIfOkResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.Write("TEST", "HELLO", "WORLD"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("NOT_OK")
                ;

            // Act

            this.writer.WriteOk(this.session.Object, "TEST", "HELLO", "WORLD");
        }

        [TestMethod]
        public void WriteResultShouldWriteAndAssertResultResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.Write("TEST", "HELLO", "WORLD"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("RESULT hello sir")
                ;

            // Act

            var result = this.writer.WriteResult(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            Assert.AreEqual("hello sir", result);

            this.session.VerifyAll();
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void WriteResultShouldThrowAssertionExceptionIfResultResponseIsNotReceived()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.Write("TEST", "HELLO", "WORLD"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("NOT_RESULT testing")
                ;

            // Act

            this.writer.WriteResult(this.session.Object, "TEST", "HELLO", "WORLD");
        }

        [TestMethod]
        public void WriteStartShouldStartSessionAndReturnEnvironmentResponse()
        {
            // Arrange

            var sequence = new MockSequence();

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("CONNECTED <sonic-server v1.00>")
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Write("START", "TEST", "HELLO"))
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"STARTED TEST protocol(1) buffer(20000)")
                ;

            // Act

            var result = this.writer.WriteStart(this.session.Object, "TEST", "HELLO");

            // Assert

            Assert.AreEqual(1, result.Protocol);
            Assert.AreEqual(20000, result.Buffer);
            Assert.AreEqual((int)Math.Floor(20000 * 0.5 / 4), result.MaxBufferStringLength);
        }


        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void WriteStartShouldThrowAssertionExceptionIfServerDoesNotStartBySendingConnectionConfirmation()
        {
            // Arrange

            this.session
                .Setup(s => s.Read())
                .Returns("NOT_CONNECTED");

            // Act

            this.writer.WriteStart(this.session.Object, "TEST", "HELLO");
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void WriteStartShouldThrowAssertionExceptionIfStartCommandDoesNotRespondWithConfiguration()
        {
            // Arrange

            this.session
                .SetupSequence(s => s.Read())
                .Returns("CONNECTED <sonic-server v1.00>")
                .Returns("NOT_STARTED")
                ;

            this.session
                .Setup(s => s.Write("START", "TEST", "HELLO"))
                ;

            // Act

            this.writer.WriteStart(this.session.Object, "TEST", "HELLO");
        }
    }
}
