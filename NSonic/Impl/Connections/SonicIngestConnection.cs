using System;
using System.Threading.Tasks;

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

        public async Task<int> CountAsync(string collection, string bucket = null, string @object = null)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "COUNT", collection, bucket, @object);

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

        public async Task<int> FlushBucketAsync(string collection, string bucket)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHB", collection, bucket);

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

        public async Task<int> FlushCollectionAsync(string collection)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHC", collection);

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

        public async Task<int> FlushObjectAsync(string collection, string bucket, string @object)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHO", collection, bucket, @object);

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

        public async Task<int> PopAsync(string collection, string bucket, string @object, string text)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "POP", collection, bucket, @object, $"\"{text}\"");

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

        public async Task PushAsync(string collection, string bucket, string @object, string text, string locale = null)
        {
            using (var session = this.SessionFactory.Create(this.Environment))
            {
                await this.RequestWriter.WriteOkAsync(session
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
