using System;

namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        Action<byte[]> SendMessageAction { set; }

        Action<byte[], int> SendToMessageAction { set; }

        void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class;

        void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class;
    }
}