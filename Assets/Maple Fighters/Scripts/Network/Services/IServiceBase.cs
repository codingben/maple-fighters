using System;

namespace Scripts.Services
{
    public interface IServiceBase
    {
        IServiceConnectionHandler ServiceConnectionHandler { get; }
        IServerPeerHandler ServerPeerHandler { get; }

        void SetServerPeerHandler<TOperationCode, TEventCode>()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible;
    }
}