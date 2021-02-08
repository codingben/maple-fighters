using System;

namespace Game.Network
{
    public class MessageHandler<T> : IMessageHandler<T>
        where T : class
    {
        private readonly Action<T> handler;

        public MessageHandler(Action<T> handler)
        {
            this.handler = handler;
        }

        public void Handle(T message)
        {
            handler?.Invoke(message);
        }
    }
}