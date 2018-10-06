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
        protected IServerPeerLogic<TOperationCode, TEventCode> ServerPeerLogic
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

            var s2sConnectionProvider = ServerComponents.Get<IS2sConnectionProvider>()
                .AssertNotNull();
            s2sConnectionProvider.Connect(
                GetConnectionDetails(),
                OnConnectionEstablished,
                OnConnectionClosed);
        }

        public void Dispose()
        {
            ServerPeerLogic?.Dispose();

            var s2sConnectionProvider = ServerComponents.Get<IS2sConnectionProvider>()
                .AssertNotNull();
            s2sConnectionProvider.Disconnect();
        }

        protected virtual void OnConnectionEstablished(
            IOutboundServerPeer serverPeer)
        {
            ServerPeerLogic =
                new CommonServerPeerLogic<TOperationCode, TEventCode>();

            var @base = (IPeerLogicBase<IOutboundServerPeer>)ServerPeerLogic;
            @base?.Setup(serverPeer, default(int));

            var connectionDetails = GetConnectionDetails();
            LogUtils.Log(
                $"A connection with the server {connectionDetails.Ip}:{connectionDetails.Port} has been established successfully.");
        }

        protected virtual void OnConnectionClosed(
            DisconnectReason disconnectReason,
            string reason)
        {
            ServerPeerLogic.Dispose();

            var connectionDetails = GetConnectionDetails();
            LogUtils.Log(
                $"A connection with the server {connectionDetails.Ip}:{connectionDetails.Port} has been closed.");
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