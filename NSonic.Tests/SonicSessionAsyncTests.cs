using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicSessionAsyncTests : SonicSessionTests
    {
        protected override bool Async => true;
    }
}
