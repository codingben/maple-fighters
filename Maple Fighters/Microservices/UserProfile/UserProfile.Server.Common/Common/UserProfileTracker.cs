using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;

namespace UserProfile.Server.Common
{
    public class UserProfileTracker : Component
    {
        private bool isManuallyDisconnected;

        private readonly int userId;
        private readonly ServerType serverType;
        private readonly bool isUserProfileChanged;

        private IClientPeerProvider clientPeerProvider;

        public UserProfileTracker(int userId, ServerType serverType, bool isUserProfileChanged = false)
        {
            this.userId = userId;
            this.serverType = serverType;
            this.isUserProfileChanged = isUserProfileChanged;
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();

            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();

            AddUserIdToPeerIdConverter();

            SubscribeToDisconnectionNotifier();
            SubscribeToUserProfilePropertiesChanged();
            SubscribeToUserProfile();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveUserIdToPeerIdFromConverter();

            UnsubscribeFromDisconnectionNotifier();
            UnsubscribeFromUserProfilePropertiesChanged();
            UnsubscribeFromUserProfile();
        }

        public void ChangeUserProfileProperties()
        {
            var userProfileServiceAPI = ServerComponents.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            var parameters = new ChangeUserProfilePropertiesRequestParameters(userId, clientPeerProvider.PeerId, serverType, ConnectionStatus.Connected);
            userProfileServiceAPI.ChangeUserProfileProperties(parameters);
        }

        private void SubscribeToUserProfile()
        {
            var userProfileServiceAPI = ServerComponents.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            userProfileServiceAPI.SubscribeToUserProfile(userId);
        }

        private void UnsubscribeFromUserProfile()
        {
            var userProfileServiceAPI = ServerComponents.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            userProfileServiceAPI.UnsubscribeFromUserProfile(userId);
        }

        private void AddUserIdToPeerIdConverter()
        {
            var userToPeerIdConverter = ServerComponents.GetComponent<IUserIdToPeerIdConverter>().AssertNotNull();
            userToPeerIdConverter.Add(userId, clientPeerProvider.PeerId);
        }

        private void RemoveUserIdToPeerIdFromConverter()
        {
            var userToPeerIdConverter = ServerComponents.GetComponent<IUserIdToPeerIdConverter>().AssertNotNull();
            userToPeerIdConverter.Remove(userId);
        }

        private void SubscribeToUserProfilePropertiesChanged()
        {
            var userProfilePropertiesChangesEventInvoker = Components.AddComponent(new UserProfilePropertiesChangesEventInvoker());
            userProfilePropertiesChangesEventInvoker.UserProfilePropertiesChanged += OnUserProfilePropertiesChanged;
        }

        private void UnsubscribeFromUserProfilePropertiesChanged()
        {
            var userProfilePropertiesChangesEventInvoker = Components.GetComponent<IUserProfilePropertiesChangesEventInvoker>();
            userProfilePropertiesChangesEventInvoker.UserProfilePropertiesChanged -= OnUserProfilePropertiesChanged;
        }

        private void OnUserProfilePropertiesChanged(UserProfilePropertiesChangedEventParameters parameters)
        {
            var isServerChanged = parameters.ServerType != serverType;
            if (isServerChanged)
            {
                DisconnectManually();
            }

            void DisconnectManually()
            {
                isManuallyDisconnected = true;
                clientPeerProvider.Peer.Disconnect();
            }
        }

        private void SubscribeToDisconnectionNotifier()
        {
            clientPeerProvider.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            clientPeerProvider.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();
            ChangeUserToOfflineStatus();
        }

        private void ChangeUserToOfflineStatus()
        {
            if (isManuallyDisconnected || !isUserProfileChanged)
            {
                return;
            }

            var userProfileServiceAPI = ServerComponents.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            var parameters = new ChangeUserProfilePropertiesRequestParameters(userId, ConnectionStatus.Disconnected);
            userProfileServiceAPI.ChangeUserProfileProperties(parameters);

            RemoveAuthorizationForUser();
        }

        private void RemoveAuthorizationForUser()
        {
            var authorizationServiceAPI = ServerComponents.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
            authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
        }
    }
}