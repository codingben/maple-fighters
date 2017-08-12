using Game.Application.PeerLogic.Operations;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class ClientPeerLogic : PeerLogicBase<GameOperations, GameEvents>
    {
        public ClientPeerLogic(IClientPeer peer) 
            : base(peer)
        {
            // Left blank intentionally
        }

        protected override void Initialize()
        {
            SetOperationsHandlers();
        }

        private void SetOperationsHandlers()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Test, new TestOperation(EventSender));
        }
    }
}