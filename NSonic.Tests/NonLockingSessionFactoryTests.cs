using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using System.Threading;

namespace NSonic.Tests
{
    [TestClass]
    public class NonLockingSessionFactoryTests
    {
        [TestMethod]
        public void ShouldCreateNonLockingSession()
        {
            // Arrange

            var factory = new NonLockingSessionFactory();
            var tcpClient = Mock.Of<IClient>(stc => stc.Semaphore == new SemaphoreSlim(1, 1));
            var environment = new EnvironmentResponse(1, 42);

            // Act

            var result = factory.Create(tcpClient);

            // Assert

            Assert.IsInstanceOfType(result, typeof(Session));
        }
    }
}
