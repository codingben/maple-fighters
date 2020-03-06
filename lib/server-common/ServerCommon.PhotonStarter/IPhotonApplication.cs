namespace ServerCommon.PhotonStarter
{
    /// <summary>
    /// Represents an access to the server application from the outside.
    /// </summary>
    public interface IPhotonApplication
    {
        /// <summary>
        /// Awakes the server application.
        /// </summary>
        void Startup();

        /// <summary>
        /// Kills the server application.
        /// </summary>
        void Shutdown();
    }
}