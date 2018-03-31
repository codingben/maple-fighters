using System.IO;
using CommonTools.Log;
using Photon.SocketServer;
using PhotonServerImplementation;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using PhotonStarter.Common.Utils;
using JsonConfig;
using PhotonServerImplementation.Server;

namespace PhotonStarter.Common
{
    public abstract class PhotonStarterBase<T> : PhotonServerImplementation.ApplicationBase
        where T : class, IApplicationBase
    {
        private T application;

        protected override void Setup()
        {
            LogUtils.Logger = CreateLogger();
            Config.Global = CreateJsonConfiguration();

            var photonFiberProvider = new PhotonFiberProvider();
            var serverConnector = new PhotonServerConnector(this, ApplicationName);
            application = CreateApplication(photonFiberProvider, serverConnector);
            application.Startup();
        }

        protected override void TearDown()
        {
            application.Shutdown();
        }

        protected abstract T CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector);

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            var clientPeer = new PhotonClientPeer(initRequest);
            clientPeer.Fiber.Enqueue(() => application.OnConnected(clientPeer));
            return clientPeer;
        }

        private Logger CreateLogger()
        {
            var path = Path.Combine(BinaryPath, $"../{Configuration.LOG4NET_PATH}");
            return File.Exists(path) ? new Logger(path) : null;
        }

        private ConfigObject CreateJsonConfiguration()
        {
            return Directory.Exists(Configuration.JSON_PATH) ? Config.ApplyFromDirectoryInfo(new DirectoryInfo(Configuration.JSON_PATH)) : null;
        }
    }
}