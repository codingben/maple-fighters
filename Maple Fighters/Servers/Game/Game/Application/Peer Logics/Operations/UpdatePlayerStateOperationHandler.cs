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
        private readonly ISceneObject sceneObject;
        private readonly IInterestAreaNotifier interestAreaNotifier;
        
        public UpdatePlayerStateOperationHandler(int sceneObjectId, ICharacterGetter characterGetter)
        {
            this.sceneObjectId = sceneObjectId;

            sceneObject = characterGetter.GetSceneObject();
            interestAreaNotifier = sceneObject.Container.GetComponent<IInterestAreaNotifier>().AssertNotNull();
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
            interestAreaNotifier.NotifySubscribers((byte)GameEvents.PlayerStateChanged, parameters, messageSendOptions);
            return null;
        }
    }
}