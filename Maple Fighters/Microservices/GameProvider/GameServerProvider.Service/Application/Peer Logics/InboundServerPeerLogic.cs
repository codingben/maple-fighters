using CommonCommunicationInterfaces;
using CommunicationHelper;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components.Interfaces;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;

namespace GameServerProvider.Service.Application.PeerLogics
{
    internal class InboundServerPeerLogic : PeerLogicBase<ServerOperations, EmptyEventCode>
    {
        protected override void OnInitialized()
        {
            SubscribeToDisconnectionNotifier();

            AddHandlerForRegisterGameServerOperation();
            AddHandlerForUpdateGameServerConnectionsOperation();
        }

        private void AddHandlerForRegisterGameServerOperation()
        {
            var peerId = ClientPeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(ServerOperations.RegisterGameServer, new RegisterGameServerOperationHandler(peerId));
        }

        private void AddHandlerForUpdateGameServerConnectionsOperation()
        {
            var peerId = ClientPeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(ServerOperations.UpdateGameServerConnections, new UpdateGameServerConnectionsOperationHandler(peerId));
        }

        private void SubscribeToDisconnectionNotifier()
        {
            ClientPeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            ClientPeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();
            UnregisterGameServer();
        }

        private void UnregisterGameServer()
        {
            var peerId = ClientPeerWrapper.PeerId;
            var gameServerInformationRemover = ServerComponents.GetComponent<IGameServerInformationRemover>();
            gameServerInformationRemover?.Remove(peerId);
        }
    }
}