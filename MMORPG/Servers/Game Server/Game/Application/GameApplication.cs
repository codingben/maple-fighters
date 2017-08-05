using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using Game.Application.PeerLogic;
using log4net.Config;
using Photon.SocketServer;

namespace Game.Application
{
    internal class GameApplication : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new ClientPeerLogic(initRequest);
        }

        protected override void Setup()
        {
            var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (!file.Exists)
            {
                return;
            }

            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            XmlConfigurator.ConfigureAndWatch(file);
        }

        protected override void TearDown()
        {
            Logger.Log.Debug("GameApplication->TearDown()");
        }
    }
}