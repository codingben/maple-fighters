using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
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
    public sealed class OutboundPeerLogic<TOperationCode, TEventCode> : 
        PeerLogicBase<IOutboundServerPeer>,
        IOutboundPeerLogic<TOperationCode, TEventCode>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private IOperationRequestSender<TOperationCode> OperationRequestSender
        {
            get;
            set;
        }

        private IEventHandlerRegister<TEventCode> EventHandlerRegister
        {
            get;
            set;
        }

        private IOperationResponseSubscriptionProvider SubscriptionProvider
        {
            get;
            set;
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

        public void SendOperation<TParameters>(
            TOperationCode code,
            TParameters parameters)
            where TParameters : struct, IParameters
        {
            OperationRequestSender.Send(
                code,
                parameters,
                MessageSendOptions.DefaultReliable());
        }

        public async Task<TResponseParameters>
            SendOperationAsync<TRequestParameters, TResponseParameters>(
                IYield yield,
                TOperationCode code,
                TRequestParameters parameters)
            where TRequestParameters : struct, IParameters
            where TResponseParameters : struct, IParameters
        {
            var id = OperationRequestSender.Send(
                code,
                parameters,
                MessageSendOptions.DefaultReliable());

            return await SubscriptionProvider
                       .ProvideSubscription<TResponseParameters>(yield, id);
        }

        public void SetEventHandler<TEventParameters>(
            TEventCode code,
            Action<TEventParameters> action)
            where TEventParameters : struct, IParameters
        {
            var eventHandler =
                new EventHandler<TEventParameters>(
                    x => action.Invoke(x.Parameters));
            EventHandlerRegister.SetHandler(code, eventHandler);
        }

        public void RemoveEventHandler(TEventCode code)
        {
            EventHandlerRegister.RemoveHandler(code);
        }

        private void OnOperationRequestFailed(
            RawMessageResponseData data,
            short requestId)
        {
            LogUtils.Log(
                $"Failed to send operation. Peer Id: {PeerId} Code: {data.Code}");
        }
    }
}