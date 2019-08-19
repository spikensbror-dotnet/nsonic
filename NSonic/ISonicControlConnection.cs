namespace NSonic
{
    public interface ISonicControlConnection : ISonicConnection
    {
        string Info();
        void Trigger(string action, string data);
    }
}
