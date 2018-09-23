using System;
using Common.ComponentModel;
using CommonCommunicationInterfaces;
using ServerCommon.Configuration;
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
            this.eventSender =
                new EventSender<TEventCode>(
                    eventSender,
                    log: ServerSettings.Peer.LogEvents);
        }

        public void Send<TEnumCode, TParameters>(
            TEnumCode code,
            TParameters parameters,
            MessageSendOptions messageSendOptions)
            where TEnumCode : IComparable, IFormattable, IConvertible
            where TParameters : struct, IParameters
        {
            if (minimalPeer.IsConnected)
            {
                var codeByte = Convert.ToByte(code);
                var eventCode = (TEventCode)Enum.Parse(
                    enumType: typeof(TEventCode),
                    value: codeByte.ToString(),
                    ignoreCase: true);

                eventSender.Send(eventCode, parameters, messageSendOptions);
            }
        }
    }
}