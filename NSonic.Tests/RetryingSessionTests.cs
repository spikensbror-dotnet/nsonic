using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using System.IO;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    [TestClass]
    public class RetryingSessionTests
    {
        private Mock<ISession> innerSession;

        private RetryingSession session;

        [TestInitialize]
        public void Initialize()
        {
            this.innerSession = new Mock<ISession>(MockBehavior.Strict);

            this.session = new RetryingSession(this.innerSession.Object);
        }

        [TestMethod]
        public void Dispose_ShouldDisposeInnerSession()
        {
            // Arrange

            this.innerSession
                .Setup(s => s.Dispose())
                ;

            // Act

            this.session.Dispose();

            // Assert

            this.innerSession.Verify(s => s.Dispose(), Times.Once);
        }

        [TestMethod]
        public void Read_ShouldRetryOnIOExceptions()
        {
            // Arrange

            var expected = "test";
            var sequence = new MockSequence();

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns(expected);

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Throws(new IOException());

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns(expected);

            // Act / Assert

            Assert.AreEqual(expected, this.session.Read());
            Assert.AreEqual(expected, this.session.Read());

            this.innerSession.Verify(s => s.Read(), Times.Exactly(3));
        }

        [TestMethod]
        public async Task ReadAsync_ShouldRetryOnIOExceptions()
        {
            // Arrange

            var expected = "test";
            var sequence = new MockSequence();

            this.innerSession
                .InSequence(sequence)
                .Setup(S => S.ReadAsync())
                .ReturnsAsync(expected);

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .ThrowsAsync(new IOException());

            this.innerSession
                .InSequence(sequence)
                .Setup(S => S.ReadAsync())
                .ReturnsAsync(expected);

            // Act / Assert

            Assert.AreEqual(expected, await this.session.ReadAsync());
            Assert.AreEqual(expected, await this.session.ReadAsync());

            this.innerSession.Verify(s => s.ReadAsync(), Times.Exactly(3));
        }

        [TestMethod]
        public void Write_ShouldRetryOnIOExceptions()
        {
            // Arrange

            var args = new[] { "hello", "world" };
            var sequence = new MockSequence();

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Write(args))
                ;

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Write(args))
                .Throws(new IOException())
                ;

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.Write(args))
                ;

            // Act / Assert

            this.session.Write(args);
            this.session.Write(args);

            this.innerSession.Verify(s => s.Write(args), Times.Exactly(3));
        }

        [TestMethod]
        public async Task WriteAsync_ShouldRetryOnIOExceptions()
        {
            // Arrange

            var args = new[] { "hello", "world" };
            var sequence = new MockSequence();

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.WriteAsync(args))
                .Returns(Task.CompletedTask)
                ;

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.WriteAsync(args))
                .ThrowsAsync(new IOException())
                ;

            this.innerSession
                .InSequence(sequence)
                .Setup(s => s.WriteAsync(args))
                .Returns(Task.CompletedTask)
                ;

            // Act / Assert

            await this.session.WriteAsync(args);
            await this.session.WriteAsync(args);

            this.innerSession.Verify(s => s.WriteAsync(args), Times.Exactly(3));
        }
    }
}
