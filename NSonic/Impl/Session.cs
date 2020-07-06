using NSonic.Utils;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NSonic.Impl
{
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
            return this.Client.GetStreamReader().ReadLine();
        }

        public async Task<string> ReadAsync()
        {
            return await (await this.Client.GetStreamReaderAsync()).ReadLineAsync();
        }

        public void Write(params string[] args)
        {
            var writer = this.Client.GetStreamWriter();
            writer.WriteLine(this.CreateMessage(args));
            writer.Flush();
        }

        public async Task WriteAsync(params string[] args)
        {
            var writer = await this.Client.GetStreamWriterAsync();
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
