using System.Threading.Tasks;

namespace NSonic
{
    /// <summary>
    /// Defines the asynchronous methods available for Sonic control mode connections.
    /// </summary>
    public interface IAsyncSonicControlConnection : ISonicConnection
    {
        /// <summary>
        /// Retrieves the Sonic server information.
        /// </summary>
        /// <returns>The server information received from Sonic.</returns>
        Task<string> InfoAsync();

        /// <summary>
        /// Triggers an action on the Sonic server.
        /// </summary>
        /// <param name="action">The action to trigger.</param>
        /// <param name="data">The data to pass to the action.</param>
        Task TriggerAsync(string action, string data = null);
    }
}
