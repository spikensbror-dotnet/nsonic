using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class LockingSession : ISession
    {
        private readonly ISession session;
        private readonly IClient client;

        public LockingSession(ISession session, IClient client)
        {
            this.session = session;
            this.client = client;
            this.client.Semaphore.Wait();
        }

        public void Dispose()
        {
            this.client.Semaphore.Release();
            this.session.Dispose();
        }

        public string Read()
        {
            return this.session.Read();
        }

        public Task<string> ReadAsync()
        {
            return this.session.ReadAsync();
        }

        public void Write(params string[] args)
        {
            this.session.Write(args);
        }

        public Task WriteAsync(params string[] args)
        {
            return this.session.WriteAsync(args);
        }
    }

    class Session : ISession
    {
        public Session(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; }

        public void Dispose()
        {
            //
        }

        public string Read()
        {
            return new StreamReader(this.Client.GetStream()).ReadLine();
        }

        public async Task<string> ReadAsync()
        {
            return await new StreamReader(await this.Client.GetStreamAsync()).ReadLineAsync();
        }

        public void Write(params string[] args)
        {
            var writer = new StreamWriter(this.Client.GetStream());
            writer.WriteLine(this.CreateMessage(args));
            writer.Flush();
        }

        public async Task WriteAsync(params string[] args)
        {
            var writer = new StreamWriter(await this.Client.GetStreamAsync());
            await writer.WriteLineAsync(this.CreateMessage(args));
            await writer.FlushAsync();
        }

        private string CreateMessage(string[] args)
        {
            var message = string.Join(" ", args.Where(a => !string.IsNullOrEmpty(a))).Trim();
            Assert.IsTrue(message.Length <= this.Client.Environment.MaxBufferStringLength, "Message was too long", message);

            return message;
        }
    }
}
