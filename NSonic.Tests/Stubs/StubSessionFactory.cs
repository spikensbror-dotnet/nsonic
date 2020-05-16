using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Tests.Stubs
{
    class StubSessionFactory : ISessionFactory
    {
        public StubSessionFactory(MockSequence sequence, ConnectionMode mode, bool async)
        {
            this.Semaphore = new SemaphoreSlim(1, 1);
            this.TcpClient = new Mock<ITcpClient>();

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

            this.PreConnectSession = new Mock<ISession>(MockBehavior.Strict);
            this.PreConnectSession.Setup(pcs => pcs.Dispose());

            this.PreConnectSession
                .SetupRead(sequence, async, "CONNECTED <sonic-server v1.00>")
                .SetupWrite(sequence, async, "START", mode.ToString().ToLowerInvariant(), StubConstants.Secret)
                .SetupRead(sequence, async, "STARTED control protocol(1) buffer(20002)")
                ;

            this.PostConnectSession = new Mock<ISession>(MockBehavior.Strict);
            this.PostConnectSession.Setup(pcs => pcs.Dispose());
        }

        public SemaphoreSlim Semaphore { get; }
        public Mock<ITcpClient> TcpClient { get; }
        public Mock<ISession> PreConnectSession { get; }
        public Mock<ISession> PostConnectSession { get; }

        public ISession Create(IClient tcpClient)
        {
            if (tcpClient.Environment.Equals(EnvironmentResponse.Default))
            {
                return this.PreConnectSession.Object;
            }
            else if (tcpClient.Environment.Equals(StubConstants.ConnectedEnvironment))
            {
                return this.PostConnectSession.Object;
            }

            throw new AssertFailedException("Invalid environment provided");
        }
    }
}
