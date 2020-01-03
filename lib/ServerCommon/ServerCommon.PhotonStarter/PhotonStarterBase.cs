using Photon.SocketServer;
using PhotonServerImplementation;
using PhotonServerImplementation.Server;
using ServerCommon.Application;
using ServerCommunicationInterfaces;

namespace ServerCommon.PhotonStarter
{
    /// <summary>
    /// The starter of the server application which uses photon socket server.
    /// </summary>
    /// <typeparam name="TApplication">The server application.</typeparam>
    /// <typeparam name="TPeerCreator">The client peer.</typeparam>
    public abstract class PhotonStarterBase<TApplication, TPeerCreator> : PhotonServerImplementation.ApplicationBase
        where TApplication : class, IApplicationBase
        where TPeerCreator : class, new()
    {
        private TApplication application;

        protected override void Setup()
        {
            var serverConnector =
                new PhotonServerConnector(this, ApplicationName);
            var fiberProvider = new PhotonFiberProvider();

            application = CreateApplication(serverConnector, fiberProvider);
            application.Startup();
        }

        protected override void TearDown()
        {
            application.Shutdown();
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var peer = new PhotonClientPeer(initRequest);
            peer.Fiber.Enqueue(() => 
            {
                // TODO: Implement
            });

            return peer;
        }

        protected abstract TApplication CreateApplication(IServerConnector serverConnector, IFiberProvider fiberProvider);
    }
}