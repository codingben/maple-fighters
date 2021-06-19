using System;

namespace Game.MessageTools
{
    public static class ExtensionMethods
    {
        public static IMessageHandler<T> ToMessageHandler<T>(this UnityEvent<T> action)
            where T : class
        {
            return new MessageHandler<T>(action);
        }
    }
}