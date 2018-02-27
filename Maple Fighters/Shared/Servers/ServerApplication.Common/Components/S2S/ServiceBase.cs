using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : Component
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected OutboundServerPeerLogic<TOperationCode, TEventCode> OutboundServerPeerLogic
        {
            get
            {
                if (outboundServerPeerLogic != null && (serviceConnectorProvider != null && serviceConnectorProvider.IsConnected()))
                {
                    return outboundServerPeerLogic;
                }

                var peerDetails = GetPeerConnectionInformation();
                LogUtils.Log($"An attempt to access outbound server peer logic but there is no connection to: {peerDetails.Ip}:{peerDetails.Port}");
                return null;
            }
        }
        private OutboundServerPeerLogic<TOperationCode, TEventCode> outboundServerPeerLogic;
        private ServiceConnectorProvider serviceConnectorProvider;

        private bool disposed;

        protected override void OnAwake()
        {
            base.OnAwake();

            var coroutinesExecutor = Server.Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            var serverConnectorProvider = Components.GetComponent<IServerConnectorProvider>().AssertNotNull();
            serviceConnectorProvider = new ServiceConnectorProvider(coroutinesExecutor, serverConnectorProvider);
            serviceConnectorProvider.Connect(GetPeerConnectionInformation(), OnConnected);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            disposed = true;

            if (serviceConnectorProvider.IsConnected())
            {
                serviceConnectorProvider.Disconnect();
            }

            outboundServerPeerLogic?.Dispose();
        }

        protected virtual void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            outboundServerPeerLogic = new OutboundServerPeerLogic<TOperationCode, TEventCode>(outboundServerPeer);

            SubscribeToDisconnectionNotifier();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been established successfully.");

            serviceConnectorProvider.SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        protected virtual void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            UnsubscribeFromDisconnectionNotifier();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"Disconnected from a server - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");

            if (!disposed)
            {
                serviceConnectorProvider.Connect(GetPeerConnectionInformation(), OnConnected);
            }
        }

        private void SubscribeToDisconnectionNotifier()
        {
            serviceConnectorProvider.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            if (serviceConnectorProvider?.PeerDisconnectionNotifier != null)
            {
                serviceConnectorProvider.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
            }
        }

        protected abstract PeerConnectionInformation GetPeerConnectionInformation();
    }
}