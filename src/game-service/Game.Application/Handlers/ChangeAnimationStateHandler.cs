using System;
using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler
    {
        private readonly IAnimationData animationData;
        private readonly IProximityChecker proximityChecker;
        private readonly Action<byte[], int> sendMessageCallback;

        public ChangeAnimationStateHandler(
            IAnimationData animationData,
            IProximityChecker proximityChecker,
            Action<byte[], int> sendMessageCallback)
        {
            this.animationData = animationData;
            this.proximityChecker = proximityChecker;
            this.sendMessageCallback = sendMessageCallback;
        }

        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.FromMessage<ChangeAnimationStateMessage>(rawData);
            var animationState = message.AnimationState;

            animationData.SetAnimationState(animationState);

            SendMessageToNearbyGameObjects(animationState);
        }

        private void SendMessageToNearbyGameObjects(byte animationState)
        {
            var nearbyGameObjects = proximityChecker.GetGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var message = new AnimationStateChangedMessage()
                {
                    GameObjectId = gameObject.Id,
                    AnimationState = animationState
                };
                var rawData = MessageUtils.WrapMessage((byte)MessageCodes.AnimationStateChanged, message);

                sendMessageCallback(rawData, gameObject.Id);
            }
        }
    }
}