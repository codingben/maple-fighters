using System;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace ServerCommon.Communication.PeerLogic
{
    public class EventHandler<TParameters> : IEventHandler<TParameters>
        where TParameters : struct, IParameters
    {
        private readonly Action<MessageData<TParameters>> eventAction;

        public EventHandler(Action<MessageData<TParameters>> eventAction)
        {
            this.eventAction = eventAction;
        }

        public bool Handle(MessageData<TParameters> eventData)
        {
            eventAction.Invoke(eventData);
            return true;
        }
    }
}