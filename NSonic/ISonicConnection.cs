using System;
using System.Threading.Tasks;

namespace NSonic
{
    /// <summary>
    /// Defines the base functionality of a Sonic connection.
    /// </summary>
    public interface ISonicConnection : IDisposable
    {
        /// <summary>
        /// Establishes a connection to the Sonic server and starts a session
        /// with the appropriate mode.
        /// </summary>
        void Connect();

        /// <summary>
        /// Establishes a connection asynchronously to the Sonic server and
        /// starts a session with the appropriate mode.
        /// </summary>
        /// <returns></returns>
        Task ConnectAsync();
    }
}
