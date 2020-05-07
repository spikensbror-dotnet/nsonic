using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class AsyncSonicIngestConnectionTests : SonicConnectionTestBase
    {
        protected override string Mode => "ingest";

        private SonicIngestConnection connection;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new SonicIngestConnection(this.SessionFactoryProvider
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
        public async Task CountShouldBeAbleToGetCountOfSpecifiedCollectionBucketAndObject()
        {
            // Arrange

            this.SetupWriteResult("32", "COUNT", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual(32, await this.connection.CountAsync("collection_name", "bucket_name", "obj_id"));
        }

        [TestMethod]
        public async Task FlushBucketShouldBeAbleToFlushSearchBucket()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHB", "collection_name", "bucket_name");

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual(32, await this.connection.FlushBucketAsync("collection_name", "bucket_name"));
        }

        [TestMethod]
        public async Task FlushCollectionShouldBeAbleToFlushSearchCollection()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHC", "collection_name");

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual(32, await this.connection.FlushCollectionAsync("collection_name"));
        }

        [TestMethod]
        public async Task FlushObjectShouldBeAbleToFlushSearchObject()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHO", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual(32, await this.connection.FlushObjectAsync("collection_name", "bucket_name", "obj_id"));
        }

        [TestMethod]
        public async Task PopShouldBeAbleToPopSpecificTermFromSpecificObject()
        {
            // Arrange

            this.SetupWriteResult("32", "POP", "collection_name", "bucket_name", "obj_id", "\"term\"");

            // Act / Assert

            await this.connection.ConnectAsync();
            Assert.AreEqual(32, await this.connection.PopAsync("collection_name", "bucket_name", "obj_id", "term"));
        }

        [TestMethod]
        public async Task PushShouldBeAbleToPushWithoutLocale()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOkAsync(this.Session.Object, "PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", ""))
                .Returns(Task.CompletedTask);

            // Act

            await this.connection.ConnectAsync();
            await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term");

            // Assert

            this.RequestWriter.VerifyAll();
        }

        [TestMethod]
        public async Task PushShouldBeAbleToPushWithLocale()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOkAsync(this.Session.Object, "PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", "LANG(test)"))
                .Returns(Task.CompletedTask);

            // Act

            await this.connection.ConnectAsync();
            await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term", "test");

            // Assert

            this.RequestWriter.VerifyAll();
        }

        private void SetupWriteResult(string response, params string[] args)
        {
            this.RequestWriter
                .Setup(rw => rw.WriteResultAsync(this.Session.Object, args))
                .Returns(Task.FromResult(response));
        }
    }
}
