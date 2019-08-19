using NSonic.Utils;

namespace NSonic.Impl
{
    class SonicRequestWriter : ISonicRequestWriter
    {
        public void WriteOk(ISonicSession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            Assert.IsTrue(response.StartsWith("OK"), "Expected OK response");
        }

        public string WriteResult(ISonicSession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            Assert.IsTrue(response.StartsWith("RESULT "), "Expected RESULT response");

            return response.Substring("RESULT ".Length);
        }
    }
}
