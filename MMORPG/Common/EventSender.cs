using System;
using System.Collections.Generic;
using Photon.SocketServer;

namespace Shared.Servers.Common
{
    public static class EventSender
    {
        public static Action<byte, Dictionary<byte, object>, SendParameters> EventAction { get; set; }

        public static void SendEvent<T>(byte eventCode, T parameters, SendParameters sendParameters = new SendParameters())
            where T : struct, IParameters
        {
            EventAction.Invoke(eventCode, parameters.SerializeParameters(), sendParameters);
        }
    }
}