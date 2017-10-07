using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
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

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(GameOperations.Authenticate, new AuthenticationOperationHandler(OnAuthenticated));
        }

        private void OnAuthenticated()
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedGamePeerLogic());
        }
    }
}