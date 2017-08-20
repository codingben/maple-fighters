using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace Shared.ServerApplication.Common.Peer
{
    public interface IClientPeerWrapper<out T> : IDisposable
        where T : IClientPeer
    {
        T Peer { get; }
        int PeerId { get; }

        event Action<DisconnectReason, string> Disconnected;

        void SendEvent<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters;
    }
}