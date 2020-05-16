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
            return new SonicControlConnection(new SonicSessionFactory(), new SonicRequestWriter(), new TcpClientAdapter(), hostname, port, secret);
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
            return new SonicIngestConnection(new SonicSessionFactory(), new SonicRequestWriter(), new TcpClientAdapter(), hostname, port, secret);
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
            return new SonicSearchConnection(new SonicSessionFactory(), new SonicRequestWriter(), new TcpClientAdapter(), hostname, port, secret);
        }
    }
}
