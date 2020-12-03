using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components;
using UserProfile.Service.Application.Components.Interfaces;
using UserProfile.Service.Application.PeerLogic.Components;
using UserProfile.Service.Application.PeerLogic.Operations;

namespace UserProfile.Service.Application.PeerLogic
{
    internal class InboundServerPeerLogic : PeerLogicBase<UserProfileOperations, UserProfileEvents>
    {
        private IUsersContainer usersContainer;
        private IUserProfilePropertiesChangesNotifier userProfilePropertiesChangesNotifier;
        private readonly IDatabaseUserProfilePropertiesUpdater databaseUserProfilePropertiesUpdater;
        private int serverId = -1;

        public InboundServerPeerLogic()
        {
            databaseUserProfilePropertiesUpdater = ServerComponents.GetComponent<IDatabaseUserProfilePropertiesUpdater>().AssertNotNull();
        }

        protected override void OnInitialized()
        {
            SubscribeToDisconnectionNotifier();

            AddCommonComponents();
            AddComponents();

            AddHandlerForRegisterToUserProfileServiceOperation();
            AddHandlerForChangeUserProfilePropertiesOperation();
            AddHandlerForSubscribeToUserProfileOperation();
            AddHandlerForUnsubscribeFromUserProfileOperation();
        }

        private void AddComponents()
        {
            usersContainer = Components.AddComponent(new UsersContainer());
        }

        private void AddHandlerForChangeUserProfilePropertiesOperation()
        {
            userProfilePropertiesChangesNotifier = Components.AddComponent(new UserProfilePropertiesChangesNotifier());
            OperationHandlerRegister.SetHandler(UserProfileOperations.ChangeUserProfileProperties, 
                new ChangeUserProfilePropertiesOperationHandler(usersContainer, userProfilePropertiesChangesNotifier));
        }

        private void AddHandlerForRegisterToUserProfileServiceOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Register, new RegisterToUserProfileServiceOperationHandler(OnRegisterToUserProfileService));
        }

        private void AddHandlerForSubscribeToUserProfileOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Subscribe, new SubscribeToUserProfileOperationHandler());
        }

        private void AddHandlerForUnsubscribeFromUserProfileOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Unsubscribe, new UnsubscribeFromUserProfileOperationHandler());
        }

        private void SubscribeToDisconnectionNotifier()
        {
            ClientPeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            ClientPeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnRegisterToUserProfileService(int id)
        {
            serverId = id;

            var serverIdToPeerIdConverter = ServerComponents.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
            serverIdToPeerIdConverter.Add(serverId, ClientPeerWrapper.PeerId);
        }

        private void UnregisterFromUserProfileService()
        {
            var serverIdToPeerIdConverter = ServerComponents.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
            serverIdToPeerIdConverter.Remove(serverId);
        }

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            if (serverId != 0)
            {
                UnregisterFromUserProfileService();
            }

            HandleUnexpectedShutdown();
        }

        private void HandleUnexpectedShutdown()
        {
            var users = usersContainer.Get();
            foreach (var userId in users)
            {
                databaseUserProfilePropertiesUpdater.Update(userId, default(int), ServerType.Login, ConnectionStatus.Disconnected);

                var parameters = new UserProfilePropertiesChangedEventParameters(userId, ServerType.Login);
                userProfilePropertiesChangesNotifier.Notify(parameters);

                RemoveAuthorization(userId);
            }
        }

        private void RemoveAuthorization(int userId)
        {
            var authorizationServiceAPI = ServerComponents.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
            authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
        }
    }
}