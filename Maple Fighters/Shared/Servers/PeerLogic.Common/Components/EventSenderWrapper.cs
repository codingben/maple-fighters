using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public class EventSenderWrapper : Component, IEventSenderWrapper
    {
        private readonly IEventSender eventSender;
        private IMinimalPeerGetter peerGetter;

        public EventSenderWrapper(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            peerGetter = Components.GetComponent<IMinimalPeerGetter>().AssertNotNull();
        }

        public void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            if (peerGetter.Peer.IsConnected)
            {
                eventSender.Send(new MessageData<TParameters>(code, parameters), messageSendOptions);
            }
        }
    }
}