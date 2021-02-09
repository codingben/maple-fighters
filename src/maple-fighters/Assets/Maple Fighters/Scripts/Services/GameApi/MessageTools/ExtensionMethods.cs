using System;

namespace Game.MessageTools
{
    public static class ExtensionMethods
    {
        public static IMessageHandler<T> ToMessageHandler<T>(this Action<T> action)
            where T : struct
        {
            return new MessageHandler<T>(action);
        }
    }
}