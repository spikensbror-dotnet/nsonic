using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicSearchConnectionTests : SonicConnectionTestBase
    {
        private const string Marker = "TSTMRKR";
        private const string Collection = "TSTCOLL";
        private const string Bucket = "TSTBCKT";
        private const string Terms = "result";

        protected override string Mode => "search";

        private SonicSearchConnection connection;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new SonicSearchConnection(this.SessionFactoryProvider
                , this.RequestWriter.Object
                , Hostname
                , Port
                , Secret
                );

            this.SetupSuccessfulConnect(new MockSequence());
        }

        [TestMethod]
        public void ShouldBeAbleToConnect()
        {
            this.connection.Connect();
        }

        [TestMethod]
        public void QueryShouldPerformSearchQueryWithNoOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", Marker);

            // Act

            this.connection.Connect();
            var results = this.connection.Query(Collection, Bucket, Terms);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        public void QueryShouldPerformSearchQueryWithOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            var limit = 42;
            var offset = 32;
            var locale = "tst";

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("QUERY"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , $"LIMIT({limit})"
                        , $"OFFSET({offset})"
                        , $"LANG({locale})"
                    )
                )
                ;

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", Marker);

            // Act

            this.connection.Connect();
            var results = this.connection.Query(Collection, Bucket, Terms, limit, offset, locale);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void QueryShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"NOT_PENDING {Marker}")
                ;

            // Act

            this.connection.Connect();
            this.connection.Query(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void QueryShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "INVALID", Marker);

            // Act

            this.connection.Connect();
            this.connection.Query(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void QueryShouldThrowAssertionExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", "INVALID");

            // Act

            this.connection.Connect();
            this.connection.Query(Collection, Bucket, Terms);
        }

        [TestMethod]
        public void SuggestShouldPerformSuggestionsWithNoOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", Marker);

            // Act

            this.connection.Connect();
            var results = this.connection.Suggest(Collection, Bucket, Terms);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        public void SuggestShouldPerformSuggestionsWithOptionals()
        {
            // Arrange

            var limit = 42;
            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("SUGGEST"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , $"LIMIT({limit})"
                    )
                )
                ;

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", Marker);

            // Act

            this.connection.Connect();
            var results = this.connection.Suggest(Collection, Bucket, Terms, limit);

            // Act

            VerifyResults(results);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void SuggestShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"NOT_PENDING {Marker}")
                ;

            // Act

            this.connection.Connect();
            this.connection.Suggest(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void SuggestShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "INVALID", Marker);

            // Act

            this.connection.Connect();
            this.connection.Suggest(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void SuggestShouldThrowAssertoinExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", "INVALID");

            // Act

            this.connection.Connect();
            this.connection.Suggest(Collection, Bucket, Terms);
        }

        private void SetupNormalQueryWrite(MockSequence sequence)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("QUERY"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , ""
                        , ""
                        , ""
                    )
                )
                ;

        }

        private void SetupNormalSuggestWrite(MockSequence sequence)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.Write("SUGGEST"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , ""
                    )
                )
                ;
        }

        private void SetupPendingMarker(MockSequence sequence, string marker)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"PENDING {marker}")
                ;
        }

        private void SetupEventResponse(MockSequence sequence, string searchType, string marker)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.Read())
                .Returns($"EVENT {searchType} {marker} result1 result2")
                ;
        }

        private static void VerifyResults(string[] results)
        {
            CollectionAssert.AreEqual(new[] { "result1", "result2" }, results);
        }
    }
}
