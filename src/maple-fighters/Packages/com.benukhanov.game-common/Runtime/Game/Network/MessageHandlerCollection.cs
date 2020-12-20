using System;
using System.Collections.Generic;

namespace Game.Network
{
    public class MessageHandlerCollection : IMessageHandlerCollection
    {
        private readonly Dictionary<byte, Action<byte[]>> collection;

        public MessageHandlerCollection()
        {
            collection = new Dictionary<byte, Action<byte[]>>();
        }

        public void Set<TMessageCode, TMessage>(TMessageCode messageCode, IMessageHandler<TMessage> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible
            where TMessage : class
        {
            var key = Convert.ToByte(messageCode);

            collection[key] = rawData =>
            {
                var message =
                    MessageUtils.DeserializeMessage<TMessage>(rawData);

                handler.Handle(message);
            };
        }

        public void Unset<TMessageCode>(TMessageCode messageCode)
            where TMessageCode : IComparable, IFormattable, IConvertible
        {
            var key = Convert.ToByte(messageCode);

            if (collection.ContainsKey(key))
            {
                collection.Remove(key);
            }
        }

        public bool TryGet<TMessageCode>(TMessageCode messageCode, out Action<byte[]> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible
        {
            var key = Convert.ToByte(messageCode);

            return collection.TryGetValue(key, out handler);
        }
    }
}