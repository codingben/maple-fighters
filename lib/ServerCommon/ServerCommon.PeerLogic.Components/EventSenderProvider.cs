using System;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using ServerCommon.Configuration;
using ServerCommunicationHelper;
using ServerCommunicationInterfaces;

namespace ServerCommon.PeerLogic.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class EventSenderProvider<TEventCode> : ComponentBase,
                                                   IEventSenderProvider
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private readonly IMinimalPeer minimalPeer;
        private readonly IEventSender<TEventCode> eventSender;

        public EventSenderProvider(
            IMinimalPeer minimalPeer,
            IEventSender eventSender)
        {
            this.minimalPeer = minimalPeer;
            this.eventSender =
                new EventSender<TEventCode>(
                    eventSender,
                    ServerSettings.InboundPeer.LogEvents);
        }

        public void Send<TEnumCode, TParameters>(
            TEnumCode code,
            TParameters parameters,
            MessageSendOptions sendOptions)
            where TEnumCode : IComparable, IFormattable, IConvertible
            where TParameters : struct, IParameters
        {
            if (minimalPeer.IsConnected)
            {
                var codeByte = Convert.ToByte(code);
                var eventCode =
                    (TEventCode)Enum.Parse(
                        typeof(TEventCode),
                        codeByte.ToString(),
                        true);

                eventSender.Send(eventCode, parameters, sendOptions);
            }
        }
    }
}