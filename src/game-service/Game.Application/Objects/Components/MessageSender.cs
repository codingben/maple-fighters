using System;
using Common.ComponentModel;
using Game.Application.Network;

namespace Game.Application.Objects.Components
{
    public class MessageSender : ComponentBase, IMessageSender
    {
        private Action<byte[]> sendMessage;
        private Action<byte[], int> sendToMessage;

        private IProximityChecker proximityChecker;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void Initialize(Action<byte[]> sendMessage, Action<byte[], int> sendToMessage)
        {
            this.sendMessage = sendMessage;
            this.sendToMessage = sendToMessage;
        }

        public void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var rawData = MessageUtils.WrapMessage(code, message);

            sendMessage?.Invoke(rawData);
        }

        public void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var nearbyGameObjects = proximityChecker.GetNearbyGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var rawData = MessageUtils.WrapMessage(code, message);

                sendToMessage?.Invoke(rawData, gameObject.Id);
            }
        }
    }
}