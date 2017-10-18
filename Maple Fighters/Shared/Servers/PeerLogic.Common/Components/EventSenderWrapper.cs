using CommonCommunicationInterfaces;
using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace PeerLogic.Common.Components
{
    public class EventSenderWrapper : Component<IPeerEntity>
    {
        private readonly IEventSender eventSender;

        public EventSenderWrapper(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void Send<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            eventSender.Send(new MessageData<TParameters>(code, parameters), messageSendOptions);
        }
    }
}