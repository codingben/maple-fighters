using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using Components.Common.Interfaces;
using ServerApplication.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public abstract class ServiceBase : IComponent
    {
        protected IOutboundServerPeer OutboundServerPeer { get; private set; }
        private IServiceConnectionProvider serviceConnectionProvider;
        private bool disposed;

        public void Awake(IContainer components)
        {
            var coroutinesManager = components.GetComponent<ICoroutinesManager>().AssertNotNull();
            var serverConnectorProvider = components.GetComponent<IServerConnectorProvider>().AssertNotNull();
            serviceConnectionProvider = new ServiceConnectionProvider(coroutinesManager, serverConnectorProvider, OnConnected);
            serviceConnectionProvider.Connect(GetPeerConnectionInformation());
        }

        public void Dispose()
        {
            disposed = true;

            serviceConnectionProvider?.Dispose();
            serviceConnectionProvider?.Disconnect();
        }

        private void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            OutboundServerPeer = outboundServerPeer;

            SubscribeToDisconnectionNotifier();

            OnConnectionEstablished();
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            OnConnectionClosed(disconnectReason);
        }

        private void SubscribeToDisconnectionNotifier()
        {
            serviceConnectionProvider.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            if (serviceConnectionProvider.PeerDisconnectionNotifier != null)
            {
                serviceConnectionProvider.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
            }
        }

        protected virtual void OnConnectionEstablished()
        {
            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been established successfully.");
        }

        protected virtual void OnConnectionClosed(DisconnectReason disconnectReason)
        {
            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with the server {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been closed.");

            var isManuallyDisconnected = disconnectReason == DisconnectReason.ServerDisconnect; // For example, if a connection is not authenticated
            if (!disposed && !isManuallyDisconnected)
            {
                serviceConnectionProvider.Connect(GetPeerConnectionInformation());
            }
        }

        protected abstract PeerConnectionInformation GetPeerConnectionInformation();
    }
}