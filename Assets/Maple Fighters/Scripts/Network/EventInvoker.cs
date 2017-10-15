using System;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Scripts.Services
{
    public class EventInvoker<TParameters> : IEventHandler<TParameters> 
        where TParameters : struct, IParameters
    {
        private readonly Func<MessageData<TParameters>, bool> unityEvent;

        public EventInvoker(Func<MessageData<TParameters>, bool> unityEvent)
        {
            this.unityEvent = unityEvent;
        }

        public bool Handle(MessageData<TParameters> eventData)
        {
            return unityEvent.Invoke(eventData);
        }
    }
}