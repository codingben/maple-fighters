using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;

namespace ServerCommunication.Common
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : Component
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOutboundServerPeerLogic OutboundServerPeerLogic
        {
            get
            {
                if (outboundServerPeerLogic != null && serviceConnectorProvider != null)
                {
                    return outboundServerPeerLogic;
                }

                var peerDetails = GetPeerConnectionInformation();
                LogUtils.Log($"An attempt to access outbound server peer logic but there is no connection to: {peerDetails.Ip}:{peerDetails.Port}");
                return null;
            }
        }
        private IOutboundServerPeerLogic outboundServerPeerLogic;
        private IServiceConnectorProvider serviceConnectorProvider;

        private bool disposed;

        protected override void OnAwake()
        {
            base.OnAwake();

            var coroutinesExecutor = Server.Components.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            var serverConnectorProvider = Components.GetComponent<IServerConnectorProvider>().AssertNotNull();
            serviceConnectorProvider = new ServiceConnectorProvider(coroutinesExecutor, serverConnectorProvider, OnConnected);
            serviceConnectorProvider.Connect(GetPeerConnectionInformation());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            disposed = true;
            
            serviceConnectorProvider?.Dispose();
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

        protected virtual void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"Disconnected from a server - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");

            if (!disposed)
            {
                serviceConnectorProvider.Connect(GetPeerConnectionInformation());
            }
        }

        private void SubscribeToDisconnectionNotifier()
        {
            serviceConnectorProvider.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            serviceConnectorProvider.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        protected abstract PeerConnectionInformation GetPeerConnectionInformation();
    }
}