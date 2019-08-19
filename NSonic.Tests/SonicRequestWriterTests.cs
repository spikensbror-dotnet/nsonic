using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;

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
                .Verifiable()
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("OK")
                .Verifiable()
                ;

            // Act

            this.writer.WriteOk(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            this.session.Verify();
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
                .Verifiable()
                ;

            this.session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("RESULT hello sir")
                .Verifiable()
                ;

            // Act

            var result = this.writer.WriteResult(this.session.Object, "TEST", "HELLO", "WORLD");

            // Assert

            Assert.AreEqual("hello sir", result);

            this.session.Verify();
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
    }
}
