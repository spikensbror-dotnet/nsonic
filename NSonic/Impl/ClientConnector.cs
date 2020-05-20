using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class ClientConnector : IClientConnector
    {
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        private readonly ISessionFactory sessionFactory;
        private readonly IStartRequestWriter startRequestWriter;

        public ClientConnector(INonLockingSessionFactory sessionFactory
            , IStartRequestWriter startRequestWriter
            )
        {
            this.sessionFactory = sessionFactory;
            this.startRequestWriter = startRequestWriter;
        }

        public EnvironmentResponse Connect(IClient client, ITcpClient tcpClient, Configuration configuration)
        {
            this.semaphore.Wait();

            try
            {
                tcpClient.Connect(configuration.Hostname, configuration.Port);

                using (var session = this.sessionFactory.Create(client))
                {
                    return this.startRequestWriter.WriteStart(session, configuration.Mode, configuration.Secret);
                }
            }
            finally
            {
                this.semaphore.Release();
            }
        }

        public async Task<EnvironmentResponse> ConnectAsync(IClient client, ITcpClient tcpClient, Configuration configuration)
        {
            await this.semaphore.WaitAsync();

            try
            {
                await tcpClient.ConnectAsync(configuration.Hostname, configuration.Port);

                using (var session = this.sessionFactory.Create(client))
                {
                    return await this.startRequestWriter.WriteStartAsync(session, configuration.Mode, configuration.Secret);
                }
            }
            finally
            {
                this.semaphore.Release();
            }
        }

        public void Disconnect(IClient client)
        {
            this.semaphore.Wait();

            try
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
            finally
            {
                this.semaphore.Release();
            }
        }
    }
}
