using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using JsonConfig;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;

namespace Server2.Common
{
    public class Server2Service : ServiceBase<ServerOperations, ServerEvents>, IServer2ServiceAPI
    {
        public event Action<EmptyParameters> TestAction;

        protected override void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            base.OnConnected(outboundServerPeer);

            OutboundServerPeerLogic.SetEventHandler((byte)ServerEvents.Server1Event, TestAction);
        }

        protected override void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            base.OnDisconnected(disconnectReason, s);

            OutboundServerPeerLogic.RemoveEventHandler((byte)ServerEvents.Server1Event);
        }

        public Task<Server1OperationResponseParameters> Server1Operation(IYield yield, Server1OperationRequestParameters parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<Server1OperationRequestParameters, Server1OperationResponseParameters>
                (yield, (byte)ServerOperations.Server1Operation, parameters);
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