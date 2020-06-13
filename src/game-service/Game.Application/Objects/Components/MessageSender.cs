using System;
using Common.ComponentModel;
using Game.Application.Network;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class MessageSender : ComponentBase, IMessageSender
    {
        private readonly Action<byte[], int> sendMessageCallback;
        private IProximityChecker proximityChecker;

        public MessageSender(Action<byte[], int> sendMessageCallback)
        {
            this.sendMessageCallback = sendMessageCallback;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var nearbyGameObjects = proximityChecker.GetGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var rawData = MessageUtils.WrapMessage(code, message);

                sendMessageCallback(rawData, gameObject.Id);
            }
        }
    }
}