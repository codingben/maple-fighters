using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using ServerApplication.Common.Components.Interfaces;
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
                LogUtils.Log($"An attempt to access outbound server peer logic but there is no initialized peer to: {peerDetails.Ip}:{peerDetails.Port}");
                return null;
            }
        }
        private IOutboundServerPeerLogic outboundServerPeerLogic;
        private IOutboundServerPeerLogicBase serverAuthenticationPeerLogic;
        private IServiceConnectorProvider serviceConnectorProvider;

        private bool disposed;
        private bool isAuthenticated;

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
            serverAuthenticationPeerLogic?.Dispose();
            outboundServerPeerLogic?.Dispose();
        }
        
        private void Authenticated(IOutboundServerPeer outboundServerPeer)
        {
            serverAuthenticationPeerLogic.Dispose();

            outboundServerPeerLogic = new OutboundServerPeerLogicBase<TOperationCode, TEventCode>(outboundServerPeer);
            outboundServerPeerLogic.Initialize();

            isAuthenticated = true;

            OnAuthenticated();
        }

        protected virtual void OnAuthenticated()
        {
            // Left blank intentionally
        }

        private void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            var secretKey = GetSecretKey();
            serverAuthenticationPeerLogic = new ServerAuthenticationPeerLogic(outboundServerPeer, secretKey, onAuthenticated: () => Authenticated(outboundServerPeer));
            serverAuthenticationPeerLogic.Initialize();

            SubscribeToDisconnectionNotifier();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been established successfully.");

            serviceConnectorProvider.SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        protected virtual void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            serviceConnectorProvider?.Dispose();
            outboundServerPeerLogic?.Dispose();
            serverAuthenticationPeerLogic?.Dispose();

            var peerConnectionInformation = GetPeerConnectionInformation();
            LogUtils.Log($"A connection with the server {peerConnectionInformation.Ip}:{peerConnectionInformation.Port} has been closed.");

            if (!disposed && isAuthenticated)
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
            if (serviceConnectorProvider.PeerDisconnectionNotifier != null)
            {
                serviceConnectorProvider.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
            }
        }

        protected abstract PeerConnectionInformation GetPeerConnectionInformation();
        protected abstract string GetSecretKey();
    }
}