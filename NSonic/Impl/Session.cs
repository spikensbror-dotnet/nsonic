using NSonic.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class Session : ISession
    {
        private SemaphoreSlim semaphore;

        public Session(IClient client)
        {
            // As long as the session is alive, it should carry an exclusive lock of the TCP client
            // to prevent operations across threads.
            this.semaphore = client.Semaphore;
            this.semaphore.Wait();

            this.Client = client;
        }

        public IClient Client { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            // Release the TCP client lock.
            this.semaphore.Release();
        }

        public string Read()
        {
            this.FixSemaphore();

            return new StreamReader(this.Client.GetStream()).ReadLine();
        }

        public async Task<string> ReadAsync()
        {
            this.FixSemaphore();

            return await new StreamReader(await this.Client.GetStreamAsync()).ReadLineAsync();
        }

        public void Write(params string[] args)
        {
            this.FixSemaphore();

            var writer = new StreamWriter(this.Client.GetStream());
            writer.WriteLine(this.CreateMessage(args));
            writer.Flush();
        }

        public async Task WriteAsync(params string[] args)
        {
            this.FixSemaphore();

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

        private void FixSemaphore()
        {
            if (this.Client.Semaphore != this.semaphore)
            {
                this.semaphore.Release();
            }

            this.semaphore = this.Client.Semaphore;
        }
    }
}
