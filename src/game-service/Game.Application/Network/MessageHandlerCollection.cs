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

        public void Set<T, M>(T messageCode, IMessageHandler<M> handler)
            where T : IComparable, IFormattable, IConvertible
            where M : class
        {
            var key = Convert.ToByte(messageCode);

            collection[key] = data =>
            {
                var message = jsonSerializer.Deserialize<M>(data);
                if (message != null)
                {
                    handler.Handle(message);
                }
            };
        }

        public void Unset<TMessageCode>(T messageCode)
            where T : IComparable, IFormattable, IConvertible
        {
            var key = Convert.ToByte(messageCode);

            if (collection.ContainsKey(key))
            {
                collection.Remove(key);
            }
        }

        public bool TryGet<T>(T messageCode, out Action<string> handler)
            where T : IComparable, IFormattable, IConvertible
        {
            var key = Convert.ToByte(messageCode);

            return collection.TryGetValue(key, out handler);
        }
    }
}