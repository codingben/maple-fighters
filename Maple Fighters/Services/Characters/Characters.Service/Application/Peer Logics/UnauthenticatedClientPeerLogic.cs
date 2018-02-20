using Characters.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class UnauthenticatedClientPeerLogic : PeerLogicBase<CharactersServiceOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(CharactersServiceOperations.Authenticate,
                new AuthenticationOperationHandler(PeerWrapper.PeerId, OnAuthenticated));
        }

        private void OnAuthenticated(int dbUserId)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedClientPeerLogic(dbUserId));
        }
    }
}