namespace NSonic.Impl
{
    interface ISonicRequestWriter
    {
        string WriteResult(ISonicSession session, params string[] args);
        void WriteOk(ISonicSession session, params string[] args);
    }
}
