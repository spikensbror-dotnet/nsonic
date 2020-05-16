using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Net;
using System.Threading;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicSessionFactoryTests
    {
        [TestMethod]
        public void ShouldBeAbleToCreateSession()
        {
            // Arrange

            var factory = new SonicSessionFactory();
            var tcpClient = Mock.Of<ITcpClient>(tc => tc.Semaphore == new SemaphoreSlim(1, 1));
            var environment = new EnvironmentResponse(1, 42);

            // Act

            var result = factory.Create(tcpClient, environment);

            // Assert

            Assert.AreSame(tcpClient, ((SonicSession)result).Client);
            Assert.AreEqual(environment, ((SonicSession)result).Environment);
        }
    }
}
