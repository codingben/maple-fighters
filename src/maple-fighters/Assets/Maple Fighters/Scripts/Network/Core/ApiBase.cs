using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public class ApiBase<TOperationCode, TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected ServerPeerHandler<TOperationCode, TEventCode> ServerPeerHandler
        {
            get;
        }

        public ApiBase()
        {
            ServerPeerHandler =
                new ServerPeerHandler<TOperationCode, TEventCode>();
        }

        public void SetServerPeer(IServerPeer serverPeer)
        {
            ServerPeerHandler.Initialize(serverPeer);
        }

        public void Dispose()
        {
            ServerPeerHandler.Dispose();
        }
    }
}