using CommunicationHelper;
using GameServerProvider.Client.Common;
using GameServerProvider.Service.Application.PeerLogic.Operations;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using UserProfile.Server.Common;

namespace GameServerProvider.Service.Application.PeerLogics
{
    internal class AuthorizedClientPeerLogic : PeerLogicBase<GameServerProviderOperations, EmptyEventCode>
    {
        private readonly int userId;

        public AuthorizedClientPeerLogic(int userId)
        {
            this.userId = userId;
        }

        protected override void OnInitialized()
        {
            AddCommonComponents();
            AddComponents();

            AddHandlerForGameServersProviderOperation();
        }

        private void AddHandlerForGameServersProviderOperation()
        {
            OperationHandlerRegister.SetHandler(GameServerProviderOperations.ProvideGameServers, new GameServersProviderOperationHandler());
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout());

            var userProfileTracker = Components.AddComponent(new UserProfileTracker(userId, ServerType.GameServerProvider, isUserProfileChanged: true));
            userProfileTracker.ChangeUserProfileProperties();
        }
    }
}