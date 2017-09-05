using CommonCommunicationInterfaces;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class EventSender : EntityComponent
    {
        private readonly IEventSender eventSender;

        public EventSender(IEventSender eventSender)
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