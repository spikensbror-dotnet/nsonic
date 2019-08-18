using NSonic.Utils;

namespace NSonic.Impl.Connections
{
    sealed class SonicControlConnection : SonicConnection, ISonicControlConnection
    {
        public SonicControlConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactoryProvider, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "control";

        public string Info()
        {
            using (var session = this.SessionFactory.Create())
            {
                session.Write("INFO");

                var response = session.Read();
                Assert.IsTrue(response.StartsWith("RESULT"), "Info returned invalid data");

                return response;
            }
        }

        public void Ping()
        {
            using (var session = this.SessionFactory.Create())
            {
                session.Write("PING");

                var response = session.Read();
                Assert.IsTrue(response.StartsWith("PONG"), "Ping failed");
            }
        }

        public void Trigger(string action, string data)
        {
            using (var session = this.SessionFactory.Create())
            {
                session.Write("TRIGGER", action, data);

                var response = session.Read();
                Assert.IsTrue(response.StartsWith("OK"), "Received unexpected response on trigger");
            }
        }
    }
}
