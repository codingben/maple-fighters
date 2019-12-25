using System;
using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Common
{
    /// <summary>
    /// A common implementation for the inbound connection which handles operations and events.
    /// </summary>
    /// <typeparam name="TOperationCode">The operations.</typeparam>
    /// <typeparam name="TEventCode">The events.</typeparam>
    public class InboundPeerLogicBase<TOperationCode, TEventCode> : PeerLogicBase<IClientPeer>, 
                                                                    IInboundPeerLogicBase
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        public IExposedComponents ExposedComponents =>
            Components.ProvideExposed();

        protected IComponents Components => 
            new ComponentsProvider();

        protected IOperationRequestHandlerRegister<TOperationCode> OperationHandlerRegister
        {
            get;
            private set;
        }

        protected override void OnSetup()
        {
            AddCommonComponents();

            var coroutinesManager =
                Components.Get<ICoroutinesExecutor>().AssertNotNull();

            OperationHandlerRegister =
                new OperationRequestsHandler<TOperationCode>(
                    Peer.OperationRequestNotifier,
                    Peer.OperationResponseSender,
                    ServerSettings.InboundPeer.Operations.LogRequests,
                    ServerSettings.InboundPeer.Operations.LogResponses,
                    coroutinesManager);
        }

        protected override void OnCleanup()
        {
            OperationHandlerRegister?.Dispose();
        }

        /// <summary>
        /// Adds common components:
        /// 1. <see cref="ICoroutinesExecutor"/>
        /// 2. <see cref="IEventSenderProvider"/>
        /// </summary>
        private void AddCommonComponents()
        {
            Components.Add(
                new CoroutinesExecutor(
                    new FiberCoroutinesExecutor(
                        Peer.Fiber,
                        updateRateMilliseconds: 100)));
            Components.Add(new EventSenderProvider<TEventCode>(
                Peer, 
                Peer.EventSender));
        }
    }
}