using System;
using CommonTools.Log;

namespace Scripts.Services
{
    public sealed class ServiceBase : IServiceBase
    {
        public IServiceConnectionHandler ServiceConnectionHandler { get; }

        private IPeerLogicBase peerLogicBase;
        private IServerPeerHandler serverPeerHandler;

        public ServiceBase()
        {
            ServiceConnectionHandler = new ServiceConnectionHandler();
        }

        public void SetPeerLogic<T, TOperationCode, TEventCode>()
            where T : IPeerLogicBase, new()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible
        {
            if (ServiceConnectionHandler.ServerPeer == null)
            {
                var peerLogicName = typeof(T).Name;
                LogUtils.Log($"There is no connection to a server. Peer Logic: {peerLogicName}");
                return;
            }

            peerLogicBase?.Dispose();
            serverPeerHandler?.Dispose();

            var peerHandler = new ServerPeerHandler<TOperationCode, TEventCode>();
            peerHandler.Initialize(ServiceConnectionHandler.ServerPeer);
            serverPeerHandler = peerHandler;

            peerLogicBase = new T();
            peerLogicBase.Awake(serverPeerHandler);
        }

        public T GetPeerLogic<T>()
            where T : IPeerLogicBase
        {
            if (peerLogicBase is T)
            {
                return (T)peerLogicBase;
            }

            var type = typeof(T).Name;
            LogUtils.Log(MessageBuilder.Trace($"Can not convert type {type}"));
            return default(T);
        }

        public void Dispose()
        {
            if (serverPeerHandler != null)
            {
                serverPeerHandler.Dispose();
                serverPeerHandler = null;
            }

            ServiceConnectionHandler?.Dispose();
        }
    }
}