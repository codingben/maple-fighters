using CommonCommunicationInterfaces;
using Game.InterestManagement;
using MathematicsHelper;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePositionOperationHandler : IOperationRequestHandler<UpdatePositionRequestParameters, EmptyParameters>
    {
        private readonly ITransform transform;
        private readonly IOrientationProvider orientationProvider;

        public UpdatePositionOperationHandler(ITransform transform, IOrientationProvider orientationProvider)
        {
            this.transform = transform;
            this.orientationProvider = orientationProvider;
        }

        public EmptyParameters? Handle(MessageData<UpdatePositionRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            orientationProvider.Direction = (Direction)messageData.Parameters.Direction.FromDirections();

            var newPosition = new Vector2(messageData.Parameters.X, messageData.Parameters.Y);
            transform.SetPosition(newPosition);
            return null;
        }
    }
}