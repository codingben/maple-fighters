using System;
using System.Collections.Generic;

namespace Game.Network
{
    public class MessageHandlerCollection : IMessageHandlerCollection
    {
        private readonly IJsonSerializer jsonSerializer;
        private readonly IDictionary<byte, Action<string>> collection;

        public MessageHandlerCollection(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;

            collection = new Dictionary<byte, Action<string>>();
        }

        public void Set<TMessageCode, TMessage>(TMessageCode messageCode, IMessageHandler<TMessage> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible
            where TMessage : class
        {
            var key = Convert.ToByte(messageCode);

            collection[key] = data =>
            {
                var message = jsonSerializer.Deserialize<TMessage>(data);
                if (message != null)
                {
                    handler.Handle(message);
                }
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

        public bool TryGet<TMessageCode>(TMessageCode messageCode, out Action<string> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible
        {
            var key = Convert.ToByte(messageCode);

            return collection.TryGetValue(key, out handler);
        }
    }
}