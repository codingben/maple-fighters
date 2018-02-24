using CommonTools.Log;
using JsonConfig;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.PeerLogic.S2S
{
    public static class Utils
    {
        public static IPeerLogicBase GetPeerLogic(this IClientPeer clientPeer, IPeerLogicBase udpPeerLogicBase, IPeerLogicBase tcpPeerLogicBase)
        {
            LogUtils.Assert(Config.Global.ConnectionInfo, MessageBuilder.Trace("Could not find an connection info for the server."));

            // var udpPort = (int)Config.Global.ConnectionInfo.UdpPort;
            var tcpPort = (int)Config.Global.ConnectionInfo.TcpPort;
            return clientPeer.ConnectionInformation.Port == tcpPort ? tcpPeerLogicBase : udpPeerLogicBase;
        }
    }
}