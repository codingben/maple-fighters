using System;

namespace Game.Network
{
    public static class ExtensionMethods
    {
        public static IMessageHandler<TMessage> ToMessageHandler<TMessage>(this Action<TMessage> action)
            where TMessage : class
        {
            return new MessageHandler<TMessage>(action);
        }
    }
}