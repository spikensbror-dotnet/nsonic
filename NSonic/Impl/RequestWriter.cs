using System.Threading.Tasks;

namespace NSonic.Impl
{
    class RequestWriter : IRequestWriter
    {
        public void WriteOk(ISession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            RequestWriterAssert.Ok(response);
        }

        public async Task WriteOkAsync(ISession session, params string[] args)
        {
            await session.WriteAsync(args);

            var response = await session.ReadAsync();
            RequestWriterAssert.Ok(response);
        }

        public string WriteResult(ISession session, params string[] args)
        {
            session.Write(args);

            var response = session.Read();
            RequestWriterAssert.Result(response);

            return response.Substring("RESULT ".Length);
        }

        public async Task<string> WriteResultAsync(ISession session, params string[] args)
        {
            await session.WriteAsync(args);

            var response = await session.ReadAsync();
            RequestWriterAssert.Result(response);

            return response.Substring("RESULT ".Length);
        }

        public EnvironmentResponse WriteStart(ISession session, ConnectionMode mode, string secret)
        {
            var response = session.Read();
            RequestWriterAssert.Connected(response);

            session.Write("START", mode.ToString().ToLowerInvariant(), secret);

            return StartResponseParser.Parse(session.Read());
        }

        public async Task<EnvironmentResponse> WriteStartAsync(ISession session, ConnectionMode mode, string secret)
        {
            var response = await session.ReadAsync();
            RequestWriterAssert.Connected(response);

            await session.WriteAsync("START", mode.ToString().ToLowerInvariant(), secret);

            return StartResponseParser.Parse(await session.ReadAsync());
        }
    }
}
