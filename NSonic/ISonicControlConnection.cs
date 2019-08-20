namespace NSonic
{
    /// <summary>
    /// A Sonic control connection is used to interact with a Sonic connection in
    /// control mode.
    /// </summary>
    public interface ISonicControlConnection : ISonicConnection
    {
        /// <summary>
        /// Retrieves the Sonic server information.
        /// </summary>
        /// <returns>The server information received from Sonic.</returns>
        string Info();

        /// <summary>
        /// Triggers an action on the Sonic server.
        /// </summary>
        /// <param name="action">The action to trigger.</param>
        /// <param name="data">The data to pass to the action.</param>
        void Trigger(string action, string data = null);
    }
}
