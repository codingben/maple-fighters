using CommonCommunicationInterfaces;
using MathematicsHelper;
using ServerCommunicationHelper;
using Game.Common;
using InterestManagement;
using InterestManagement.Components.Interfaces;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePositionOperationHandler : IOperationRequestHandler<UpdatePositionRequestParameters, EmptyParameters>
    {
        private readonly IPositionTransform positionTransform;
        private readonly IDirectionTransform directionTransform;

        public UpdatePositionOperationHandler(IPositionTransform positionTransform, IDirectionTransform directionTransform)
        {
            this.positionTransform = positionTransform;
            this.directionTransform = directionTransform;
        }

        public EmptyParameters? Handle(MessageData<UpdatePositionRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            positionTransform.SetPosition(new Vector2(messageData.Parameters.X, messageData.Parameters.Y));
            directionTransform.SetDirection((Direction)messageData.Parameters.Direction.FromDirections());
            return null;
        }
    }
}