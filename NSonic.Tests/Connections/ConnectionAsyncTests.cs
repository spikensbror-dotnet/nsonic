using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class ConnectionAsyncTests : ConnectionTests
    {
        protected override bool Async => true;
    }
}
