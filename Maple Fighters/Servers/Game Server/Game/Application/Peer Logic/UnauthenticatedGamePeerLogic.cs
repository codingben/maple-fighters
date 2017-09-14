using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using ServerCommunicationInterfaces;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic
{
    internal class UnauthenticatedGamePeerLogic : PeerLogicBase<GameOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForEnterWorldOperation();
        }

        private void AddHandlerForEnterWorldOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.EnterWorld, new EnterWorldOperationHandler(PeerWrapper.PeerId, OnAuthenticated));
        }

        private void OnAuthenticated(IGameObject gameObject)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedGamePeerLogic(gameObject));
        }
    }
}