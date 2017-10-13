using Chat.Application.PeerLogic.Operations;
using Chat.Common;
using CommunicationHelper;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

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
            OperationRequestHandlerRegister.SetHandler(ChatOperations.Authenticate, new AuthenticationOperationHandler(OnAuthenticated, OnUnauthenticated));
        }

        private void OnAuthenticated()
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedPeerLogic());
        }

        private void OnUnauthenticated()
        {
            PeerWrapper.Peer.Disconnect();
        }
    }
}