using NSonic.Utils;

namespace NSonic.Impl.Connections
{
    sealed class SonicControlConnection : SonicConnection, ISonicControlConnection
    {
        public SonicControlConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , ISonicRequestWriter requestWriter
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactoryProvider, requestWriter, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "control";

        public string Info()
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                return this.RequestWriter.WriteResult(session, "INFO");
            }
        }

        public void Trigger(string action, string data = null)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                this.RequestWriter.WriteOk(session, "TRIGGER", action, data);
            }
        }
    }
}
