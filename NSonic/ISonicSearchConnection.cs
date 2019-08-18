using System.Threading.Tasks;

namespace NSonic
{
    public interface ISonicSearchConnection : ISonicConnection
    {
        string[] Query(string collection
            , string bucket
            , string terms
            , int? count = null
            , int? limit = null
            , string locale = null
            );

        string[] Suggest(string collection
            , string bucket
            , string word
            , int? limit = null
            );
    }
}
