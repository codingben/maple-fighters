using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public class EventSenderWrapper : Component, IEventSenderWrapper
    {
        private readonly IEventSender eventSender;
        private IClientPeerProvider clientPeerProvider;

        public EventSenderWrapper(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();
        }

        public void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (clientPeerProvider.Peer.IsConnected)
            {
                eventSender.Send(new MessageData<TParameters>(code, parameters), messageSendOptions);
            }
        }
    }
}