using System.IO;
using CommonTools.Log;
using Photon.SocketServer;
using PhotonServerImplementation;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using PhotonStarter.Common.Utils;
using JsonConfig;

namespace PhotonStarter.Common
{
    public abstract class PhotonStarterBase<T> : PhotonServerImplementation.ApplicationBase
        where T : IApplicationBase
    {
        private T application;

        protected override void Setup()
        {
            LogUtils.Logger = CreateLogger();
            Config.Global = CreateJsonConfiguration();

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
            var clientPeer = new PhotonClientPeer(initRequest);
            clientPeer.Fiber.Enqueue(() => application.OnConnected(clientPeer));
            return clientPeer;
        }

        private ConfigObject CreateJsonConfiguration()
        {
            var directoryInfo = new DirectoryInfo("configuration/json");
            var configObject = Config.ApplyFromDirectoryInfo(directoryInfo);
            return configObject;
        }

        private Logger CreateLogger()
        {
            var path = Path.Combine(BinaryPath, "../configuration/log4net.config");
            return new Logger(path);
        }
    }
}