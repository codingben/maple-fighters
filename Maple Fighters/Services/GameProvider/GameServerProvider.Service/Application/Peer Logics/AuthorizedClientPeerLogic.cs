using CommunicationHelper;
using GameServerProvider.Client.Common;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace GameServerProvider.Service.Application.PeerLogics
{
    internal class AuthorizedClientPeerLogic : PeerLogicBase<ClientOperations, EmptyEventCode>
    {
        private readonly int userId;

        public AuthorizedClientPeerLogic(int userId)
        {
            this.userId = userId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForGameServersProviderOperation();
        }

        private void AddHandlerForGameServersProviderOperation()
        {
            OperationHandlerRegister.SetHandler(ClientOperations.ProvideGameServers, new GameServersProviderOperationHandler(OnGameServersSent));
        }

        private void OnGameServersSent()
        {
            PeerWrapper.SetPeerLogic(new UserProfileTrackerPeerLogic(userId, ServerType.GameServerProvider));
        }
    }
}