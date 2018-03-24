using CommonCommunicationInterfaces;
using CommunicationHelper;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace GameServerProvider.Service.Application.PeerLogics
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class InboundServerPeerLogic : PeerLogicBase<ServerOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectionNotifier();

            AddHandlerForRegisterGameServerOperation();
        }

        private void AddHandlerForRegisterGameServerOperation()
        {
            var peerId = PeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(ServerOperations.RegisterGameServer, new RegisterGameServerOperationHandler(peerId));
        }
        
        private void SubscribeToDisconnectionNotifier()
        {
            PeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            PeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();
            UnregisterGameServer();
        }

        private void UnregisterGameServer()
        {
            var peerId = PeerWrapper.PeerId;
            var gameServerInformationRemover = Server.Components.GetComponent<IGameServerInformationRemover>();
            gameServerInformationRemover?.Remove(peerId);
        }
    }
}