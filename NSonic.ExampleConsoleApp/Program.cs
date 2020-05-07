using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSonic.ExampleConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
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

                    info = await control.InfoAsync();
                    Console.WriteLine($"INFO (async): {info}");

                    control.Trigger("consolidate");
                }

                using (var search = NSonicFactory.Search(hostname, port, secret))
                {
                    await search.ConnectAsync();

                    var queryResults = search.Query("messages", "user:1", "s");
                    Console.WriteLine($"QUERY: {string.Join(", ", queryResults)}");

                    queryResults = await search.QueryAsync("messages", "user:1", "s");
                    Console.WriteLine($"QUERY (async): {string.Join(", ", queryResults)}");

                    var suggestResults = search.Suggest("messages", "user:1", "s");
                    Console.WriteLine($"SUGGEST: {string.Join(", ", suggestResults)}");

                    suggestResults = await search.SuggestAsync("messages", "user:1", "s");
                    Console.WriteLine($"SUGGEST (async): {string.Join(", ", suggestResults)}");
                }

                using (var ingest = NSonicFactory.Ingest(hostname, port, secret))
                {
                    ingest.Connect();

                    ingest.Push("messages", "user:1", "conversation:1", "This is an example push.", locale: null);

                    var popResult = ingest.Pop("messages", "user:1", "conversation:1", "This is an example push.");
                    Console.WriteLine($"POP: {popResult}");

                    popResult = await ingest.PopAsync("messages", "user:1", "conversation:1", "This is an example push.");
                    Console.WriteLine($"POP (async): {popResult}");

                    var countResult = ingest.Count("messages", "user:1");
                    Console.WriteLine($"COUNT: {countResult}");

                    countResult = await ingest.CountAsync("messages", "user:1");
                    Console.WriteLine($"COUNT (async): {countResult}");

                    var flushCollectionResult = ingest.FlushCollection("messages");
                    Console.WriteLine($"FLUSHC: {flushCollectionResult}");

                    flushCollectionResult = await ingest.FlushCollectionAsync("messages");
                    Console.WriteLine($"FLUSHC (async): {flushCollectionResult}");

                    var flushBucketResult = ingest.FlushBucket("messages", "user:1");
                    Console.WriteLine($"FLUSHB: {flushBucketResult}");

                    flushBucketResult = await ingest.FlushBucketAsync("messages", "user:1");
                    Console.WriteLine($"FLUSHB (async): {flushBucketResult}");

                    var flushObjectResult = ingest.FlushObject("messages", "user:1", "conversation:1");
                    Console.WriteLine($"FLUSHO: {flushObjectResult}");

                    flushObjectResult = await ingest.FlushObjectAsync("messages", "user:1", "conversation:1");
                    Console.WriteLine($"FLUSHO (async): {flushObjectResult}");
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
