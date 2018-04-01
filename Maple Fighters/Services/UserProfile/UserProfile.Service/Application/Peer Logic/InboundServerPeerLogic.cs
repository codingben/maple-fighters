using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using PeerLogic.Common;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components;
using UserProfile.Service.Application.PeerLogic.Components;
using UserProfile.Service.Application.PeerLogic.Operations;

namespace UserProfile.Service.Application.PeerLogic
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class InboundServerPeerLogic : PeerLogicBase<UserProfileOperations, UserProfileEvents>
    {
        private IUsersContainer usersContainer;
        private IUserProfilePropertiesChangesNotifier userProfilePropertiesChangesNotifier;
        private readonly IDatabaseUserProfilePropertiesUpdater databaseUserProfilePropertiesUpdater;

        public InboundServerPeerLogic()
        {
            databaseUserProfilePropertiesUpdater = Server.Components.GetComponent<IDatabaseUserProfilePropertiesUpdater>().AssertNotNull();
        }

        public override void Initialize(IClientPeerWrapper peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectionNotifier();

            AddCommonComponents();
            AddComponents();

            AddHandlerForRegisterToUserProfileServiceOperation();
            AddHandlerForUnregisterFromUserProfileServiceOperation();
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
            var peerId = ClientPeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(UserProfileOperations.Register, new RegisterToUserProfileServiceOperationHandler(peerId));
        }

        private void AddHandlerForUnregisterFromUserProfileServiceOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Unregister, new UnregisterFromUserProfileServiceOperationHandler());
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

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();
            HandleUnexpectedShutdown();
        }

        private void HandleUnexpectedShutdown()
        {
            foreach (var userId in usersContainer.Get())
            {
                databaseUserProfilePropertiesUpdater.Update(userId, default(int), ServerType.Login, ConnectionStatus.Disconnected);

                var parameters = new UserProfilePropertiesChangedEventParameters(userId, ServerType.Login);
                userProfilePropertiesChangesNotifier.Notify(parameters);

                RemoveAuthorization(userId);
            }
        }

        private void RemoveAuthorization(int userId)
        {
            var authorizationServiceAPI = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
            authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
        }
    }
}