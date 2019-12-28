using Photon.SocketServer;
using PhotonServerImplementation;
using PhotonServerImplementation.Server;
using ServerCommon.Application;
using ServerCommon.PeerBase;
using ServerCommunicationInterfaces;

namespace ServerCommon.PhotonStarter
{
    using ApplicationBase = PhotonServerImplementation.ApplicationBase;
    using PeerBase = Photon.SocketServer.PeerBase;

    /// <summary>
    /// The starter of the server application which uses photon socket server.
    /// </summary>
    /// <typeparam name="TApplication">The server application.</typeparam>
    /// <typeparam name="TPeerBase">The client peer.</typeparam>
    public abstract class PhotonStarterBase<TApplication, TPeerBase> : ApplicationBase
        where TApplication : class, IApplicationBase
        where TPeerBase : class, IPeerBase, new()
    {
        private TApplication application;

        protected override void Setup()
        {
            var fiberProvider = new PhotonFiberProvider();
            var serverConnector =
                new PhotonServerConnector(this, ApplicationName);

            application = CreateApplication(fiberProvider, serverConnector);
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
                var peerBase = new TPeerBase();
                peerBase.Connected(peer);
            });

            return peer;
        }

        protected abstract TApplication CreateApplication(
            IFiberProvider fiberProvider,
            IServerConnector serverConnector);
    }
}