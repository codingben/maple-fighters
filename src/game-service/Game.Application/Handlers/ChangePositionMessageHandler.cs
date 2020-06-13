using System;
using Common.MathematicsHelper;
using InterestManagement;
using Game.Application.Objects.Components;
using Game.Application.Network;
using Game.Application.Messages;

namespace Game.Application.Handlers
{
    public class ChangePositionMessageHandler : IMessageHandler
    {
        private readonly ITransform transform;
        private readonly IProximityChecker proximityChecker;
        private readonly Action<byte[], int> sendMessageCallback;

        public ChangePositionMessageHandler(
            ITransform transform,
            IProximityChecker proximityChecker,
            Action<byte[], int> sendMessageCallback)
        {
            this.transform = transform;
            this.proximityChecker = proximityChecker;
            this.sendMessageCallback = sendMessageCallback;
        }

        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.FromMessage<ChangePositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);

            transform.SetPosition(position);

            SendMessageToNearbyGameObjects(position);
        }

        private void SendMessageToNearbyGameObjects(Vector2 position)
        {
            var nearbyGameObjects = proximityChecker.GetGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = gameObject.Id,
                    X = position.X,
                    Y = position.Y
                };
                var rawData = MessageUtils.WrapMessage((byte)MessageCodes.PositionChanged, message);

                sendMessageCallback(rawData, gameObject.Id);
            }
        }
    }
}