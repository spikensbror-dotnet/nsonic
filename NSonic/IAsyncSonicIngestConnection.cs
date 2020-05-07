using System.Threading.Tasks;

namespace NSonic
{
    /// <summary>
    /// Defines the asynchonous methods available for Sonic ingest mode connections.
    /// </summary>
    public interface IAsyncSonicIngestConnection : ISonicConnection
    {
        /// <summary>
        /// Pushes search data to an object.
        /// </summary>
        /// <param name="collection">The collection to push to.</param>
        /// <param name="bucket">The bucket to push to.</param>
        /// <param name="object">The object to push to.</param>
        /// <param name="text">The text to push.</param>
        /// <param name="locale">The locale to push for.</param>
        Task PushAsync(string collection, string bucket, string @object, string text, string locale = null);

        /// <summary>
        /// Pops search data from an object.
        /// </summary>
        /// <param name="collection">The collection to pop from.</param>
        /// <param name="bucket">The bucket to pop from.</param>
        /// <param name="object">The object to pop from.</param>
        /// <param name="text">The text to pop.</param>
        /// <returns>The amount of search data that was popped.</returns>
        Task<int> PopAsync(string collection, string bucket, string @object, string text);

        /// <summary>
        /// Counts search data in a collection, bucket or object.
        /// </summary>
        /// <param name="collection">The collection to count.</param>
        /// <param name="bucket">The bucket to count.</param>
        /// <param name="object">The object to count.</param>
        /// <returns>The amount of search data for the given target.</returns>
        Task<int> CountAsync(string collection, string bucket = null, string @object = null);

        /// <summary>
        /// Flushes search data from a collection.
        /// </summary>
        /// <param name="collection">The collection to flush.</param>
        /// <returns>The amount of search data that was flushed.</returns>
        Task<int> FlushCollectionAsync(string collection);

        /// <summary>
        /// Flushes search data from a bucket.
        /// </summary>
        /// <param name="collection">The collection of the bucket.</param>
        /// <param name="bucket">The bucket to flush.</param>
        /// <returns>The amount of search data that was flushed.</returns>
        Task<int> FlushBucketAsync(string collection, string bucket);

        /// <summary>
        /// Flushes search data from an object.
        /// </summary>
        /// <param name="collection">The collection of the object.</param>
        /// <param name="bucket">The bucket of the object.</param>
        /// <param name="object">The object to flush.</param>
        /// <returns>The amount of search data that was flushed.</returns>
        Task<int> FlushObjectAsync(string collection, string bucket, string @object);
    }
}
