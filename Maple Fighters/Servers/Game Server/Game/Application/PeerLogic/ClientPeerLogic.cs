using Game.Application.PeerLogic.Operations;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;
using Shared.Game.Common;
using Shared.Servers.Common;

namespace Game.Application.PeerLogic
{
    internal class ClientPeerLogic : ClientPeerLogicBase<GameOperations, GameEvents>
    {
        public ClientPeerLogic(InitRequest initRequest) 
            : base(initRequest)
        {
            Logger.Log.Debug($"A new peer has connected - {initRequest.RemoteIP}:{initRequest.LocalPort}");

            Initialize();
        }

        private void Initialize()
        {
            ActivateLogs(operationRequets: true, operationResponses: true, events: true);
            SetOperationsHandlers();
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }

        protected override void OnDisconnected(DisconnectReason reasonCode, string reasonDetail)
        {
            Logger.Log.Debug($"A peer has been disconnected. Reason Code -> {reasonCode}, Reason -> {reasonDetail}");
        }
    }
}