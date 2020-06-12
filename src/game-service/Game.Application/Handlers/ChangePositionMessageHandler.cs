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
        private readonly IProximityChecker prxomitiyChecker;
        private readonly Action<byte[], int> sendMessageCallback;

        public ChangePositionMessageHandler(
            ITransform transform,
            IProximityChecker prxomitiyChecker,
            Action<byte[], int> sendMessageCallback)
        {
            this.transform = transform;
            this.prxomitiyChecker = prxomitiyChecker;
            this.sendMessageCallback = sendMessageCallback;
        }

        public void Handle(byte[] rawData)
        {
            var message = MessageUtils.FromMessage<ChangePlayerPositionMessage>(rawData);
            var x = message.X;
            var y = message.Y;
            var position = new Vector2(x, y);

            transform.SetPosition(position);

            SendMessageToNearbyGameObjects(position);
        }

        private void SendMessageToNearbyGameObjects(Vector2 position)
        {
            var nearbyGameObjects = prxomitiyChecker.GetGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = gameObject.Id,
                    X = position.X,
                    Y = position.Y
                };
                var rawData = MessageUtils.ToMessage(message);

                sendMessageCallback(rawData, gameObject.Id);
            }
        }
    }
}