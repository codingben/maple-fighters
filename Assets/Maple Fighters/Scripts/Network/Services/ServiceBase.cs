using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;

namespace Scripts.Services
{
    public class ServiceBase : IServiceBase
    {
        public IServiceConnectionHandler ServiceConnectionHandler { get; private set; }
        public IServerPeerHandler ServerPeerHandler { get; private set; }

        public void SetServerPeerHandler<TOperationCode, TEventCode>()
            where TOperationCode : IComparable, IFormattable, IConvertible
            where TEventCode : IComparable, IFormattable, IConvertible
        {
            ServerPeerHandler?.Dispose();

            var serverPeerHandler = new ServerPeerHandler<TOperationCode, TEventCode>();
            ServerPeerHandler = serverPeerHandler;

            if (ServiceConnectionHandler != null)
            {
                serverPeerHandler.Initialize(ServiceConnectionHandler.ServerPeer);
                OnServerPeerHandlerChanged<TEventCode>();
            }
            else
            {
                ServiceConnectionHandler = new ServiceConnectionHandler(onConnected: (serverPeer) =>
                {
                    serverPeerHandler.Initialize(serverPeer);
                    OnConnected();
                });
            }
        }

        protected virtual void OnConnected()
        {
            SubscribeToDisconnectionNotifier();

            var serverType = ServiceConnectionHandler.ServerConnectionInformation.ServerType;
            var ip = ServiceConnectionHandler.ServerConnectionInformation.PeerConnectionInformation.Ip;
            var port = ServiceConnectionHandler.ServerConnectionInformation.PeerConnectionInformation.Port;
            LogUtils.Log($"A {serverType} server has been connected: {ip}:{port}");
        }

        protected virtual void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            var ip = ServiceConnectionHandler.ServerConnectionInformation.PeerConnectionInformation.Ip;
            var port = ServiceConnectionHandler.ServerConnectionInformation.PeerConnectionInformation.Port;
            LogUtils.Log($"The connection has been closed with {ip}:{port}. Reason: {reason}");
        }

        protected virtual void OnServerPeerHandlerChanged<TEventCode>()
            where TEventCode : IComparable, IFormattable, IConvertible
        {
            // Left blank intentionally
        }

        private void SubscribeToDisconnectionNotifier()
        {
            ServiceConnectionHandler.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            ServiceConnectionHandler.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }
    }
}