using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SearchConnectionTests : TestBase
    {
        private const string Marker = "TSTMRKR";
        private const string Collection = "TSTCOLL";
        private const string Bucket = "TSTBCKT";
        private const string Terms = "result";

        internal override ConnectionMode Mode => ConnectionMode.Search;
        protected override bool Async => false;

        private SearchConnection connection;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new SearchConnection(this.SessionFactory
                , new RequestWriter()
                , this.Client
                , StubConstants.Hostname
                , StubConstants.Port
                , StubConstants.Secret
                );
        }

        [TestMethod]
        public async Task Query_ShouldPerformSearchQueryWithNoOptionals()
        {
            // Arrange

            this.SetupNormalQueryWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("QUERY", Marker);

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                VerifyResults(await this.connection.QueryAsync(Collection, Bucket, Terms));
            }
            else
            {
                this.connection.Connect();

                VerifyResults(this.connection.Query(Collection, Bucket, Terms));
            }
        }

        [TestMethod]
        public async Task Query_ShouldPerformSearchQueryWithOptionals()
        {
            // Arrange

            var limit = 42;
            var offset = 32;
            var locale = "tst";

            this.Session
                .SetupWrite(this.Sequence
                    , this.Async
                    , "QUERY"
                    , Collection
                    , Bucket
                    , $"\"{Terms}\""
                    , $"LIMIT({limit})"
                    , $"OFFSET({offset})"
                    , $"LANG({locale})"
                )
                ;

            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("QUERY", Marker);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                VerifyResults(await this.connection.QueryAsync(Collection, Bucket, Terms, limit, offset, locale));
            }
            else
            {
                this.connection.Connect();

                VerifyResults(this.connection.Query(Collection, Bucket, Terms, limit, offset, locale));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Query_ShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            this.SetupNormalQueryWrite();
            this.Session.SetupRead(this.Sequence, this.Async, $"NOT_PENDING {Marker}");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.QueryAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();


                this.connection.Query(Collection, Bucket, Terms);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Query_ShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            this.SetupNormalQueryWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("INVALID", Marker);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.QueryAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();

                this.connection.Query(Collection, Bucket, Terms);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Query_ShouldThrowAssertionExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            this.SetupNormalQueryWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("QUERY", "INVALID");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.QueryAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();

                this.connection.Query(Collection, Bucket, Terms);
            }
        }

        [TestMethod]
        public async Task Suggest_ShouldPerformSuggestionsWithNoOptionals()
        {
            // Arrange

            this.SetupNormalSuggestWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("SUGGEST", Marker);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                VerifyResults(await this.connection.SuggestAsync(Collection, Bucket, Terms));
            }
            else
            {
                this.connection.Connect();

                VerifyResults(this.connection.Suggest(Collection, Bucket, Terms));
            }
        }

        [TestMethod]
        public async Task Suggest_ShouldPerformSuggestionsWithOptionals()
        {
            // Arrange

            var limit = 42;

            this.Session
                .SetupWrite(this.Sequence
                    , this.Async
                    , "SUGGEST"
                    , Collection
                    , Bucket
                    , $"\"{Terms}\""
                    , $"LIMIT({limit})"
                )
                ;

            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("SUGGEST", Marker);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                VerifyResults(await this.connection.SuggestAsync(Collection, Bucket, Terms, limit));
            }
            else
            {
                this.connection.Connect();

                VerifyResults(this.connection.Suggest(Collection, Bucket, Terms, limit));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Suggest_ShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            this.SetupNormalSuggestWrite();
            this.Session.SetupRead(this.Sequence, this.Async, $"NOT_PENDING {Marker}");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.SuggestAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();

                this.connection.Suggest(Collection, Bucket, Terms);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Suggest_ShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            this.SetupNormalSuggestWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("INVALID", Marker);

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.SuggestAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();

                this.connection.Suggest(Collection, Bucket, Terms);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task Suggest_ShouldThrowAssertoinExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            this.SetupNormalSuggestWrite();
            this.SetupPendingMarker(Marker);
            this.SetupEventResponse("SUGGEST", "INVALID");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.SuggestAsync(Collection, Bucket, Terms);
            }
            else
            {
                this.connection.Connect();

                this.connection.Suggest(Collection, Bucket, Terms);
            }
        }

        private void SetupNormalQueryWrite()
        {
            this.Session
                .SetupWrite(this.Sequence
                    , this.Async
                    , "QUERY"
                    , Collection
                    , Bucket
                    , $"\"{Terms}\""
                    , ""
                    , ""
                    , ""
                )
                ;

        }

        private void SetupNormalSuggestWrite()
        {
            this.Session
                .SetupWrite(this.Sequence
                    , this.Async
                    , "SUGGEST"
                    , Collection
                    , Bucket
                    , $"\"{Terms}\""
                    , ""
                )
                ;
        }

        private void SetupPendingMarker(string marker)
        {
            this.Session
                .SetupRead(this.Sequence, this.Async, $"PENDING {marker}")
                ;
        }

        private void SetupEventResponse(string searchType, string marker)
        {
            this.Session
                .SetupRead(this.Sequence, this.Async, $"EVENT {searchType} {marker} result1 result2")
                ;
        }

        private static void VerifyResults(string[] results)
        {
            CollectionAssert.AreEqual(new[] { "result1", "result2" }, results);
        }
    }
}
