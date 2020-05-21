using NSonic.Utils;
using System;
using System.Text.RegularExpressions;

namespace NSonic.Impl
{
    static class StartResponseParser
    {
        public static EnvironmentResponse Parse(string response)
        {
            Assert.IsTrue(response.StartsWith("STARTED"), "Failed to start control session", response);

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
