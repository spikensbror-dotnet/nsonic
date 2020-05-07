using NSonic.Impl.Net;
using NSonic.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSonic.Impl
{
    class SonicSession : ISonicSession
    {
        private readonly SemaphoreSlim semaphore;

        public SonicSession(ITcpClient client, SemaphoreSlim semaphore, EnvironmentResponse environment)
        {
            this.semaphore = semaphore;

            // As long as the session is alive, it should carry an exclusive lock of the TCP client
            // to prevent operations across threads.
            this.semaphore.Wait();

            this.Client = client;
            this.Environment = environment;
        }

        public ITcpClient Client { get; }
        public EnvironmentResponse Environment { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            // Release the TCP client lock.
            this.semaphore.Release();
        }

        public string Read()
        {
            return new StreamReader(this.Client.GetStream()).ReadLine();
        }

        public async Task<string> ReadAsync()
        {
            return await new StreamReader(this.Client.GetStream()).ReadLineAsync();
        }

        public void Write(params string[] args)
        {
            var writer = new StreamWriter(this.Client.GetStream());
            writer.WriteLine(this.CreateMessage(args));
            writer.Flush();
        }

        public async Task WriteAsync(params string[] args)
        {
            var writer = new StreamWriter(this.Client.GetStream());
            await writer.WriteLineAsync(this.CreateMessage(args));
            await writer.FlushAsync();
        }

        private string CreateMessage(string[] args)
        {
            var message = string.Join(" ", args.Where(a => !string.IsNullOrEmpty(a))).Trim();
            Assert.IsTrue(message.Length <= this.Environment.MaxBufferStringLength, "Message was too long");

            return message;
        }
    }
}
