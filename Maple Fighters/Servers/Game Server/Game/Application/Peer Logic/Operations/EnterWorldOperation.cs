using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Entity.Components;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperation : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        private readonly int peerId;
        private readonly int entityId;
        private readonly Transform transform;
        private readonly PeerContainer peerContainer;

        public EnterWorldOperation(int peerId, int entityId, Transform transform)
        {
            this.peerId = peerId;
            this.entityId = entityId;
            this.transform = transform;

            peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var entity = new Shared.Game.Common.Entity(entityId, EntityType.Player);
            var parameters = new EntityInitialInfomraitonEventParameters(entity, transform.Position.X, transform.Position.Y);

            peerContainer.GetPeerWrapper(peerId)?.AssertNotNull().SendEvent(
                (byte)GameEvents.EntityInitialInformation, parameters, MessageSendOptions.DefaultReliable());

            return null;
        }
    }
}