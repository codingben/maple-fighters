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
    public class GameServerProviderService : ServiceBase<EmptyOperationCode, EmptyEventCode>
    {
        private int connections;
        private readonly IPeerContainer peerContainer;
        private ICoroutine updateGameServerConnectionsContinuously;

        private const int CONNECTIONS_UPDATE_TIME = 30;

        public GameServerProviderService()
        {
            peerContainer = ServerComponents.GetComponent<IPeerContainer>().AssertNotNull();
            connections = peerContainer.GetPeersCount();
        }

        protected override void OnAuthenticated()
        {
            base.OnAuthenticated();

            LogUtils.Log(MessageBuilder.Trace("Authenticated with GameServerProvider service."));

            RegisterGameServer();

            var coroutinesManager = ServerComponents.GetComponent<ICoroutinesManager>().AssertNotNull();
            updateGameServerConnectionsContinuously = coroutinesManager.StartCoroutine(UpdateGameServerConnectionsContinuously());
        }

        protected override void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            base.OnDisconnected(disconnectReason, details);

            updateGameServerConnectionsContinuously?.Dispose();
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
            OutboundServerPeerLogic?.SendOperation((byte)ServerOperations.RegisterGameServer, parameters);
        }

        private void UpdateGameServerConnections()
        {
            var parameters = new UpdateGameServerConnectionsInfoRequestParameters(connections);
            OutboundServerPeerLogic?.SendOperation((byte)ServerOperations.UpdateGameServerConnections, parameters);
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

        protected override string GetSecretKey()
        {
            LogUtils.Assert(Config.Global.GameServerProviderService, MessageBuilder.Trace("Could not find a configuration for the GameServerProvider service."));

            var secretKey = (string)Config.Global.GameServerProviderService.SecretKey;
            return secretKey;
        }
    }
}