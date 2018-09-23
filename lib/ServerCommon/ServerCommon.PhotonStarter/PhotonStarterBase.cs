using CommonTools.Log;
using Photon.SocketServer;
using PhotonServerImplementation;
using PhotonServerImplementation.Server;
using ServerCommon.Application;
using ServerCommon.Logging;
using ServerCommunicationInterfaces;

namespace ServerCommon.PhotonStarter
{
    using ApplicationBase = PhotonServerImplementation.ApplicationBase;

    /// <summary>
    /// The starter of the server application which uses photon socket.
    /// </summary>
    /// <typeparam name="TApplication">The server application.</typeparam>
    public abstract class PhotonStarterBase<TApplication> : ApplicationBase
        where TApplication : class, IApplicationBase
    {
        private readonly ILogger logger;
        private TApplication application;

        protected PhotonStarterBase()
        {
            logger = new Logger();
        }

        protected override void Setup()
        {
            var photonFiberProvider = new PhotonFiberProvider();
            var serverConnector =
                new PhotonServerConnector(this, ApplicationName);

            application = CreateApplication(
                logger,
                photonFiberProvider,
                serverConnector);
            application.Startup();

            logger.Log("An application has started.");
        }

        protected override void TearDown()
        {
            application.Shutdown();

            logger.Log("An application has been stopped.");
        }

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var clientPeer = new PhotonClientPeer(initRequest);
            clientPeer.Fiber.Enqueue(() => OnPeerCreated(clientPeer));

            return clientPeer;
        }

        protected abstract TApplication CreateApplication(
            ILogger logger,
            IFiberProvider fiberProvider,
            IServerConnector serverConnector);

        private void OnPeerCreated(IClientPeer clientPeer)
        {
            application.Connected(clientPeer);

            var ip = clientPeer.ConnectionInformation.Ip;
            var port = clientPeer.ConnectionInformation.Port;
            logger.Log($"A new peer has been connected - {ip}:{port}");
        }
    }
}