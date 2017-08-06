using System;
using System.Collections.Generic;
using Photon.SocketServer;
using Shared.Common.Communications;

namespace Shared.Servers.Common
{
    public class EventSender
    {
        private readonly Action<byte, Dictionary<byte, object>, SendParameters> eventAction;

        public EventSender(Action<byte, Dictionary<byte, object>, SendParameters> eventAction)
        {
            this.eventAction = eventAction;
        }

        public void SendEvent<T>(byte eventCode, T parameters, SendParameters sendParameters = new SendParameters())
            where T : struct, IParameters
        {
            eventAction.Invoke(eventCode, parameters.ToDictionary(), sendParameters);
        }
    }
}