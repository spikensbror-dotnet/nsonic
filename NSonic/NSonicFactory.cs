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
        private static readonly SonicSessionFactory s_sessionFactory = new SonicSessionFactory();
        private static readonly SonicRequestWriter s_requestWriter = new SonicRequestWriter();
        private static readonly SonicClientConnector s_connector = new SonicClientConnector(s_sessionFactory, s_requestWriter);

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
            return new SonicControlConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
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
            return new SonicIngestConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
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
            return new SonicSearchConnection(s_sessionFactory, s_requestWriter, CreateTcpClient(), hostname, port, secret);
        }

        private static IDisposableSonicClient CreateTcpClient()
        {
            return new SonicClient(s_connector, new TcpClientAdapter());
        }
    }
}
