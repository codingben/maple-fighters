using Authorization.Server.Common;
using Character.Client.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Character.Service.Application.PeerLogics
{
    internal class UnauthorizedClientPeerLogic : PeerLogicBase<CharacterOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthorizationOperation();
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(CharacterOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
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