using NSonic.Utils;
using System;
using System.Text.RegularExpressions;

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

        public EnvironmentResponse WriteStart(ISonicSession session, string mode, string secret)
        {
            var response = session.Read();
            Assert.IsTrue(response.StartsWith("CONNECTED"), "Did not receive connection confirmation from the server");

            session.Write("START", mode, secret);

            response = session.Read();
            Assert.IsTrue(response.StartsWith("STARTED"), "Failed to start control session");

            var protocol = 0;
            var buffer = 0;

            response = response.Substring("STARTED ".Length);
            foreach (var split in response.Split(' '))
            {
                var regex = Regex.Match(split, "([a-z_]+)\\(([0-9]*)\\)");
                if (!regex.Success)
                {
                    continue;
                }

                if (regex.Groups[1].Value == "protocol")
                {
                    protocol = Convert.ToInt32(regex.Groups[2].Value);
                }
                else if (regex.Groups[1].Value == "buffer")
                {
                    buffer = Convert.ToInt32(regex.Groups[2].Value);
                }
            }

            return new EnvironmentResponse(protocol, buffer);
        }
    }
}
