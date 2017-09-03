using System;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public abstract class PeerLogicWrapper<TOperationCode, TEventCode> : PeerLogicBase<TOperationCode, TEventCode>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IComponentsContainer ComponentsContainer { get; } = new ComponentsContainer();

        protected PeerLogicWrapper(IClientPeer peer, int peerId) 
            : base(peer, peerId)
        {
            // Left blank intentionally
        }

        protected void AddCommonComponents()
        {
            ComponentsContainer.AddComponent(new CoroutinesExecutor(new FiberCoroutinesExecutor(Peer.Fiber, 100)));
        }
    }
}