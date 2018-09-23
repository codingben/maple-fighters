using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    /// <summary>
    /// Represents an access to the server application from the outside.
    /// </summary>
    public interface IApplicationBase
    {
        /// <summary>
        /// Awakes the server application.
        /// </summary>
        void Startup();

        /// <summary>
        /// Kills the server application.
        /// </summary>
        void Shutdown();

        /// <summary>
        /// Called when a new client connected.
        /// </summary>
        /// <param name="clientPeer">The client peer.</param>
        void Connected(IClientPeer clientPeer);
    }
}