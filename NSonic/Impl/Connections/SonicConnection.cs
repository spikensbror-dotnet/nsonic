using NSonic.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    abstract class SonicConnection : ISonicConnection
    {
        private readonly ISonicSessionFactoryProvider sessionFactoryProvider;
        private readonly string hostname;
        private readonly int port;
        private readonly string secret;

        protected SonicConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , ISonicRequestWriter requestWriter
            , string hostname
            , int port
            , string secret
            )
        {
            this.sessionFactoryProvider = sessionFactoryProvider;
            this.RequestWriter = requestWriter;
            this.hostname = hostname;
            this.port = port;
            this.secret = secret;
        }

        protected abstract string Mode { get; }

        protected ISonicRequestWriter RequestWriter { get; }
        protected ISonicSessionFactory SessionFactory { get; private set; }

        public EnvironmentResponse Environment { get; private set; } = EnvironmentResponse.Default;

        public void Connect()
        {
            this.CreateSessionFactory();

            using (var session = this.CreateSession())
            {
                this.Environment = this.RequestWriter.WriteStart(session, this.Mode, this.secret);
            }
        }

        public async Task ConnectAsync()
        {
            this.CreateSessionFactory();

            using (var session = this.CreateSession())
            {
                this.Environment = await this.RequestWriter.WriteStartAsync(session, this.Mode, this.secret);
            }
        }

        public void Dispose()
        {
            if (this.SessionFactory != null)
            {
                using (var session = this.CreateSession())
                {
                    try
                    {
                        session.Write("QUIT");
                        Assert.IsTrue(session.Read().StartsWith("ENDED"), "Quit failed when disposing sonic connection");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }

                this.SessionFactory.Dispose();
            }
        }

        protected ISonicSession CreateSession()
        {
            return this.SessionFactory.Create(this.Environment);
        }

        private void CreateSessionFactory()
        {
            // The reason for this is that the TCP client that is created along with the session
            // factory will be instantly connected. Otherwise, we would've passed it in the
            // constructor.
            this.SessionFactory = this.sessionFactoryProvider.Create(this.hostname, this.port);
        }
    }
}
