using System;

namespace Game.Network
{
    public interface IMessageHandlerCollection
    {
        void Set<TMessageCode, TMessage>(TMessageCode messageCode, IMessageHandler<TMessage> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible
            where TMessage : class;

        void Unset<TMessageCode>(TMessageCode messageCode)
            where TMessageCode : IComparable, IFormattable, IConvertible;

        bool TryGet<TMessageCode>(TMessageCode messageCode, out Action<byte[]> handler)
            where TMessageCode : IComparable, IFormattable, IConvertible;
    }
}