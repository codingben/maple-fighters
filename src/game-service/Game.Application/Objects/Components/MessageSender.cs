using System;
using Common.ComponentModel;
using Game.Application.Network;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class MessageSender : ComponentBase, IMessageSender
    {
        private readonly Action<byte[]> sendMessage;
        private readonly Action<byte[], int> sendToMessage;

        private IProximityChecker proximityChecker;

        public MessageSender(Action<byte[]> sendMessage, Action<byte[], int> sendToMessage)
        {
            this.sendMessage = sendMessage;
            this.sendToMessage = sendToMessage;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
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