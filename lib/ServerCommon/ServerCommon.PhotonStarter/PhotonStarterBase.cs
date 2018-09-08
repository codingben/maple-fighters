using Photon.SocketServer;
using PhotonServerImplementation;
using PhotonServerImplementation.Server;
using ServerCommon.Application;
using ServerCommunicationInterfaces;

namespace ServerCommon.PhotonStarter
{
    using ApplicationBase = PhotonServerImplementation.ApplicationBase;

    public abstract class PhotonStarterBase<TApplication> : ApplicationBase
        where TApplication : class, IApplicationBase
    {
        private TApplication application;

        protected override void Setup()
        {
            var photonFiberProvider = new PhotonFiberProvider();
            var serverConnector = new PhotonServerConnector(this, ApplicationName);

            application = CreateApplication(photonFiberProvider, serverConnector);
            application.Startup();
        }

        protected override void TearDown()
        {
            application.Shutdown();
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var clientPeer = new PhotonClientPeer(initRequest);
            clientPeer.Fiber.Enqueue(() => application.Connected(clientPeer));

            return clientPeer;
        }

        protected abstract TApplication CreateApplication(
            IFiberProvider fiberProvider, IServerConnector serverConnector);
    }
}