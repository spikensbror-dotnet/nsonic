using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class IngestConnectionTests : TestBase
    {
        private IngestConnection connection;

        internal override ConnectionMode Mode => ConnectionMode.Ingest;
        protected override bool Async => false;

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            this.connection = new IngestConnection(this.SessionFactory
                , new RequestWriter()
                , this.Client
                , StubConstants.Configuration
                );
        }

        [TestMethod]
        public async Task Count_ShouldBeAbleToGetCountOfSpecifiedCollectionBucketAndObject()
        {
            // Arrange

            this.SetupWriteWithResult("32", "COUNT", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(32, await this.connection.CountAsync("collection_name", "bucket_name", "obj_id"));
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(32, this.connection.Count("collection_name", "bucket_name", "obj_id"));
            }
        }

        [TestMethod]
        public async Task FlushBucket_ShouldBeAbleToFlushSearchBucket()
        {
            // Arrange

            this.SetupWriteWithResult("32", "FLUSHB", "collection_name", "bucket_name");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(32, await this.connection.FlushBucketAsync("collection_name", "bucket_name"));
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(32, this.connection.FlushBucket("collection_name", "bucket_name"));
            }
        }

        [TestMethod]
        public async Task FlushCollection_ShouldBeAbleToFlushSearchCollection()
        {
            // Arrange

            this.SetupWriteWithResult("32", "FLUSHC", "collection_name");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(32, await this.connection.FlushCollectionAsync("collection_name"));
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(32, this.connection.FlushCollection("collection_name"));
            }
        }

        [TestMethod]
        public async Task FlushObject_ShouldBeAbleToFlushSearchObject()
        {
            // Arrange

            this.SetupWriteWithResult("32", "FLUSHO", "collection_name", "bucket_name", "obj_id");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(32, await this.connection.FlushObjectAsync("collection_name", "bucket_name", "obj_id"));
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(32, this.connection.FlushObject("collection_name", "bucket_name", "obj_id"));
            }
        }

        [TestMethod]
        public async Task Pop_ShouldBeAbleToPopSpecificTermFromSpecificObject()
        {
            // Arrange

            this.SetupWriteWithResult("32", "POP", "collection_name", "bucket_name", "obj_id", "\"term\"");

            // Act / Assert

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                Assert.AreEqual(32, await this.connection.PopAsync("collection_name", "bucket_name", "obj_id", "term"));
            }
            else
            {
                this.connection.Connect();

                Assert.AreEqual(32, this.connection.Pop("collection_name", "bucket_name", "obj_id", "term"));
            }
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPushWithoutLocale()
        {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", "");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term");
            }
            else
            {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term");
            }

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPushWithLocale()
        {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\"", "LANG(test)");

            // Act

            if (this.Async)
            {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term", "test");
            }
            else
            {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term", "test");
            }

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPreventInjectionAttacksWithLocale_1() {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\\\"\"", "LANG(test)");

            // Act

            if (this.Async) {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term\"", "test");
            } else {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term\"", "test");
            }

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPreventInjectionAttacksWithLocale_2() {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\\n\"", "LANG(test)");

            // Act

            if (this.Async) {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term\n", "test");
            } else {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term\n", "test");
            }

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPreventInjectionAttacksWithoutLocale_1() {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\\\"\"", "");

            // Act

            if (this.Async) {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term\"");
            } else {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term\"");
            }

            // Assert

            this.VerifyAll();
        }

        [TestMethod]
        public async Task Push_ShouldBeAbleToPreventInjectionAttacksWithoutLocale_2() {
            // Arrange

            this.SetupWriteWithOk("PUSH", "collection_name", "bucket_name", "obj_id", "\"term\\n\"", "");

            // Act

            if (this.Async) {
                await this.connection.ConnectAsync();

                await this.connection.PushAsync("collection_name", "bucket_name", "obj_id", "term\n");
            } else {
                this.connection.Connect();

                this.connection.Push("collection_name", "bucket_name", "obj_id", "term\n");
            }

            // Assert

            this.VerifyAll();
        }
    }
}
