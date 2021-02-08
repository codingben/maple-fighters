using System;
using System.Collections.Generic;

namespace Game.MessageTools
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
            where M : struct
        {
            var key = Convert.ToByte(messageCode);

            collection[key] = data =>
            {
                var message = jsonSerializer.Deserialize<M>(data);
                handler.Handle(message);
            };
        }

        public void Unset<T>(T messageCode)
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