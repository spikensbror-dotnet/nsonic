using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class IngestConnectionAsyncTests : IngestConnectionTests
    {
        protected override bool Async => true;
    }
}
