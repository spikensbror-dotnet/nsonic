using NSonic.Impl.Net;

namespace NSonic.Impl
{
    struct Configuration
    {
        public string Hostname { get; }
        public int Port { get; }
        public string Secret { get; }
        public ConnectionMode Mode { get; }

        public Configuration(string hostname
            , int port
            , string secret
            , ConnectionMode mode = ConnectionMode.None
            )
        {
            this.Hostname = hostname;
            this.Port = port;
            this.Secret = secret;
            this.Mode = mode;
        }

        public Configuration WithMode(ConnectionMode mode)
        {
            return new Configuration(this.Hostname, this.Port, this.Secret, mode);
        }
    }
}
