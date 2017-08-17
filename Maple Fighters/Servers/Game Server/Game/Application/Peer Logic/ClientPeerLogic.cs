using Game.Application.PeerLogic.Operations;
using Game.Entities;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class ClientPeerLogic : PeerLogic<GameOperations, GameEvents>
    {
        private IEntity entity;

        public ClientPeerLogic(IClientPeer peer) 
            : base(peer)
        {
            ServerComponents.Container.GetComponent(out EntityContainer entityContainer);
            entity = entityContainer.CreateEntity(EntityType.Player);

            SetOperationsHandlers();
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }
    }
}