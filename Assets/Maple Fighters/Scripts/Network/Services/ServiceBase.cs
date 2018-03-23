using System;
using CommonTools.Log;

namespace Scripts.Services
{
    public sealed class ServiceBase : IServiceBase
    {
        public IServiceConnectionHandler ServiceConnectionHandler { get; }

        private IPeerLogicBase peerLogicBase;
        private IServerPeerHandler ServerPeerHandler { get; set; }

        public ServiceBase()
        {
            ServiceConnectionHandler = new ServiceConnectionHandler();
        }

        public void SetPeerLogic<T, TOperationCode, TEventCode>(T peerLogic)
            where T : IPeerLogicBase
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
            ServerPeerHandler?.Dispose();

            var serverPeerHandler = new ServerPeerHandler<TOperationCode, TEventCode>();
            serverPeerHandler.Initialize(ServiceConnectionHandler.ServerPeer);
            ServerPeerHandler = serverPeerHandler;

            peerLogicBase = peerLogic;
            peerLogicBase.Awake(ServerPeerHandler);
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
    }
}