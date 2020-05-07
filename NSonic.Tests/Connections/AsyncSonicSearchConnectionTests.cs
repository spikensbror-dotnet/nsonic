using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class AsyncSonicSearchConnectionTests : SonicConnectionTestBase
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

            this.SetupSuccessfulConnectAsync(new MockSequence());
        }

        [TestMethod]
        public async Task ShouldBeAbleToConnect()
        {
            await this.connection.ConnectAsync();
        }

        [TestMethod]
        public async Task QueryShouldPerformSearchQueryWithNoOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", Marker);

            // Act

            await this.connection.ConnectAsync();
            var results = await this.connection.QueryAsync(Collection, Bucket, Terms);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        public async Task QueryShouldPerformSearchQueryWithOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            var limit = 42;
            var offset = 32;
            var locale = "tst";

            this.Session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("QUERY"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , $"LIMIT({limit})"
                        , $"OFFSET({offset})"
                        , $"LANG({locale})"
                    )
                )
                .Returns(Task.CompletedTask)
                ;

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", Marker);

            // Act

            await this.connection.ConnectAsync();
            var results = await this.connection.QueryAsync(Collection, Bucket, Terms, limit, offset, locale);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task QueryShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult($"NOT_PENDING {Marker}"))
                ;

            // Act

            await this.connection.ConnectAsync();
            await this.connection.QueryAsync(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task QueryShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "INVALID", Marker);

            // Act

            await this.connection.ConnectAsync();
            await this.connection.QueryAsync(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task QueryShouldThrowAssertionExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalQueryWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "QUERY", "INVALID");

            // Act

            await this.connection.ConnectAsync();
            await this.connection.QueryAsync(Collection, Bucket, Terms);
        }

        [TestMethod]
        public async Task SuggestShouldPerformSuggestionsWithNoOptionals()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", Marker);

            // Act

            await this.connection.ConnectAsync();
            var results = await this.connection.SuggestAsync(Collection, Bucket, Terms);

            // Assert

            VerifyResults(results);
        }

        [TestMethod]
        public async Task SuggestShouldPerformSuggestionsWithOptionals()
        {
            // Arrange

            var limit = 42;
            var sequence = new MockSequence();

            this.Session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("SUGGEST"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , $"LIMIT({limit})"
                    )
                )
                .Returns(Task.CompletedTask)
                ;

            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", Marker);

            // Act

            await this.connection.ConnectAsync();
            var results = await this.connection.SuggestAsync(Collection, Bucket, Terms, limit);

            // Act

            VerifyResults(results);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task SuggestShouldThrowAssertionExceptionIfPendingResponseIsInvalid()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);

            this.Session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult($"NOT_PENDING {Marker}"))
                ;

            // Act

            await this.connection.ConnectAsync();
            await this.connection.SuggestAsync(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task SuggestShouldThrowAssertionExceptionIfEventResponseIsOfInvalidType()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "INVALID", Marker);

            // Act

            await this.connection.ConnectAsync();
            await this.connection.SuggestAsync(Collection, Bucket, Terms);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public async Task SuggestShouldThrowAssertoinExceptionIfEventResponseIsForTheWrongMarker()
        {
            // Arrange

            var sequence = new MockSequence();

            this.SetupNormalSuggestWrite(sequence);
            this.SetupPendingMarker(sequence, Marker);
            this.SetupEventResponse(sequence, "SUGGEST", "INVALID");

            // Act

            await this.connection.ConnectAsync();
            await this.connection.SuggestAsync(Collection, Bucket, Terms);
        }

        private void SetupNormalQueryWrite(MockSequence sequence)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("QUERY"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , ""
                        , ""
                        , ""
                    )
                )
                .Returns(Task.CompletedTask)
                ;

        }

        private void SetupNormalSuggestWrite(MockSequence sequence)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.WriteAsync("SUGGEST"
                        , Collection
                        , Bucket
                        , $"\"{Terms}\""
                        , ""
                    )
                )
                .Returns(Task.CompletedTask)
                ;
        }

        private void SetupPendingMarker(MockSequence sequence, string marker)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult($"PENDING {marker}"))
                ;
        }

        private void SetupEventResponse(MockSequence sequence, string searchType, string marker)
        {
            this.Session
                .InSequence(sequence)
                .Setup(s => s.ReadAsync())
                .Returns(Task.FromResult($"EVENT {searchType} {marker} result1 result2"))
                ;
        }

        private static void VerifyResults(string[] results)
        {
            CollectionAssert.AreEqual(new[] { "result1", "result2" }, results);
        }
    }
}
