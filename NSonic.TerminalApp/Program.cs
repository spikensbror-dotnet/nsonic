using NSonic.Impl;
using NSonic.Impl.Net;
using System;
using System.Threading.Tasks;

namespace NSonic.TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = new SessionFactory();
            var requestWriter = new RequestWriter();
            var connector = new ClientConnector(sessionFactory, requestWriter);

            using (var client = new Client(connector, new TcpClientAdapter()))
            {
                client.Configure(new Configuration("localhost", 1491, "SecretPassword", ConnectionMode.Search));

                client.Connect();

                using (var session = sessionFactory.Create(client))
                {
                    Console.WriteLine(".NET Sonic Terminal");
                    Console.WriteLine("Write .read to read next line from the server.");

                    while (true)
                    {
                        while (true)
                        {
                            Console.Write($"W > ");

                            var input = Console.ReadLine();
                            if (input.ToLower().Trim() == ".read")
                            {
                                break;
                            }
                            else if (!string.IsNullOrWhiteSpace(input))
                            {
                                session.Write(input);
                                break;
                            }
                        }

                        var response = session.Read();
                        Console.WriteLine($"R > {response}");

                        if (response.StartsWith("ENDED"))
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
