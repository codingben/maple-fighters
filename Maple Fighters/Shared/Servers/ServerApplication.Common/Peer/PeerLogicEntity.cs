using System;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components.Coroutines;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public abstract class PeerLogicEntity<TOperationCode, TEventCode> : PeerLogicBase<TOperationCode, TEventCode>
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        protected IComponentsContainer ComponentsContainer { get; } = new ComponentsContainer();

        protected PeerLogicEntity(IClientPeer peer, int peerId) 
            : base(peer, peerId)
        {
            // Left blank intentionally
        }

        protected void AddCommonComponents()
        {
            ComponentsContainer.AddComponent(new CoroutinesExecuter(new FiberCoroutinesExecuter(Peer.Fiber, 100)));
        }
    }
}