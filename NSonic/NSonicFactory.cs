using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Impl.Net;

namespace NSonic
{
    /// <summary>
    /// Constructs and bootstraps Sonic connections.
    /// </summary>
    public static class NSonicFactory
    {
        private static readonly SessionFactory s_sessionFactory = new SessionFactory();
        private static readonly RequestWriter s_requestWriter = new RequestWriter();
        private static readonly ClientConnector s_connector = new ClientConnector(s_sessionFactory, s_requestWriter);

        /// <summary>
        /// Creates a control mode connection.
        /// </summary>
        /// <param name="hostname">The hostname where the Sonic server is located.</param>
        /// <param name="port">The port of the Sonic server.</param>
        /// <param name="secret">The secret of the Sonic server.</param>
        /// <returns>An unconnected control mode connection.</returns>
        public static ISonicControlConnection Control(string hostname
            , int port
            , string secret
            )
        {
            return new ControlConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
        }

        /// <summary>
        /// Creates a ingest mode connection.
        /// </summary>
        /// <param name="hostname">The hostname where the Sonic server is located.</param>
        /// <param name="port">The port of the Sonic server.</param>
        /// <param name="secret">The secret of the Sonic server.</param>
        /// <returns>An unconnected ingest mode connection.</returns>
        public static ISonicIngestConnection Ingest(string hostname
            , int port
            , string secret
            )
        {
            return new IngestConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
        }

        /// <summary>
        /// Creates a search mode connection.
        /// </summary>
        /// <param name="hostname">The hostname where the Sonic server is located.</param>
        /// <param name="port">The port of the Sonic server.</param>
        /// <param name="secret">The secret of the Sonic server.</param>
        /// <returns>An unconnected search mode connection.</returns>
        public static ISonicSearchConnection Search(string hostname
            , int port
            , string secret
            )
        {
            return new SearchConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
        }

        private static IDisposableSonicClient CreateTcpClient()
        {
            return new Client(s_connector, new TcpClientAdapter());
        }
    }
}
