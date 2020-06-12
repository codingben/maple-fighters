using System;
using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class ChangeAnimationStateHandler : IMessageHandler
    {
        private readonly IAnimationData animationData;
        private readonly IProximityChecker prxomitiyChecker;
        private readonly Action<byte[], int> sendMessageCallback;

        public ChangeAnimationStateHandler(
            IAnimationData animationData,
            IProximityChecker prxomitiyChecker,
            Action<byte[], int> sendMessageCallback)
        {
            this.animationData = animationData;
            this.prxomitiyChecker = prxomitiyChecker;
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
            var nearbyGameObjects = prxomitiyChecker.GetGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var messageData = new MessageData()
                {
                    Code = (byte)MessageCodes.PositionChanged,
                    RawData = MessageUtils.ToMessage(new AnimationStateChangedMessage()
                    {
                        GameObjectId = gameObject.Id,
                        AnimationState = animationState
                    })
                };
                var rawData = MessageUtils.ToMessage(messageData);

                sendMessageCallback(rawData, gameObject.Id);
            }
        }
    }
}