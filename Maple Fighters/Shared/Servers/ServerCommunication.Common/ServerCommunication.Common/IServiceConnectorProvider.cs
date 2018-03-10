using System;
using CommonCommunicationInterfaces;

namespace ServerCommunication.Common
{
    internal interface IServiceConnectorProvider : IDisposable
    {
        IPeerDisconnectionNotifier PeerDisconnectionNotifier { get; }

        void Connect(PeerConnectionInformation connectionInformation);
        void SetNetworkTrafficState(NetworkTrafficState state);

        bool IsConnected();
    }
}