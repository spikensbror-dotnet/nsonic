using NSonic.Utils;

namespace NSonic.Impl.Connections
{
    sealed class SonicSearchConnection : SonicConnection, ISonicSearchConnection
    {
        public SonicSearchConnection(ISonicSessionFactoryProvider sessionFactoryProvider
            , ISonicRequestWriter requestWriter
            , string hostname
            , int port
            , string secret
            )
            : base(sessionFactoryProvider, requestWriter, hostname, port, secret)
        {
            //
        }

        protected override string Mode => "search";

        public string[] Query(string collection
            , string bucket
            , string terms
            , int? limit = null
            , int? offset = null
            , string locale = null
            )
        {
            using (var session = this.SessionFactory.Create())
            {
                session.Write("QUERY"
                    , collection
                    , bucket
                    , $"\"{terms}\""
                    , limit.HasValue ? $"LIMIT({limit})" : ""
                    , offset.HasValue ? $"OFFSET({offset})" : ""
                    , !string.IsNullOrEmpty(locale) ? $"LOCALE({locale})" : ""
                    );

                var response = session.Read();
                Assert.IsTrue(response.StartsWith("PENDING"), "Expected pending marker");

                var marker = response.Substring("PENDING ".Length);

                response = session.Read();
                Assert.IsTrue(response.StartsWith($"EVENT QUERY {marker}"), "Expected query result");

                return response
                    .Substring($"EVENT QUERY {marker} ".Length)
                    .Split(' ');
            }
        }

        public string[] Suggest(string collection, string bucket, string word, int? limit = null)
        {
            using (var session = this.SessionFactory.Create())
            {
                session.Write("SUGGEST"
                    , collection
                    , bucket
                    , $"\"{word}\""
                    , limit.HasValue ? $"LIMIT({limit})" : ""
                    );

                var response = session.Read();
                Assert.IsTrue(response.StartsWith("PENDING"), "Expected pending marker");

                var marker = response.Substring("PENDING ".Length);

                response = session.Read();
                Assert.IsTrue(response.StartsWith($"EVENT SUGGEST {marker}"), "Expected suggest result");

                return response
                    .Substring($"EVENT SUGGEST {marker} ".Length)
                    .Split(' ');
            }
        }
    }
}
