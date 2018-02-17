using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using JsonConfig;
using ServerApplication.Common.Components;

namespace Server2.Common
{
    public class Server2Service : ServiceBase<ServerOperations, ServerEvents>, IServer2Service
    {
        public event Action<EmptyParameters> TestAction;

        protected override void OnConnected()
        {
            base.OnConnected();

            SetEventHandler(ServerEvents.Server1Event, TestAction);
        }

        protected override void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            base.OnDisconnected(disconnectReason, s);

            RemoveEventHandler(ServerEvents.Server1Event);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.Server2, MessageBuilder.Trace("Could not find an connection info for the Server2 server."));

            var ip = (string)Config.Global.Server2.IP;
            var port = (int)Config.Global.Server2.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}