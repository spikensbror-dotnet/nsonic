using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;

namespace NSonic.Tests.Stubs
{
    class StubSessionFactoryProvider : ISonicSessionFactoryProvider
    {
        public StubSessionFactoryProvider(MockSequence sequence, string mode, bool async)
        {
            this.PreConnectSession = new Mock<ISonicSession>(MockBehavior.Strict);
            this.PreConnectSession.Setup(pcs => pcs.Dispose());

            this.PreConnectSession
                .SetupRead(sequence, async, "CONNECTED <sonic-server v1.00>")
                .SetupWrite(sequence, async, "START", mode, StubConstants.Secret)
                .SetupRead(sequence, async, "STARTED control protocol(1) buffer(20002)")
                ;

            this.PostConnectSession = new Mock<ISonicSession>(MockBehavior.Strict);
            this.PostConnectSession.Setup(pcs => pcs.Dispose());
        }

        public Mock<ISonicSession> PreConnectSession { get; }
        public Mock<ISonicSession> PostConnectSession { get; }

        public ISonicSessionFactory Create(string hostname, int port)
        {
            Assert.AreEqual(StubConstants.Hostname, hostname);
            Assert.AreEqual(StubConstants.Port, port);

            var mockSessionFactory = new Mock<ISonicSessionFactory>(MockBehavior.Strict);

            mockSessionFactory
                .Setup(msf => msf.Create(EnvironmentResponse.Default))
                .Returns(this.PreConnectSession.Object);

            mockSessionFactory
                .Setup(ssf => ssf.Create(StubConstants.ConnectedEnvironment))
                .Returns(this.PostConnectSession.Object);

            mockSessionFactory
                .Setup(msf => msf.Dispose())
                ;

            return mockSessionFactory.Object;
        }
    }
}
