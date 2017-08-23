using CommonCommunicationInterfaces;
using Game.Entity.Components;
using MathematicsHelper;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdateEntityPositionOperation : IOperationRequestHandler<UpdateEntityPositionRequestParameters, EmptyParameters>
    {
        private readonly Transform entityTransform;

        public UpdateEntityPositionOperation(Transform entityTransform)
        {
            this.entityTransform = entityTransform;
        }

        public EmptyParameters? Handle(MessageData<UpdateEntityPositionRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var newPosition = new Vector2(messageData.Parameters.X, messageData.Parameters.Y);
            entityTransform.SetPosition(newPosition);

            return new EmptyParameters();
        }
    }
}