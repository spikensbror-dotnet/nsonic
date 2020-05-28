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

        private StreamReader streamReader;

        private StreamWriter streamWriter;

        public Session(IClient client)
        {
            this.Client = client;
        }

        public IClient Client { get; }

        public void Dispose()
        {
        }

        public string Read()
        {
            if (streamReader == null)
            {
                streamReader = new StreamReader(this.Client.GetStream());
            }

            return streamReader.ReadLine();
        }

        public async Task<string> ReadAsync()
        {
            if (streamReader == null)
            {
                streamReader = new StreamReader(await this.Client.GetStreamAsync());
            }

            return await streamReader.ReadLineAsync();
        }

        public void Write(params string[] args)
        {
            if (streamWriter == null)
            {
                streamWriter = new StreamWriter(this.Client.GetStream());
            }

            streamWriter.WriteLine(this.CreateMessage(args));
            streamWriter.Flush();
        }

        public async Task WriteAsync(params string[] args)
        {
            if (streamWriter == null)
            {
                streamWriter = new StreamWriter(await this.Client.GetStreamAsync());
            }

            await streamWriter.WriteLineAsync(this.CreateMessage(args));
            await streamWriter.FlushAsync();
        }

        private string CreateMessage(string[] args)
        {
            var message = string.Join(" ", args.Where(a => !string.IsNullOrEmpty(a))).Trim();
            Assert.IsTrue(message.Length <= this.Client.Environment.MaxBufferStringLength, "Message was too long", message);

            return message;
        }
    }
}
