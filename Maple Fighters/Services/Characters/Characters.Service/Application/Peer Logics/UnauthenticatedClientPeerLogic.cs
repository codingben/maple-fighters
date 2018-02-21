using Characters.Client.Common;
using CharactersService.Application.PeerLogics.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class UnauthenticatedClientPeerLogic : PeerLogicBase<ClientOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ClientOperations.Authenticate,
                new AuthenticationOperationHandler(PeerWrapper.PeerId, OnAuthenticated));
        }

        private void OnAuthenticated(int dbUserId)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedClientPeerLogic(dbUserId));
        }
    }
}