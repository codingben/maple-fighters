using Authorization.Server.Common;
using Chat.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Chat.Application.PeerLogics
{
    internal class UnauthorizedClientPeerLogic : PeerLogicBase<ChatOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthorizationOperation();
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(ChatOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized()
        {
            PeerWrapper.SetPeerLogic(new AuthorizedClientPeerLogic());
        }

        private void OnNonAuthorized()
        {
            var ip = PeerWrapper.Peer.ConnectionInformation.Ip;
            var peerId = PeerWrapper.PeerId;

            LogUtils.Log(MessageBuilder.Trace($"An authorization for peer {ip} with id #{peerId} has been failed."));

            PeerWrapper.Peer.Disconnect();
        }
    }
}