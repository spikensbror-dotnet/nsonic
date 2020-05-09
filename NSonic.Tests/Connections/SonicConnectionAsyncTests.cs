using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class SonicConnectionAsyncTests : SonicConnectionTests
    {
        protected override bool Async => true;
    }
}
