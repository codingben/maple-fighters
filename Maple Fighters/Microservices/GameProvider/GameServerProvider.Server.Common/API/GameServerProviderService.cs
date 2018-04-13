using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
using Components.Common.Interfaces;
using JsonConfig;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using ServerCommunication.Common;

namespace GameServerProvider.Server.Common
{
    public class GameServerProviderService : ServiceBase
    {
        private const int CONNECTIONS_UPDATE_TIME = 30;

        private readonly IPeerContainer peerContainer;
        private int connections;
        private ICoroutine updateGameServerConnectionsContinuously;

        private IOutboundServerPeerLogic outboundServerPeerLogic;

        public GameServerProviderService()
        {
            peerContainer = ServerComponents.GetComponent<IPeerContainer>().AssertNotNull();
            connections = peerContainer.GetPeersCount();
        }

        protected override void OnConnectionEstablished()
        {
            base.OnConnectionEstablished();

            var secretKey = GetSecretKey().AssertNotNull(MessageBuilder.Trace("Secret key not found."));
            outboundServerPeerLogic = OutboundServerPeer.CreateCommonServerAuthenticationPeerLogic(secretKey, OnAuthenticated);
        }

        protected override void OnConnectionClosed(DisconnectReason disconnectReason)
        {
            base.OnConnectionClosed(disconnectReason);

            outboundServerPeerLogic.Dispose();
            updateGameServerConnectionsContinuously?.Dispose();
        }

        private void OnAuthenticated()
        {
            outboundServerPeerLogic.Dispose();
            outboundServerPeerLogic = OutboundServerPeer.CreateOutboundServerPeerLogic<ServerOperations, EmptyEventCode>();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with GameServerProvider service."));

            RegisterGameServer();

            var coroutinesManager = ServerComponents.GetComponent<ICoroutinesManager>().AssertNotNull();
            updateGameServerConnectionsContinuously = coroutinesManager.StartCoroutine(UpdateGameServerConnectionsContinuously());
        }

        private IEnumerator<IYieldInstruction> UpdateGameServerConnectionsContinuously()
        {
            while (true)
            {
                var peersCount = peerContainer.GetPeersCount();
                if (connections != peersCount)
                {
                    connections = peersCount;
                    UpdateGameServerConnections();
                }
                yield return new WaitForSeconds(CONNECTIONS_UPDATE_TIME);
            }
        }

        private void RegisterGameServer()
        {
            var parameters = GetGameServerDetailsParameters();
            outboundServerPeerLogic?.SendOperation((byte)ServerOperations.RegisterGameServer, parameters);
        }

        private void UpdateGameServerConnections()
        {
            var parameters = new UpdateGameServerConnectionsInfoRequestParameters(connections);
            outboundServerPeerLogic?.SendOperation((byte)ServerOperations.UpdateGameServerConnections, parameters);
        }

        private RegisterGameServerRequestParameters GetGameServerDetailsParameters()
        {
            LogUtils.Assert(Config.Global.ConnectionDetails, MessageBuilder.Trace("Could not find a connection details for the Game server."));

            var name = (string)Config.Global.ConnectionDetails.Name;
            var ip = (string)Config.Global.ConnectionDetails.IP;
            var port = (int)Config.Global.ConnectionDetails.Port;
            var maxConnections = (int)Config.Global.ConnectionDetails.MaxConnections;
            return new RegisterGameServerRequestParameters(name, ip, port, connections, maxConnections);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.GameServerProviderService, MessageBuilder.Trace("Could not find a connection info for the Game Server Provider service."));

            var ip = (string)Config.Global.GameServerProviderService.IP;
            var port = (int)Config.Global.GameServerProviderService.Port;
            return new PeerConnectionInformation(ip, port);
        }

        private string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.GameServerProviderService, MessageBuilder.Trace("Could not find a configuration for the GameServerProvider service."));

            var secretKey = (string)Config.Global.GameServerProviderService.SecretKey;
            return secretKey;
        }
    }
}