using NSonic.Impl.Net;
using NSonic.Utils;
using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    sealed class ControlConnection : Connection, ISonicControlConnection
    {
        public ControlConnection(ISessionFactory sessionFactory
            , IRequestWriter requestWriter
            , IDisposableSonicClient client
            , Configuration configuration
            )
            : base(sessionFactory, requestWriter, client, configuration)
        {
            //
        }

        protected override ConnectionMode Mode => ConnectionMode.Control;

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
