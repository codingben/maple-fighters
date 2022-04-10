using System;
using Game.MessageTools;

namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        Action<string> SendMessageCallback { get; set; }

        Action<string, int> SendToMessageCallback { get; set; }

        void SetJsonSerializer(IJsonSerializer jsonSerializer);

        void SendMessage<T>(byte code, T message)
            where T : struct;

        void SendMessageToNearbyGameObjects<T>(byte code, T message)
            where T : struct;
    }
}