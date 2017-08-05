using Game.Application.PeerLogic.Operations;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using Shared.Game.Common;
using Shared.Servers.Common;

namespace Game.Application.PeerLogic
{
    internal class ClientPeerLogic : ClientPeerLogicBase
    {
        public ClientPeerLogic(InitRequest initRequest) 
            : base(initRequest)
        {
            Logger.Log.Debug($"A new peer has connected - {initRequest.RemoteIP}:{initRequest.LocalPort}");

            SetOperationsHandlers();
        }

        private void SetOperationsHandlers()
        {
            SetOperationRequestHandler(GameOperations.Test, new TestOperation());
        }

        protected override void OnPeerDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Logger.Log.Debug($"A peer has been disconnected. Reason Code: {reasonCode}, Reason: {reasonDetail}");
        }
    }
}