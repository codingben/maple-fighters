using System;
using CommonCommunicationInterfaces;
using Game.Entities;

namespace Game.Entity.Components
{
    public class EventSender<TParameters> : EntityComponent
        where TParameters : struct
    {
        private readonly Action<byte, TParameters, MessageSendOptions> eventSenderAction;

        protected EventSender(IEntity entity, Action<byte, TParameters, MessageSendOptions> eventSenderAction) 
            : base(entity)
        {
            this.eventSenderAction = eventSenderAction;
        }

        public void SendEvent(byte code, TParameters parameters, MessageSendOptions messageSendOptions)
        {
            eventSenderAction.Invoke(code, parameters, messageSendOptions);
        }
    }
}