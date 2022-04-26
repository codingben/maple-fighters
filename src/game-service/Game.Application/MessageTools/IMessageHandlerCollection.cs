using System;

namespace Game.MessageTools
{
    public interface IMessageHandlerCollection : IDisposable
    {
        void Set<T, M>(T messageCode, IMessageHandler<M> handler)
            where T : IComparable, IFormattable, IConvertible
            where M : struct;

        void Unset<T>(T messageCode)
            where T : IComparable, IFormattable, IConvertible;

        bool TryGet<T>(T messageCode, out Action<string> handler)
            where T : IComparable, IFormattable, IConvertible;
    }
}