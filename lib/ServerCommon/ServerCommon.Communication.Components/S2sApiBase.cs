using System;
using Common.ComponentModel;
using Common.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommon.Communication.PeerLogic;
using ServerCommon.PeerLogic;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication.Components
{
    public abstract class S2sApiBase<TOperationCode, TEventCode> : IComponent
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOutboundPeerLogic<TOperationCode, TEventCode> ServerPeerLogic
        {
            get;
            private set;
        }

        protected IComponents ServerComponents
        {
            get;
            private set;
        }

        protected int PeerId => ProvidePeerId();

        public void Awake(IComponents components)
        {
            ServerComponents = components;

            var s2sConnectionProvider = ServerComponents
                .Get<IS2sConnectionProvider>()
                .AssertNotNull();
            s2sConnectionProvider.Connect(
                GetConnectionDetails(),
                OnConnectionEstablished,
                OnConnectionClosed);
        }

        public void Dispose()
        {
            ServerPeerLogic?.Dispose();

            var s2sConnectionProvider = ServerComponents
                .Get<IS2sConnectionProvider>()
                .AssertNotNull();
            s2sConnectionProvider.Disconnect();
        }

        protected virtual void OnConnectionEstablished(
            IOutboundServerPeer serverPeer)
        {
            ServerPeerLogic =
                new OutboundPeerLogic<TOperationCode, TEventCode>();

            var @base = (IPeerLogicBase<IOutboundServerPeer>)ServerPeerLogic;
            @base?.Setup(serverPeer, default);

            var connectionDetails = GetConnectionDetails();
            var ip = connectionDetails.Ip;
            var port = connectionDetails.Port;
            LogUtils.Log($"Connected to {ip}:{port} server.");
        }

        protected virtual void OnConnectionClosed(
            DisconnectReason disconnectReason,
            string reason)
        {
            ServerPeerLogic.Dispose();

            var connectionDetails = GetConnectionDetails();
            var ip = connectionDetails.Ip;
            var port = connectionDetails.Port;
            LogUtils.Log($"Disconnected from the {ip}:{port} server.");
        }

        private int ProvidePeerId()
        {
            var idGenerator =
                ServerComponents.Get<IIdGenerator>().AssertNotNull();
            return idGenerator.GenerateId();
        }

        protected abstract PeerConnectionInformation GetConnectionDetails();
    }
}