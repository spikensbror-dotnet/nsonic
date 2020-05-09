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

            return StartResponseParser.Parse(session.Read());
        }

        public async Task<EnvironmentResponse> WriteStartAsync(ISonicSession session, string mode, string secret)
        {
            var response = await session.ReadAsync();
            SonicRequestWriterAssert.Connected(response);

            await session.WriteAsync("START", mode, secret);

            return StartResponseParser.Parse(await session.ReadAsync());
        }
    }
}
