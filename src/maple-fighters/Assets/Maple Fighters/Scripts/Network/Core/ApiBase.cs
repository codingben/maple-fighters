using System;

namespace Scripts.Services
{
    public class ApiBase<TOperationCode, TEventCode>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public ServerPeerHandler<TOperationCode, TEventCode> ServerPeer
        {
            get;
        }

        public ApiBase()
        {
            ServerPeer = new ServerPeerHandler<TOperationCode, TEventCode>();
        }
    }
}