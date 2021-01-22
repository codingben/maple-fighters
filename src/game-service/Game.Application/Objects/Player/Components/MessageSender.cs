using System;
using Common.ComponentModel;
using Game.Network;

namespace Game.Application.Objects.Components
{
    public class MessageSender : ComponentBase, IMessageSender
    {
        public Action<byte[]> SendMessageCallback { get; set; }

        public Action<byte[], int> SendToMessageCallback { get; set; }

        private IProximityChecker proximityChecker;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var rawData = MessageUtils.WrapMessage(code, message);

            SendMessageCallback?.Invoke(rawData);
        }

        public void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var nearbyGameObjects = proximityChecker.GetNearbyGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var rawData = MessageUtils.WrapMessage(code, message);

                SendToMessageCallback?.Invoke(rawData, gameObject.Id);
            }
        }
    }
}