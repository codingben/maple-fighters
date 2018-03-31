using CommonCommunicationInterfaces;
using Game.InterestManagement;
using MathematicsHelper;
using ServerCommunicationHelper;
using Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePositionOperationHandler : IOperationRequestHandler<UpdatePositionRequestParameters, EmptyParameters>
    {
        private readonly ITransform transform;

        public UpdatePositionOperationHandler(ITransform transform)
        {
            this.transform = transform;
        }

        public EmptyParameters? Handle(MessageData<UpdatePositionRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            transform.Direction = (Direction)messageData.Parameters.Direction.FromDirections();

            var newPosition = new Vector2(messageData.Parameters.X, messageData.Parameters.Y);
            transform.SetPosition(newPosition);
            return null;
        }
    }
}