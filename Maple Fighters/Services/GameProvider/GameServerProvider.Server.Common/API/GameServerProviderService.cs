using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using JsonConfig;
using ServerApplication.Common.Components;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;

namespace GameServerProvider.Server.Common
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class GameServerProviderService : ServiceBase<EmptyOperationCode, EmptyEventCode>
    {
        protected override void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            base.OnConnected(outboundServerPeer);

            RegisterGameServer();
        }

        private void RegisterGameServer()
        {
            var peerContainer = Server.Components.GetComponent<IPeerContainer>().AssertNotNull();
            var parameters = GetGameServerDetailsParameters();
            parameters.Connections = peerContainer.GetPeersCount();
            OutboundServerPeerLogic?.SendOperation((byte)ServerOperations.RegisterGameServer, parameters);
        }

        private RegisterGameServerRequestParameters GetGameServerDetailsParameters()
        {
            LogUtils.Assert(Config.Global.ConnectionDetails, MessageBuilder.Trace("Could not find a connection details for the Game server."));

            var name = (string)Config.Global.ConnectionDetails.Name;
            var ip = (string)Config.Global.ConnectionDetails.IP;
            var port = (int)Config.Global.ConnectionDetails.Port;
            var maxConnections = (int)Config.Global.ConnectionDetails.MaxConnections;
            return new RegisterGameServerRequestParameters(name, ip, port, maxConnections);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.GameServerProviderService, MessageBuilder.Trace("Could not find a connection info for the Game Server Provider service."));

            var ip = (string)Config.Global.GameServerProviderService.IP;
            var port = (int)Config.Global.GameServerProviderService.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}