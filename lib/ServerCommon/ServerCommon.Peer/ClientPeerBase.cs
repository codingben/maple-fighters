using System;
using ServerCommunicationInterfaces;

namespace ServerCommon.Peer
{
    public abstract class ClientPeerBase<TO, TE> : IPeerBase<IClientPeer>
        where TO : IComparable, IFormattable, IConvertible
        where TE : IComparable, IFormattable, IConvertible
    {
        public IClientPeer Peer { get; }

        public int PeerId { get; }

        public void Dispose()
        {
            // TODO: Implement
        }
    }
}