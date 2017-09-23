using CommonCommunicationInterfaces;
using Game.Application.PeerLogic.Components;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePlayerStateOperationHandler : IOperationRequestHandler<UpdatePlayerStateRequestParameters, EmptyParameters>
    {
        private readonly int gameObjectId;
        private readonly InterestAreaManagement interestAreaManagement;
        
        public UpdatePlayerStateOperationHandler(int gameObjectId, InterestAreaManagement interestAreaManagement)
        {
            this.gameObjectId = gameObjectId;
            this.interestAreaManagement = interestAreaManagement;
        }

        public EmptyParameters? Handle(MessageData<UpdatePlayerStateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var playerState = messageData.Parameters.PlayerState;
            var parameters = new PlayerStateChangedEventParameters(playerState, gameObjectId);
            interestAreaManagement.SendEventForGameObjectsInMyRegions((byte)GameEvents.PlayerStateChanged, parameters, MessageSendOptions.DefaultReliable());
            return null;
        }
    }
}