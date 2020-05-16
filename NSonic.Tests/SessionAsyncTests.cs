using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests
{
    [TestClass]
    public class SessionAsyncTests : SessionTests
    {
        protected override bool Async => true;
    }
}
