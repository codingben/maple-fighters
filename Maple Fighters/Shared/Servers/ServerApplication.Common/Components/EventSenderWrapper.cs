using CommonCommunicationInterfaces;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

namespace ServerApplication.Common.Components
{
    public class EventSenderWrapper : Component<IPeerEntity>
    {
        private readonly IEventSender eventSender;

        public EventSenderWrapper(IEventSender eventSender)
        {
            this.eventSender = eventSender;
        }

        public void SendEvent<TParameters>(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
            where TParameters : struct, IParameters
        {
            eventSender.Send(new MessageData<TParameters>(code, parameters), messageSendOptions);
        }
    }
}