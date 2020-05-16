using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NSonic.Impl;
using NSonic.Impl.Connections;
using NSonic.Tests.Stubs;
using System.Threading.Tasks;

namespace NSonic.Tests.Connections
{
    [TestClass]
    public class ControlConnectionAsyncTests : ControlConnectionTests
    {
        protected override bool Async => true;
    }
}
