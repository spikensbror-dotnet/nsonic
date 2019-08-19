namespace NSonic
{
    public interface ISonicIngestConnection : ISonicConnection
    {
        void Push(string collection, string bucket, string @object, string text, string locale = null);
        int Pop(string collection, string bucket, string @object, string text);
        int Count(string collection, string bucket = null, string @object = null);
        int FlushCollection(string collection);
        int FlushBucket(string collection, string bucket);
        int FlushObject(string collection, string bucket, string @object);
    }
}
