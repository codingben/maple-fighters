using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using ServerCommon.Configuration;
using ServerCommon.PeerLogic;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication.PeerLogic
{
    /// <summary>
    /// A common implementation for the outbound connection which handles operations and events.
    /// </summary>
    /// <typeparam name="TOperationCode">The operations.</typeparam>
    /// <typeparam name="TEventCode">The events.</typeparam>
    public class OutboundPeerLogicBase<TOperationCode, TEventCode> : 
        PeerLogicBase<IOutboundServerPeer>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IOperationRequestSender<TOperationCode> OperationRequestSender
        {
            get;
            private set;
        }

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister
        {
            get;
            private set;
        }

        protected IOperationResponseSubscriptionProvider SubscriptionProvider
        {
            get;
            private set;
        }

        /// <inheritdoc />
        public override void OnSetup()
        {
            OperationRequestSender = new OperationRequestSender<TOperationCode>(
                Peer.OperationRequestSender,
                ServerSettings.OutboundPeer.Operations.LogRequests);

            SubscriptionProvider =
                new OperationResponseSubscriptionProvider<TOperationCode>(
                    Peer.OperationResponseNotifier,
                    OnOperationRequestFailed,
                    ServerSettings.OutboundPeer.Operations.LogResponses);

            EventHandlerRegister = new EventHandlerRegister<TEventCode>(
                Peer.EventNotifier,
                ServerSettings.OutboundPeer.LogEvents);
        }

        /// <inheritdoc />
        public override void OnCleanup()
        {
            EventHandlerRegister?.Dispose();
            SubscriptionProvider?.Dispose();
        }

        private void OnOperationRequestFailed(
            RawMessageResponseData data,
            short requestId)
        {
            LogUtils.Log(
                $"Sending an operation has been failed. Peer Id: {PeerId} Code: {data.Code}");
        }
    }
}