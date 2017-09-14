using CommonCommunicationInterfaces;
using Game.InterestManagement;
using MathematicsHelper;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdateEntityPositionOperationHandler : IOperationRequestHandler<UpdateEntityPositionRequestParameters, EmptyParameters>
    {
        private readonly Transform transform;

        public UpdateEntityPositionOperationHandler(Transform transform)
        {
            this.transform = transform;
        }

        public EmptyParameters? Handle(MessageData<UpdateEntityPositionRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var newPosition = new Vector2(messageData.Parameters.X, messageData.Parameters.Y);
            var direction = messageData.Parameters.Direction;

            transform.SetPosition(newPosition, direction);
            return null;
        }
    }
}