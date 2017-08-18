using CommonTools.Log;
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

        public ClientPeerLogic(IClientPeer peer, int peerId)
            : base(peer)
        {
            CreateEntity(peerId);
            SetOperationsHandlers();
        }

        private void CreateEntity(int peerId)
        {
            var entityContainer = ServerComponents.Container.GetComponent<EntityContainer>().AssertNotNull() as EntityContainer;
            entity = entityContainer.CreateEntity(peerId, EntityType.Player);
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }
    }
}