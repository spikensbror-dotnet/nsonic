using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public abstract class SonicConnectionTestBase
    {
        protected const string Hostname = "testing";
        protected const int Port = 1337;
        protected const string Secret = "top_secret";

        internal Mock<ISonicSession> Session { get; private set; }
        internal ISonicSessionFactoryProvider SessionFactoryProvider { get; private set; }
        protected abstract string Mode { get; }

        [TestInitialize]
        public virtual void Initialize()
        {
            this.Session = new Mock<ISonicSession>(MockBehavior.Strict);
            this.Session.Setup(s => s.Dispose());

            this.SessionFactoryProvider = Mock.Of<ISonicSessionFactoryProvider>(ssfp => ssfp.Create(Hostname, Port).Create() == this.Session.Object);
        }

        protected void SetupSuccessfulConnect(MockSequence sequence)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns("CONNECTED <sonic-server v1.00>")
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("START", this.Mode, Secret))
                ;

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"STARTED {this.Mode} protocol(1) buffer(20000)")
                ;
        }
    }
}
