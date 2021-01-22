using System;

namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        Action<byte[]> SendMessageCallback { get; set; }

        Action<byte[], int> SendToMessageCallback { get; set; }

        void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class;

        void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class;
    }
}