using NSonic.Impl.Net;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace NSonic.Impl
{
    class SonicSession : ISonicSession
    {
        public SonicSession(ITcpClient client)
        {
            // As long as the session is alive, it should carry an exclusive lock of the TCP client
            // to prevent operations across threads.
            Monitor.Enter(client);

            this.Client = client;
        }

        public ITcpClient Client { get; }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            // Release the TCP client lock.
            Monitor.Exit(this.Client);
        }

        public string Read()
        {
            return new StreamReader(this.Client.GetStream()).ReadLine();
        }

        public void Write(params string[] args)
        {
            var message = string.Join(" ", args.Where(a => !string.IsNullOrEmpty(a))).Trim();

            var writer = new StreamWriter(this.Client.GetStream());
            writer.WriteLine(message);
            writer.Flush();

            /*
            var buffer = Encoding.UTF8.GetBytes(message);

            this.Client.GetStream().Write(buffer, 0, buffer.Length);
            */
        }
    }
}
