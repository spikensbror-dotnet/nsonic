using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class SonicRequestWriter : ISonicRequestWriter
    {
        public void WriteOk(ISonicSession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            SonicRequestWriterAssert.Ok(response);
        }

        public async Task WriteOkAsync(ISonicSession session, params string[] args)
        {
            await session.WriteAsync(args);

            var response = await session.ReadAsync();
            SonicRequestWriterAssert.Ok(response);
        }

        public string WriteResult(ISonicSession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            SonicRequestWriterAssert.Result(response);

            return response.Substring("RESULT ".Length);
        }

        public async Task<string> WriteResultAsync(ISonicSession session, params string[] args)
        {
            await session.WriteAsync(args);

            var response = await session.ReadAsync();
            SonicRequestWriterAssert.Result(response);

            return response.Substring("RESULT ".Length);
        }

        public EnvironmentResponse WriteStart(ISonicSession session, string mode, string secret)
        {
            var response = session.Read();
            SonicRequestWriterAssert.Connected(response);

            session.Write("START", mode, secret);

            return this.ParseStartResponse(session.Read());
        }

        public async Task<EnvironmentResponse> WriteStartAsync(ISonicSession session, string mode, string secret)
        {
            var response = await session.ReadAsync();
            SonicRequestWriterAssert.Connected(response);

            await session.WriteAsync("START", mode, secret);

            return this.ParseStartResponse(await session.ReadAsync());
        }

        private EnvironmentResponse ParseStartResponse(string response)
        {
            SonicRequestWriterAssert.Started(response);

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
