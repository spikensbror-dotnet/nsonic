using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl.Connections;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicIngestConnectionTests : SonicConnectionTestBase
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

            this.SetupSuccessfulConnect(new MockSequence());
        }

        [TestMethod]
        public void ShouldBeAbleToConnect()
        {
            this.connection.Connect();
        }

        [TestMethod]
        public void CountShouldBeAbleToGetCountOfSpecifiedCollectionBucketAndObject()
        {
            // Arrange

            this.SetupWriteResult("32", "COUNT", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual(32, this.connection.Count("collection_name", "bucket_name", "obj_id"));
        }

        [TestMethod]
        public void FlushBucketShouldBeAbleToFlushSearchBucket()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHB", "collection_name", "bucket_name");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual(32, this.connection.FlushBucket("collection_name", "bucket_name"));
        }

        [TestMethod]
        public void FlushCollectionShouldBeAbleToFlushSearchCollection()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHC", "collection_name");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual(32, this.connection.FlushCollection("collection_name"));
        }

        [TestMethod]
        public void FlushObjectShouldBeAbleToFlushSearchObject()
        {
            // Arrange

            this.SetupWriteResult("32", "FLUSHO", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual(32, this.connection.FlushObject("collection_name", "bucket_name", "obj_id"));
        }

        [TestMethod]
        public void PopShouldBeAbleToPopSpecificTermFromSpecificObject()
        {
            // Arrange

            this.SetupWriteResult("32", "POP", "collection_name", "bucket_name", "obj_id", "\"term\"");

            // Act / Assert

            this.connection.Connect();
            Assert.AreEqual(32, this.connection.Pop("collection_name", "bucket_name", "obj_id", "term"));
        }

        [TestMethod]
        public void PushShouldBeAbleToPushWithoutLocale()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOk(this.Session.Object, "PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", ""))
                .Verifiable();

            // Act

            this.connection.Connect();
            this.connection.Push("collection_name", "bucket_name", "obj_id", "term");

            // Assert

            this.RequestWriter.Verify();
        }

        [TestMethod]
        public void PushShouldBeAbleToPushWithLocale()
        {
            // Arrange

            this.RequestWriter
                .Setup(rw => rw.WriteOk(this.Session.Object, "PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", "LANG(test)"))
                .Verifiable();

            // Act

            this.connection.Connect();
            this.connection.Push("collection_name", "bucket_name", "obj_id", "term", "test");

            // Assert

            this.RequestWriter.Verify();
        }

        private void SetupWriteResult(string response, params string[] args)
        {
            this.RequestWriter
                .Setup(rw => rw.WriteResult(this.Session.Object, args))
                .Returns(response);
        }
    }
}
