using System.IO;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class RetryingSession : ISession
    {
        private readonly ISession session;

        public RetryingSession(ISession session)
        {
            this.session = session;
        }

        public void Dispose()
        {
            this.session.Dispose();
        }

        public string Read()
        {
            try
            {
                return this.session.Read();
            }
            catch (IOException)
            {
                return this.session.Read();
            }
        }

        public async Task<string> ReadAsync()
        {
            try
            {
                return await this.session.ReadAsync();
            }
            catch (IOException)
            {
                return await this.session.ReadAsync();
            }
        }

        public void Write(params string[] args)
        {
            try
            {
                this.session.Write(args);
            }
            catch (IOException)
            {
                this.session.Write(args);
            }
        }

        public async Task WriteAsync(params string[] args)
        {
            try
            {
                await this.session.WriteAsync(args);
            }
            catch (IOException)
            {
                await this.session.WriteAsync(args);
            }
        }
    }
}
