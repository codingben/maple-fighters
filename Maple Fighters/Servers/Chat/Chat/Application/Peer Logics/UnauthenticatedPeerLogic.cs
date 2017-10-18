using Chat.Application.PeerLogic.Operations;
using Chat.Common;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Chat.Application.PeerLogics
{
    internal class UnauthenticatedPeerLogic : PeerLogicBase<ChatOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ChatOperations.Authenticate, 
                new AuthenticationOperationHandler(PeerWrapper.PeerId, OnAuthenticated));
        }

        private void OnAuthenticated()
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedPeerLogic());
        }
    }
}