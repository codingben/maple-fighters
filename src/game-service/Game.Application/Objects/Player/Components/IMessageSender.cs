using System;

namespace Game.Application.Objects.Components
{
    public interface IMessageSender
    {
        Action<string> SendMessageCallback { get; set; }

        Action<string, int> SendToMessageCallback { get; set; }

        void SendMessage<TMessage>(byte code, TMessage message)
            where TMessage : class;

        void SendMessageToNearbyGameObjects<TMessage>(byte code, TMessage message)
            where TMessage : class;
    }
}