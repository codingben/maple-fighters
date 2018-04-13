using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using JsonConfig;
using ServerCommunication.Common;

namespace Server2.Common
{
    public class Server2Service : ServiceBase, IServer2ServiceAPI
    {
        public event Action<EmptyParameters> TestAction;

        private IOutboundServerPeerLogic outboundServerPeerLogic;

        protected override void OnConnectionEstablished()
        {
            base.OnConnectionEstablished();

            outboundServerPeerLogic = OutboundServerPeer.CreateOutboundServerPeerLogic<ServerOperations, ServerEvents>();
            outboundServerPeerLogic.SetEventHandler((byte)ServerEvents.Server1Event, TestAction);
        }

        protected override void OnConnectionClosed(DisconnectReason disconnectReason)
        {
            base.OnConnectionClosed(disconnectReason);

            outboundServerPeerLogic.RemoveEventHandler((byte)ServerEvents.Server1Event);
        }

        public Task<Server1OperationResponseParameters> Server1Operation(IYield yield, Server1OperationRequestParameters parameters)
        {
            return outboundServerPeerLogic.SendOperation<Server1OperationRequestParameters, Server1OperationResponseParameters>
                (yield, (byte)ServerOperations.Server1Operation, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.Server2, MessageBuilder.Trace("Could not find a connection info for the Server2 server."));

            var ip = (string)Config.Global.Server2.IP;
            var port = (int)Config.Global.Server2.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}