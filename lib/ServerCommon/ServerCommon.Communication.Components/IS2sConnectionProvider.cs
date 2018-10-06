using System;
using CommonCommunicationInterfaces;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication.Components
{
    public interface IS2sConnectionProvider
    {
        void Connect(
            PeerConnectionInformation connectionDetails,
            Action<IOutboundServerPeer> connected,
            Action<DisconnectReason, string> disconnected);

        void Disconnect();
    }
}