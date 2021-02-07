using System;
using Common.ComponentModel;
using Game.Network;

namespace Game.Application.Objects.Components
{
    public class MessageSender : ComponentBase, IMessageSender
    {
        public Action<string> SendMessageCallback { get; set; }

        public Action<string, int> SendToMessageCallback { get; set; }

        private IProximityChecker proximityChecker;
        private IJsonSerializer jsonSerializer;

        public MessageSender(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var data = jsonSerializer.Serialize(new MessageData()
            {
                Code = code,
                Data = jsonSerializer.Serialize(message)
            });

            SendMessageCallback?.Invoke(data);
        }

        public void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class
        {
            var nearbyGameObjects = proximityChecker.GetNearbyGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var data = jsonSerializer.Serialize(new MessageData()
                {
                    Code = code,
                    Data = jsonSerializer.Serialize(message)
                });

                SendToMessageCallback?.Invoke(data, gameObject.Id);
            }
        }
    }
}