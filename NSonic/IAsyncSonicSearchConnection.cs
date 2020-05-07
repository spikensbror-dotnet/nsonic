using System.Threading.Tasks;

namespace NSonic
{
    /// <summary>
    /// Defines the asynchronous methods available for Sonic search mode connections.
    /// </summary>
    public interface IAsyncSonicSearchConnection : ISonicConnection
    {
        /// <summary>
        /// Queries the specified bucket for the given terms.
        /// </summary>
        /// <param name="collection">The collection of the bucket.</param>
        /// <param name="bucket">The bucket to query.</param>
        /// <param name="terms">The terms to query for.</param>
        /// <param name="limit">The maximum amount of results to return.</param>
        /// <param name="offset">The amount of results to skip.</param>
        /// <param name="locale">The locale to query for.</param>
        /// <returns>An array of objects that match the query.</returns>
        Task<string[]> QueryAsync(string collection
            , string bucket
            , string terms
            , int? limit = null
            , int? offset = null
            , string locale = null
            );

        /// <summary>
        /// Gathers suggestions for the given terms from the specified bucket.
        /// </summary>
        /// <param name="collection">The collection of the bucket.</param>
        /// <param name="bucket">The bucket to query.</param>
        /// <param name="word">The word to get suggestions for.</param>
        /// <param name="limit">The maximum amount of results to return.</param>
        /// <returns>An array of suggestions that match the word.</returns>
        Task<string[]> SuggestAsync(string collection
            , string bucket
            , string word
            , int? limit = null
            );
    }
}
