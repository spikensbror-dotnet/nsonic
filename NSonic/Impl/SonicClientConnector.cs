using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class SonicClientConnector : ISonicClientConnector
    {
        private readonly ISonicSessionFactory sessionFactory;
        private readonly ISonicStartRequestWriter startRequestWriter;

        public SonicClientConnector(ISonicSessionFactory sessionFactory
            , ISonicStartRequestWriter startRequestWriter
            )
        {
            this.sessionFactory = sessionFactory;
            this.startRequestWriter = startRequestWriter;
        }

        public EnvironmentResponse Connect(ISonicClient client, ITcpClient tcpClient, Configuration configuration)
        {
            tcpClient.Connect(configuration.Hostname, configuration.Port);

            using (var session = this.sessionFactory.Create(client))
            {
                return this.startRequestWriter.WriteStart(session, configuration.Mode, configuration.Secret);
            }
        }

        public async Task<EnvironmentResponse> ConnectAsync(ISonicClient client, ITcpClient tcpClient, Configuration configuration)
        {
            await tcpClient.ConnectAsync(configuration.Hostname, configuration.Port);

            using (var session = this.sessionFactory.Create(client))
            {
                return await this.startRequestWriter.WriteStartAsync(session, configuration.Mode, configuration.Secret);
            }
        }

        public void Disconnect(ISonicClient client)
        {
            using (var session = this.sessionFactory.Create(client))
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
    }
}
