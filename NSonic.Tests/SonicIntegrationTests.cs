using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace NSonic.Tests
{
    /// <summary>
    /// These tests should be ignored when pushed to the repository since the CI
    /// does not have an environment to run against. You should ensure however
    /// that these tests run before pushing any changes.
    /// 
    /// To run, simply CD into the NSonic.Tests project directory and run:
    /// > sonic -c sonic-config.cfg
    /// 
    /// This will setup a project-local Sonic environment that can be used to run
    /// these integration tests.
    /// </summary>
    [TestClass, Ignore]
    public class SonicIntegrationTests
    {
        const string Hostname = "localhost";
        const int Port = 1491;
        const string Secret = "SecretPassword";

        [TestMethod]
        public async Task VerifyFunctionalityAsync()
        {
            using (var control = NSonicFactory.Control(Hostname, Port, Secret))
            {
                await control.ConnectAsync();
                await control.InfoAsync();
                await control.TriggerAsync("consolidate");
            }

            using (var search = NSonicFactory.Search(Hostname, Port, Secret))
            {
                await search.ConnectAsync();
                await search.QueryAsync("messages", "user:1", "s");
                await search.SuggestAsync("messages", "user:1", "s");
            }

            using (var ingest = NSonicFactory.Ingest(Hostname, Port, Secret))
            {
                await ingest.ConnectAsync();
                await ingest.PushAsync("messages", "user:1", "conversation:1", "This is an example push.", locale: "SWE");
                await ingest.PopAsync("messages", "user:1", "conversation:1", "This is an example push.");
                await ingest.CountAsync("messages", "user:1");
                await ingest.FlushCollectionAsync("messages");
                await ingest.FlushBucketAsync("messages", "user:1");
                await ingest.FlushObjectAsync("messages", "user:1", "conversation:1");
            }
        }

        [TestMethod]
        public void VerifyFunctionality()
        {
            using (var control = NSonicFactory.Control(Hostname, Port, Secret))
            {
                control.Connect();
                control.Info();
                control.Trigger("consolidate");
            }

            using (var search = NSonicFactory.Search(Hostname, Port, Secret))
            {
                search.Connect();
                search.Query("messages", "user:1", "s");
                search.Suggest("messages", "user:1", "s");
            }

            using (var ingest = NSonicFactory.Ingest(Hostname, Port, Secret))
            {
                ingest.Connect();
                ingest.Push("messages", "user:1", "conversation:1", "This is an example push.", locale: "SWE");
                ingest.Pop("messages", "user:1", "conversation:1", "This is an example push.");
                ingest.Count("messages", "user:1");
                ingest.FlushCollection("messages");
                ingest.FlushBucket("messages", "user:1");
                ingest.FlushObject("messages", "user:1", "conversation:1");
            }
        }
    }
}
