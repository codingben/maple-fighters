using CommonCommunicationInterfaces;
using CommunicationHelper;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components.Interfaces;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;

namespace GameServerProvider.Service.Application.PeerLogics
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class InboundServerPeerLogic : PeerLogicBase<ServerOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectionNotifier();

            AddHandlerForRegisterGameServerOperation();
        }

        private void AddHandlerForRegisterGameServerOperation()
        {
            var peerId = ClientPeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(ServerOperations.RegisterGameServer, new RegisterGameServerOperationHandler(peerId));
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
            var gameServerInformationRemover = Server.Components.GetComponent<IGameServerInformationRemover>();
            gameServerInformationRemover?.Remove(peerId);
        }
    }
}