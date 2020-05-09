using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSonic.Impl;
using System;

namespace NSonic.Tests
{
    [TestClass]
    public class StartResponseParserTests
    {
        [TestMethod]
        public void Parse_ShouldCreateEnvironmentResponseFromResponse()
        {
            // Act

            var result = StartResponseParser.Parse("STARTED TEST protocol(4) buffer(25000)");

            // Assert

            Assert.AreEqual(4, result.Protocol);
            Assert.AreEqual(25000, result.Buffer);
            Assert.AreEqual((int)Math.Floor(25000 * 0.5 / 4), result.MaxBufferStringLength);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertionException))]
        public void Parse_ShouldThrowAssertionExceptionIfResponseIsInvalid()
        {
            // Act

            StartResponseParser.Parse("NOT_STARTED");
        }
    }
}
