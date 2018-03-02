using Authorization.Server.Common;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using Game.Common;

namespace Game.Application.PeerLogics
{
    internal class UnauthorizedClientPeerLogic : PeerLogicBase<GameOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthorizationOperation();
        }

        private void AddHandlerForAuthorizationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(GameOperations.Authorize, new AuthorizationOperationHandler(OnAuthorized, OnNonAuthorized));
        }

        private void OnAuthorized(int userId)
        {
            PeerWrapper.SetPeerLogic(new CharacterSelectionPeerLogic(userId));
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