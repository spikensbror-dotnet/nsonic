using System;
using System.Diagnostics;

namespace NSonic.ExampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostname = "localhost";
            var port = 1491;
            var secret = "SecretPassword";

            try
            {
                using (var control = NSonicFactory.Control(hostname, port, secret))
                {
                    control.Connect();

                    var info = control.Info();
                    Console.WriteLine($"INFO: {info}");

                    control.Trigger("consolidate");
                }

                using (var search = NSonicFactory.Search(hostname, port, secret))
                {
                    search.Connect();

                    var queryResults = search.Query("messages", "user:1", "s");
                    Console.WriteLine($"QUERY: {string.Join(", ", queryResults)}");

                    var suggestResults = search.Suggest("messages", "user:1", "s");
                    Console.WriteLine($"SUGGEST: {string.Join(", ", suggestResults)}");
                }

                using (var ingest = NSonicFactory.Ingest(hostname, port, secret))
                {
                    ingest.Connect();

                    ingest.Push("messages", "user:1", "conversation:1", "This is an example push.", locale: null);

                    var popResult = ingest.Pop("messages", "user:1", "conversation:1", "This is an example push.");
                    Console.WriteLine($"POP: {popResult}");

                    var countResult = ingest.Count("messages", "user:1");
                    Console.WriteLine($"COUNT: {countResult}");

                    var flushCollectionResult = ingest.FlushCollection("messages");
                    Console.WriteLine($"FLUSHC: {flushCollectionResult}");

                    var flushBucketResult = ingest.FlushBucket("messages", "user:1");
                    Console.WriteLine($"FLUSHB: {flushBucketResult}");

                    var flushObjectResult = ingest.FlushObject("messages", "user:1", "conversation:1");
                    Console.WriteLine($"FLUSHO: {flushObjectResult}");
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
