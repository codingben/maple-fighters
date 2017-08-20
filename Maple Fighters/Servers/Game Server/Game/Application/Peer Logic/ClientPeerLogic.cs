using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Operations;
using Game.Entities;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class ClientPeerLogic : PeerLogic<GameOperations, GameEvents>
    {
        private IEntity entity;

        private readonly EntityIdToPeerIdConverter entityIdToPeerIdConverter;

        public ClientPeerLogic(IClientPeer peer, int peerId)
            : base(peer, peerId)
        {
            SetOperationsHandlers();
            CreateEntity();

            entityIdToPeerIdConverter = ServerComponents.Container.GetComponent<EntityIdToPeerIdConverter>().AssertNotNull() as EntityIdToPeerIdConverter;
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }

        private void CreateEntity()
        {
            var entityContainer = ServerComponents.Container.GetComponent<EntityContainer>().AssertNotNull() as EntityContainer;
            entity = entityContainer.CreateEntity(EntityType.Player);

            AddPeerIdToEntityIdConverter(entity.Id);
        }

        private void AddPeerIdToEntityIdConverter(int entityId)
        {
            entityIdToPeerIdConverter.AddEntityIdToPeerId(entityId, PeerId);
        }

        private void RemovePeerIdFromEntityIdConverter(int entityId)
        {
            entityIdToPeerIdConverter.RemoveEntityIdToPeerId(entityId);
        }

        protected override void PeerDisconnectionNotifier(DisconnectReason disconnectReason, string s)
        {
            RemovePeerIdFromEntityIdConverter(entity.Id);

            base.PeerDisconnectionNotifier(disconnectReason, s);
        }
    }
}