using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;

namespace Scripts.Services
{
    public class ServiceBase<TOperationCode, TEventCode> : IServiceBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public IServiceConnectionHandler ServiceConnectionHandler { get; }
        protected IServerPeerHandler ServerPeerHandler { get; }

        protected ServiceBase()
        {
            var serverPeerHandler = new ServerPeerHandler<TOperationCode, TEventCode>();
            ServerPeerHandler = serverPeerHandler;

            Action<IServerPeer> onConnected = (serverPeer) => 
            {
                serverPeerHandler.Initialize(serverPeer);
                OnConnected();
            };

            ServiceConnectionHandler = new ServiceConnectionHandler(onConnected);
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