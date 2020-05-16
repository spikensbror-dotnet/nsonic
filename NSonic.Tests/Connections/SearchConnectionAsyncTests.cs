using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SearchConnectionAsyncTests : SearchConnectionTests
    {
        protected override bool Async => true;
    }
}
