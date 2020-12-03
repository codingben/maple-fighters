using CommonTools.Log;
using JsonConfig;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Server2
{
    public class Server2Application : ApplicationBase
    {
        public Server2Application(IFiberProvider fiberProvider, IServerConnector serverConnector)
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            LogUtils.Assert(Config.Global.ConnectionInfo, MessageBuilder.Trace("Could not find an connection info for the server."));

            var udpPort = (int)Config.Global.ConnectionInfo.UdpPort;
            var tcpPort = (int)Config.Global.ConnectionInfo.TcpPort;

            var clientConnectionInfo = clientPeer.ConnectionInformation;
            if (clientConnectionInfo.Port == udpPort)
            {
                WrapClientPeer(clientPeer, new ClientPeerLogic());
            }
            else if(clientConnectionInfo.Port == tcpPort)
            {
                WrapClientPeer(clientPeer, new ServerPeerLogic());
            }
            else
            {
                LogUtils.Log($"No handler found for peer: {clientConnectionInfo.Ip}:{clientConnectionInfo.Port}");
            }
        }
    }
}