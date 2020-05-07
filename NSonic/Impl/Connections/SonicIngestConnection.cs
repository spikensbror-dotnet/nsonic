using System;

namespace NSonic.Impl.Connections
{
    sealed class SonicIngestConnection : SonicConnection, ISonicIngestConnection
    {
        public SonicIngestConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , ISonicRequestWriter requestWriter
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactoryProvider, requestWriter, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "ingest";

        public int Count(string collection, string bucket = null, string @object = null)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = this.RequestWriter.WriteResult(session, "COUNT", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public int FlushBucket(string collection, string bucket)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHB", collection, bucket);

                return Convert.ToInt32(response);
            }
        }

        public int FlushCollection(string collection)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHC", collection);

                return Convert.ToInt32(response);
            }
        }

        public int FlushObject(string collection, string bucket, string @object)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHO", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public int Pop(string collection, string bucket, string @object, string text)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = this.RequestWriter.WriteResult(session, "POP", collection, bucket, @object, $"\"{text}\"");

                return Convert.ToInt32(response);
            }
        }

        public void Push(string collection, string bucket, string @object, string text, string locale = null)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                this.RequestWriter.WriteOk(session
                    , "PUSH"
                    , collection
                    , bucket
                    , @object
                    , $"\"{text}\""
                    , !string.IsNullOrEmpty(locale) ? $"LANG({locale})" : ""
                    );
            }
        }
    }
}
