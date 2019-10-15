using System;
using CommonCommunicationInterfaces;

namespace Network.Scripts
{
    public class DummyPeer : IServerPeer, IPeerDisconnectionNotifier, IOperationRequestSender, IOperationResponseNotifier, IEventNotifier
    {
        public NetworkTrafficState NetworkTrafficState { get; set; }

        public IPeerDisconnectionNotifier PeerDisconnectionNotifier => this;

        public IOperationRequestSender OperationRequestSender => this;

        public IOperationResponseNotifier OperationResponseNotifier => this;

        public IEventNotifier EventNotifier => this;

        public bool IsConnected { get; } = true;

        public event Action<DisconnectReason, string> Disconnected;

        public event Action<RawMessageResponseData, short> OperationResponded;

        public event Action<RawMessageData> EventRecieved;

        public void Disconnect()
        {
            // Left blank intentionally
        }

        public short Send<TParam>(MessageData<TParam> data, MessageSendOptions sendOptions)
            where TParam : IParameters, new()
        {
            return default;
        }
    }
}