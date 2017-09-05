using System.IO;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Photon.SocketServer;
using PhotonServerImplementation;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.PhotonStarter
{
    public abstract class PhotonStarterBase<T> : PhotonServerImplementation.ApplicationBase
        where T : IApplication
    {
        private const string LOGGER_PATH = "../log4net.config";

        private T application;

        protected override void Setup()
        {
            LogUtils.Logger = CreateLogger();

            application = CreateApplication(new PhotonFiberProvider());
            application.Startup();
        }

        protected override void TearDown()
        {
            application.Shutdown();
        }

        protected abstract T CreateApplication(IFiberProvider fiberProvider);

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var clientPeer = new PhotonClientPeer(initRequest)
            {
                NetworkTrafficState = NetworkTrafficState.Paused
            };

            clientPeer.Fiber.Enqueue(() =>
            {
                application.OnConnected(clientPeer);
                LogUtils.Log($"A new peer has been connected -> {clientPeer.ConnectionInformation.Ip}:{clientPeer.ConnectionInformation.Port}");
            });
            return clientPeer;
        }

        private Logger.Logger CreateLogger()
        {
            var logger = new Logger.Logger();
            logger.Initialize(Path.Combine(BinaryPath, LOGGER_PATH));
            return logger;
        }
    }
}