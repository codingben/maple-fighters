using Authorization.Server.Common;
using Characters.Client.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application.PeerLogics
{
    internal class UnauthorizedClientPeerLogic : PeerLogicBase<ClientOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthorizationOperation();
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationRequestHandlerRegister.SetAsyncHandler(ClientOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized(int userId)
        {
            PeerWrapper.SetPeerLogic(new AuthorizedClientPeerLogic(userId));
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