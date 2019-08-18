namespace NSonic.Impl.Connections
{
    sealed class SonicIngestConnection : SonicConnection, ISonicIngestConnection
    {
        public SonicIngestConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactoryProvider, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "ingest";
    }
}
