using System;

namespace Scripts.Services.Game
{
    public interface IGameApi
    {
        void SendMessage<TCode, TMessage>(TCode code, TMessage message)
            where TCode : IComparable, IFormattable, IConvertible
            where TMessage : class;
    }
}