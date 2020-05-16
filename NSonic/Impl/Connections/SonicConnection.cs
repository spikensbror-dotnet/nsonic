using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    abstract class SonicConnection : ISonicConnection
    {
        private readonly ISonicSessionFactory sessionFactory;
        private readonly IDisposableTcpClient tcpClient;
        private readonly string hostname;
        private readonly int port;
        private readonly string secret;

        internal EnvironmentResponse environment = EnvironmentResponse.Default;

        protected SonicConnection(ISonicSessionFactory sessionFactory
            , ISonicRequestWriter requestWriter
            , IDisposableTcpClient tcpClient
            , string hostname
            , int port
            , string secret
            )
        {
            this.sessionFactory = sessionFactory;
            this.RequestWriter = requestWriter;
            this.tcpClient = tcpClient;
            this.hostname = hostname;
            this.port = port;
            this.secret = secret;
        }

        protected abstract string Mode { get; }

        protected ISonicRequestWriter RequestWriter { get; }

        public void Connect()
        {
            this.tcpClient.Connect(this.hostname, this.port);

            using (var session = this.CreateSession())
            {
                this.environment = this.RequestWriter.WriteStart(session, this.Mode, this.secret);
            }
        }

        public async Task ConnectAsync()
        {
            await this.tcpClient.ConnectAsync(this.hostname, this.port);

            using (var session = this.CreateSession())
            {
                this.environment = await this.RequestWriter.WriteStartAsync(session, this.Mode, this.secret);
            }
        }

        public void Dispose()
        {
            if (this.tcpClient.Connected)
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
            }

            this.tcpClient.Dispose();
        }

        protected ISonicSession CreateSession()
        {
            return this.sessionFactory.Create(this.tcpClient, this.environment);
        }
    }
}
