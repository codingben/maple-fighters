using System;
using CommonCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    internal interface IServiceConnectorProvider : IDisposable
    {
        IPeerDisconnectionNotifier PeerDisconnectionNotifier { get; }

        void Connect(PeerConnectionInformation connectionInformation);
        void SetNetworkTrafficState(NetworkTrafficState state);

        bool IsConnected();
    }
}