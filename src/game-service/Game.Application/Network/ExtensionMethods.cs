using System;

namespace Game.Network
{
    public static class ExtensionMethods
    {
        public static IMessageHandler<T> ToMessageHandler<T>(this Action<T> action)
            where T : class
        {
            return new MessageHandler<T>(action);
        }
    }
}