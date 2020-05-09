using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicIngestConnectionAsyncTests : SonicIngestConnectionTests
    {
        protected override bool Async => true;
    }
}
