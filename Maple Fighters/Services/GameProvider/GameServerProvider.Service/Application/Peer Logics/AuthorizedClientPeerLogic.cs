using CommunicationHelper;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace GameServerProvider.Service.Application.PeerLogics
{
    using AuthorizationOperations = Authorization.Client.Common.AuthorizationOperations;

    internal class AuthorizedClientPeerLogic : PeerLogicBase<AuthorizationOperations, EmptyEventCode>
    {
        private readonly int userId;

        public AuthorizedClientPeerLogic(int userId)
        {
            this.userId = userId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForGameServersProviderOperation();
        }

        private void AddHandlerForGameServersProviderOperation()
        {
            OperationHandlerRegister.SetHandler(AuthorizationOperations.Authorize, new GameServersProviderOperationHandler());
        }

        private void AddComponents()
        {
            var userProfileTracker = Components.AddComponent(new UserProfileTracker(userId, ServerType.GameServerProvider, isUserProfileChanged: true));
            userProfileTracker.ChangeUserProfileProperties();
        }
    }
}