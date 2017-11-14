using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class UpdatePlayerStateOperationHandler : IOperationRequestHandler<UpdatePlayerStateRequestParameters, EmptyParameters>
    {
        private readonly int sceneObjectId;
        private readonly IInterestAreaManagement interestAreaManagement;
        private readonly ISceneObject sceneObject;
        
        public UpdatePlayerStateOperationHandler(int sceneObjectId, IInterestAreaManagement interestAreaManagement, ICharacterGetter characterGetter)
        {
            this.sceneObjectId = sceneObjectId;
            this.interestAreaManagement = interestAreaManagement;

            sceneObject = characterGetter.GetSceneObject();
        }

        public EmptyParameters? Handle(MessageData<UpdatePlayerStateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var playerState = messageData.Parameters.PlayerState;

            var characterBody = sceneObject.Container.GetComponent<ICharacterBody>().AssertNotNull();
            if (characterBody != null)
            {
                characterBody.PlayerState = playerState;
            }

            var parameters = new PlayerStateChangedEventParameters(playerState, sceneObjectId);
            var messageSendOptions = MessageSendOptions.DefaultReliable((byte)GameDataChannels.Animations);
            interestAreaManagement.SendEventForSubscribers((byte)GameEvents.PlayerStateChanged, parameters, messageSendOptions);
            return null;
        }
    }
}