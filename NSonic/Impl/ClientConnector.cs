using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class ClientConnector : IClientConnector
    {
        private readonly ISessionFactory sessionFactory;
        private readonly IStartRequestWriter startRequestWriter;

        public ClientConnector(ISessionFactory sessionFactory
            , IStartRequestWriter startRequestWriter
            )
        {
            this.sessionFactory = sessionFactory;
            this.startRequestWriter = startRequestWriter;
        }

        public EnvironmentResponse Connect(IClient client, ITcpClient tcpClient, Configuration configuration)
        {
            tcpClient.Connect(configuration.Hostname, configuration.Port);

            using (var session = this.sessionFactory.Create(client))
            {
                return this.startRequestWriter.WriteStart(session, configuration.Mode, configuration.Secret);
            }
        }

        public async Task<EnvironmentResponse> ConnectAsync(IClient client, ITcpClient tcpClient, Configuration configuration)
        {
            await tcpClient.ConnectAsync(configuration.Hostname, configuration.Port);

            using (var session = this.sessionFactory.Create(client))
            {
                return await this.startRequestWriter.WriteStartAsync(session, configuration.Mode, configuration.Secret);
            }
        }

        public void Disconnect(IClient client)
        {
            using (var session = this.sessionFactory.Create(client))
            {
                try
                {
                    session.Write("QUIT");
                    var response = session.Read();
                    Assert.IsTrue(response.StartsWith("ENDED"), "Quit failed when disposing sonic connection", response);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }
        }
    }
}
