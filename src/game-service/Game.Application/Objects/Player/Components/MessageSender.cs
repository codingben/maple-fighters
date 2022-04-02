using System;
using Game.Application.Components;
using Game.MessageTools;

namespace Game.Application.Objects.Components
{
    public class MessageSender : ComponentBase, IMessageSender
    {
        public Action<string> SendMessageCallback { get; set; }

        public Action<string, int> SendToMessageCallback { get; set; }

        private IProximityChecker proximityChecker;
        private readonly IJsonSerializer jsonSerializer;

        public MessageSender(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void SendMessage<T>(byte code, T message)
            where T : struct
        {
            var data = jsonSerializer.Serialize(new MessageData()
            {
                Code = code,
                Data = jsonSerializer.Serialize(message)
            });

            SendMessageCallback?.Invoke(data);
        }

        public void SendMessageToNearbyGameObjects<T>(byte code, T message)
            where T : struct
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