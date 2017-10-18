using CommonCommunicationInterfaces;
using Game.Application.PeerLogic.Components;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePlayerStateOperationHandler : IOperationRequestHandler<UpdatePlayerStateRequestParameters, EmptyParameters>
    {
        private readonly int sceneObjectId;
        private readonly IInterestAreaManagement interestAreaManagement;
        
        public UpdatePlayerStateOperationHandler(int sceneObjectId, IInterestAreaManagement interestAreaManagement)
        {
            this.sceneObjectId = sceneObjectId;
            this.interestAreaManagement = interestAreaManagement;
        }

        public EmptyParameters? Handle(MessageData<UpdatePlayerStateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var playerState = messageData.Parameters.PlayerState;
            var parameters = new PlayerStateChangedEventParameters(playerState, sceneObjectId);
            var messageSendOptions = MessageSendOptions.DefaultReliable((byte)GameDataChannels.Animations);
            interestAreaManagement.SendEventForSubscribers((byte)GameEvents.PlayerStateChanged, parameters, messageSendOptions);
            return null;
        }
    }
}