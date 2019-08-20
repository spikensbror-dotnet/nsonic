using System;

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
    }
}
