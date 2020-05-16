using NSonic.Impl.Net;
using NSonic.Utils;
using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    sealed class SonicControlConnection : SonicConnection, ISonicControlConnection
    {
        public SonicControlConnection(ISonicSessionFactory sessionFactory
            , ISonicRequestWriter requestWriter
            , IDisposableTcpClient tcpClient
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactory, requestWriter, tcpClient, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "control";

        public string Info()
        {
            using (var session = this.CreateSession())
            {
                return this.RequestWriter.WriteResult(session, "INFO");
            }
        }

        public async Task<string> InfoAsync()
        {
            using (var session = this.CreateSession())
            {
                return await this.RequestWriter.WriteResultAsync(session, "INFO");
            }
        }

        public void Trigger(string action, string data = null)
        {
            using (var session = this.CreateSession())
            {
                this.RequestWriter.WriteOk(session, "TRIGGER", action, data);
            }
        }

        public async Task TriggerAsync(string action, string data = null)
        {
            using (var session = this.CreateSession())
            {
                await this.RequestWriter.WriteOkAsync(session, "TRIGGER", action, data);
            }
        }
    }
}
