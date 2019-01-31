using System;
using PhotonClientImplementation;

namespace Scripts.Services
{
    public sealed class ServiceBase : IServiceBase
    {
        public IServiceConnectionHandler ServiceConnectionHandler { get; }

        private IPeerLogicBase peerLogicBase;
        private IDisposable serverPeerHandler;

        public ServiceBase()
        {
            ServiceConnectionHandler = new ServiceConnectionHandler();
        }

        public void SetPeerLogic<TPeerLogic, TOperationCode, TEventCode>()
            where TPeerLogic : IPeerLogicBase, new()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible
        {
            var peerHandler =
                new ServerPeerHandler<TOperationCode, TEventCode>();
            peerHandler.Initialize(
                ServiceConnectionHandler.ServerPeer ?? new PhotonPeer());

            serverPeerHandler?.Dispose();
            serverPeerHandler = peerHandler;

            peerLogicBase?.Dispose();
            peerLogicBase = new TPeerLogic();
            peerLogicBase.Awake(serverPeerHandler);
        }

        public TPeerLogic GetPeerLogic<TPeerLogic>()
            where TPeerLogic : IPeerLogicBase
        {
            if (peerLogicBase is TPeerLogic)
            {
                return (TPeerLogic)peerLogicBase;
            }

            return default(TPeerLogic);
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