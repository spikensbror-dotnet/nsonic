using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public abstract class SonicConnectionTestBase
    {
        protected const string Hostname = "testing";
        protected const int Port = 1337;
        protected const string Secret = "top_secret";

        protected abstract string Mode { get; }

        internal EnvironmentResponse Environment { get; private set; }
        internal Mock<ISonicSession> Session { get; private set; }
        internal Mock<ISonicRequestWriter> RequestWriter { get; private set; }
        internal ISonicSessionFactoryProvider SessionFactoryProvider { get; private set; }

        [TestInitialize]
        public virtual void Initialize()
        {
            this.Environment = new EnvironmentResponse(1, 20000);
            this.Session = new Mock<ISonicSession>(MockBehavior.Strict);
            this.Session.Setup(s => s.Dispose());

            this.RequestWriter = new Mock<ISonicRequestWriter>(MockBehavior.Strict);

            this.SessionFactoryProvider = Mock.Of<ISonicSessionFactoryProvider>(ssfp => ssfp.Create(Hostname, Port).Create(this.Environment) == this.Session.Object);
        }

        protected void SetupSuccessfulConnect(MockSequence sequence)
        {
            this.RequestWriter
                .InSequence(sequence)
                .Setup(rw => rw.WriteStart(this.Session.Object, this.Mode, Secret))
                .Returns(this.Environment);
        }

        protected void SetupSuccessfulConnectAsync(MockSequence sequence)
        {
            this.RequestWriter
                .InSequence(sequence)
                .Setup(rw => rw.WriteStartAsync(this.Session.Object, this.Mode, Secret))
                .Returns(Task.FromResult(this.Environment));
        }
    }
}
