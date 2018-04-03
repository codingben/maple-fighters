using Authorization.Server.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;

namespace Game.Application.PeerLogics
{
    using AuthorizationOperations = Authorization.Client.Common.AuthorizationOperations;

    internal class UnauthorizedClientPeerLogic : PeerLogicBase<AuthorizationOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthorizationOperation();
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(AuthorizationOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized(int userId)
        {
            ClientPeerWrapper.SetPeerLogic(new CharacterSelectionPeerLogic(userId));
        }

        private void OnNonAuthorized()
        {
            var ip = ClientPeerWrapper.Peer.ConnectionInformation.Ip;
            var peerId = ClientPeerWrapper.PeerId;

            LogUtils.Log(MessageBuilder.Trace($"An authorization for peer {ip} with id #{peerId} has been failed."));

            ClientPeerWrapper.Peer.Disconnect();
        }
    }
}