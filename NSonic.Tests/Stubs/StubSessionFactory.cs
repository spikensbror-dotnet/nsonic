using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Tests.Stubs
{
    class StubSessionFactory : ISonicSessionFactory
    {
        public StubSessionFactory(MockSequence sequence, string mode, bool async)
        {
            this.Semaphore = new SemaphoreSlim(1, 1);
            this.TcpClient = new Mock<IDisposableTcpClient>();

            if (async)
            {
                this.TcpClient
                    .InSequence(sequence)
                    .Setup(tc => tc.ConnectAsync(StubConstants.Hostname, StubConstants.Port))
                    .Callback((string x, int y) =>
                    {
                        this.TcpClient.Setup(tc => tc.Connected).Returns(true);
                    })
                    .Returns(Task.CompletedTask)
                    ;
            }
            else
            {
                this.TcpClient
                    .InSequence(sequence)
                    .Setup(tc => tc.Connect(StubConstants.Hostname, StubConstants.Port))
                    .Callback((string x, int y) =>
                    {
                        this.TcpClient.Setup(tc => tc.Connected).Returns(true);
                    })
                    ;
            }

            this.TcpClient.Setup(tc => tc.Dispose());
            this.TcpClient.Setup(tc => tc.Semaphore).Returns(this.Semaphore);

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

        public SemaphoreSlim Semaphore { get; }
        public Mock<IDisposableTcpClient> TcpClient { get; }
        public Mock<ISonicSession> PreConnectSession { get; }
        public Mock<ISonicSession> PostConnectSession { get; }

        public ISonicSession Create(ITcpClient tcpClient, EnvironmentResponse environment)
        {
            Assert.AreSame(this.TcpClient.Object, tcpClient);

            if (environment.Equals(EnvironmentResponse.Default))
            {
                return this.PreConnectSession.Object;
            }
            else if (environment.Equals(StubConstants.ConnectedEnvironment))
            {
                return this.PostConnectSession.Object;
            }

            throw new AssertFailedException("Invalid environment provided");
        }
    }
}
