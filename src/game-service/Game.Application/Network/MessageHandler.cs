using System;

namespace Game.Network
{
    public class MessageHandler<TMessage> : IMessageHandler<TMessage>
        where TMessage : class
    {
        private readonly Action<TMessage> handler;

        public MessageHandler(Action<TMessage> handler)
        {
            this.handler = handler;
        }

        public void Handle(TMessage message)
        {
            handler?.Invoke(message);
        }
    }
}