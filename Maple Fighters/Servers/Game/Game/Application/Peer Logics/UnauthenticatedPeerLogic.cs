using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using ServerCommunicationInterfaces;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogics
{
    internal class UnauthenticatedPeerLogic : PeerLogicBase<GameOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Authenticate, 
                new AuthenticationOperationHandler(PeerWrapper.PeerId, OnAuthenticated));
        }

        private void OnAuthenticated(int dbUserId)
        {
            PeerWrapper.SetPeerLogic(new CharactersSelectionPeerLogic(dbUserId));
        }
    }
}