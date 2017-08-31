using CommonCommunicationInterfaces;
using CommonTools.Log;
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

        public EnterWorldOperation(int peerId, int entityId)
        {
            this.peerId = peerId;
            this.entityId = entityId;
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var peerContainer = ServerComponents.Container.GetComponent<PeerContainer>().AssertNotNull() as PeerContainer;
            var parameters = new EntityInitialInfomraitonEventParameters(new Shared.Game.Common.Entity(entityId, EntityType.Player));

            peerContainer.GetPeerWrapper(peerId)?.AssertNotNull().SendEvent(
                (byte)GameEvents.EntityInitialInformation, parameters, MessageSendOptions.DefaultReliable());

            return null;
        }
    }
}