using System;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class EventSenderProvider<TEventCode> : ComponentBase, IEventSenderProvider
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private readonly IMinimalPeer minimalPeer;
        private readonly IEventSender<TEventCode> eventSender;

        public EventSenderProvider(
            IMinimalPeer minimalPeer,
            IEventSender eventSender)
        {
            this.minimalPeer = minimalPeer;
            this.eventSender = new EventSender<TEventCode>(
                eventSender, log: true); // TODO: Get log from the configuration
        }

        public void Send<TParameters, TEnumCode>(
            TEnumCode code, TParameters parameters, MessageSendOptions options)
            where TParameters : struct, IParameters
            where TEnumCode : IComparable, IFormattable, IConvertible
        {
            if (minimalPeer.IsConnected)
            {
                var codeByte = Convert.ToByte(code);
                var eventCode = (TEventCode)Enum.Parse(
                    typeof(TEventCode), codeByte.ToString(), true);

                eventSender.Send(eventCode, parameters, options);
            }
        }
    }
}