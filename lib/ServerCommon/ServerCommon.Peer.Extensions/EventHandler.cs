using System;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace ServerCommon.Peer.Extensions
{
    public class EventHandler<TP> : IEventHandler<TP>
        where TP : IParameters, new()
    {
        private readonly Action<MessageData<TP>> action;

        public EventHandler(Action<MessageData<TP>> action)
        {
            this.action = action;
        }

        public bool Handle(MessageData<TP> eventData)
        {
            if (action != null)
            {
                action.Invoke(eventData);
                return true;
            }

            return false;
        }
    }
}