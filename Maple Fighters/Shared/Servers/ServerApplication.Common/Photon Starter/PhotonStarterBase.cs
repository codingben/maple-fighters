using System.IO;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Photon.SocketServer;
using PhotonServerImplementation;
using ServerApplication.Common.ApplicationBase;

namespace ServerApplication.Common.PhotonStarter
{
    public abstract class PhotonStarterBase<T> : PhotonServerImplementation.ApplicationBase
        where T : Application
    {
        private const string LOGGER_PATH = "../log4net.config";

        private T application;

        protected abstract T CreateApplication();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogUtils.Log($"A new peer has been created -> {initRequest.RemoteIP}:{initRequest.LocalPort}");

            var clientPeer = new PhotonClientPeer(initRequest)
            {
                NetworkTrafficState = NetworkTrafficState.Flowing
            };

            clientPeer.Fiber.Enqueue(() => application.OnConnected(clientPeer));

            return clientPeer;
        }

        protected override void Setup()
        {
            LogUtils.Logger = CreateLogger();

            application = CreateApplication();
            application?.Initialize();
        }

        protected override void TearDown()
        {
            application?.Dispose();
        }

        private Logger.Logger CreateLogger()
        {
            var logger = new Logger.Logger();
            logger.Initialize(Path.Combine(BinaryPath, LOGGER_PATH));

            return logger;
        }
    }
}