using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicSearchConnectionAsyncTests : SonicSearchConnectionTests
    {
        protected override bool Async => true;
    }
}
