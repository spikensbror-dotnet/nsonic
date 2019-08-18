using System.Threading.Tasks;

namespace NSonic
{
    public interface ISonicControlConnection : ISonicConnection
    {
        string Info();
        void Ping();
        void Trigger(string action, string data);
    }
}
