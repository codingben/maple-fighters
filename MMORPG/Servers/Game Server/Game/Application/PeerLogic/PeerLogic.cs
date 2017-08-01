using System;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace Game.Application.PeerLogic
{
    internal class PeerLogic : ClientPeer
    {
        public PeerLogic(InitRequest initRequest)
            : base(initRequest)
        {

        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            throw new NotImplementedException();
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            throw new NotImplementedException();
        }
    }
}