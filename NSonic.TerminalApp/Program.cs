using NSonic.Impl;
using NSonic.Impl.Net;
using System;

namespace NSonic.TerminalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = new SonicSessionFactory();
            var requestWriter = new SonicRequestWriter();
            var connector = new SonicClientConnector(sessionFactory, requestWriter);

            using (var tcpClient = new SonicClient(connector, new TcpClientAdapter()))
            using (var session = sessionFactory.Create(tcpClient))
            {
                Console.WriteLine(".NET Sonic Terminal");
                Console.WriteLine("Write .read to read next line from the server.");

                while (true)
                {
                    var response = session.Read();
                    Console.WriteLine($"R > {response}");

                    if (response.StartsWith("ENDED"))
                    {
                        break;
                    }

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
                }
            }
        }
    }
}
