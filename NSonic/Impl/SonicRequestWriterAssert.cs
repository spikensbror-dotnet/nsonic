using NSonic.Utils;

namespace NSonic.Impl
{
    static class SonicRequestWriterAssert
    {
        public static void Ok(string response)
        {
            Assert.IsTrue(response.StartsWith("OK"), "Expected OK response");
        }

        public static void Result(string response)
        {
            Assert.IsTrue(response.StartsWith("RESULT "), "Expected RESULT response");
        }

        public static void Connected(string response)
        {
            Assert.IsTrue(response.StartsWith("CONNECTED"), "Did not receive connection confirmation from the server");
        }
    }
}
