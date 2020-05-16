using NSonic.Impl.Net;
using System;
using System.Threading.Tasks;

namespace NSonic.Impl.Connections
{
    sealed class IngestConnection : Connection, ISonicIngestConnection
    {
        public IngestConnection(ISessionFactory sessionFactory
            , IRequestWriter requestWriter
            , IDisposableSonicClient client
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactory, requestWriter, client, hostname, port, secret)
        {
            //
        }

        protected override ConnectionMode Mode => ConnectionMode.Ingest;

        public int Count(string collection, string bucket = null, string @object = null)
        {
            using (var session = this.CreateSession())
            {
                var response = this.RequestWriter.WriteResult(session, "COUNT", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public async Task<int> CountAsync(string collection, string bucket = null, string @object = null)
        {
            using (var session = this.CreateSession())
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "COUNT", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public int FlushBucket(string collection, string bucket)
        {
            using (var session = this.CreateSession())
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHB", collection, bucket);

                return Convert.ToInt32(response);
            }
        }

        public async Task<int> FlushBucketAsync(string collection, string bucket)
        {
            using (var session = this.CreateSession())
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHB", collection, bucket);

                return Convert.ToInt32(response);
            }
        }

        public int FlushCollection(string collection)
        {
            using (var session = this.CreateSession())
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHC", collection);

                return Convert.ToInt32(response);
            }
        }

        public async Task<int> FlushCollectionAsync(string collection)
        {
            using (var session = this.CreateSession())
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHC", collection);

                return Convert.ToInt32(response);
            }
        }

        public int FlushObject(string collection, string bucket, string @object)
        {
            using (var session = this.CreateSession())
            {
                var response = this.RequestWriter.WriteResult(session, "FLUSHO", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public async Task<int> FlushObjectAsync(string collection, string bucket, string @object)
        {
            using (var session = this.CreateSession())
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "FLUSHO", collection, bucket, @object);

                return Convert.ToInt32(response);
            }
        }

        public int Pop(string collection, string bucket, string @object, string text)
        {
            using (var session = this.CreateSession())
            {
                var response = this.RequestWriter.WriteResult(session, "POP", collection, bucket, @object, $"\"{text}\"");

                return Convert.ToInt32(response);
            }
        }

        public async Task<int> PopAsync(string collection, string bucket, string @object, string text)
        {
            using (var session = this.CreateSession())
            {
                var response = await this.RequestWriter.WriteResultAsync(session, "POP", collection, bucket, @object, $"\"{text}\"");

                return Convert.ToInt32(response);
            }
        }

        public void Push(string collection, string bucket, string @object, string text, string locale = null)
        {
            using (var session = this.CreateSession())
            {
                var request = new PushRequest(text, locale);

                this.RequestWriter.WriteOk(session
                    , "PUSH"
                    , collection
                    , bucket
                    , @object
                    , request.Text
                    , request.Locale
                    );
            }
        }

        public async Task PushAsync(string collection, string bucket, string @object, string text, string locale = null)
        {
            using (var session = this.CreateSession())
            {
                var request = new PushRequest(text, locale);

                await this.RequestWriter.WriteOkAsync(session
                    , "PUSH"
                    , collection
                    , bucket
                    , @object
                    , request.Text
                    , request.Locale
                    );
            }
        }

        class PushRequest
        {
            private readonly string text;
            private readonly string locale;

            public PushRequest(string text, string locale)
            {
                this.text = text;
                this.locale = locale;
            }

            public string Text => $"\"{this.text}\"";
            public string Locale => !string.IsNullOrEmpty(this.locale) ? $"LANG({this.locale})" : "";
        }
    }
}
