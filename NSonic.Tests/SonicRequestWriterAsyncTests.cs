using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests
{
    [TestClass]
    public class SonicRequestWriterAsyncTests : SonicRequestWriterTests
    {
        protected override bool Async => true;
    }
}
