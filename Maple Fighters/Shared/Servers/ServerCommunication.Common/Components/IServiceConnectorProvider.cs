using System;
using CommonCommunicationInterfaces;

namespace ServerCommunication.Common
{
    internal interface IServiceConnectionProvider : IDisposable
    {
        IPeerDisconnectionNotifier PeerDisconnectionNotifier { get; }

        void Connect(PeerConnectionInformation connectionInformation);
        void Disconnect();

        void SetNetworkTrafficState(NetworkTrafficState state);

        bool IsConnected();
    }
}