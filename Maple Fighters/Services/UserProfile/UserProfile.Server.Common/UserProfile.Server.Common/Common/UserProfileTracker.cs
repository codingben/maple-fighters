using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components;

namespace UserProfile.Server.Common
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class UserProfileTracker : Component
    {
        private bool isManuallyDisconnected;
        private bool isUserProfileChanged;

        private readonly int userId;
        private readonly ServerType serverType;

        private IMinimalPeerGetter peerGetter;

        public UserProfileTracker(int userId, ServerType serverType)
        {
            this.userId = userId;
            this.serverType = serverType;
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();

            peerGetter = Components.GetComponent<IMinimalPeerGetter>().AssertNotNull();

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
            isUserProfileChanged = true;

            var userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            var parameters = new ChangeUserProfilePropertiesRequestParameters(userId, peerGetter.PeerId, serverType, ConnectionStatus.Connected);
            userProfileServiceAPI.ChangeUserProfileProperties(parameters);
        }

        private void SubscribeToUserProfile()
        {
            var userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            userProfileServiceAPI.SubscribeToUserProfile(userId);
        }

        private void UnsubscribeFromUserProfile()
        {
            var userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            userProfileServiceAPI.UnsubscribeFromUserProfile(userId);
        }

        private void AddUserIdToPeerIdConverter()
        {
            var userToPeerIdConverter = Server.Components.GetComponent<IUserIdToPeerIdConverter>().AssertNotNull();
            userToPeerIdConverter.Add(userId, peerGetter.PeerId);
        }

        private void RemoveUserIdToPeerIdFromConverter()
        {
            var userToPeerIdConverter = Server.Components.GetComponent<IUserIdToPeerIdConverter>().AssertNotNull();
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
            if (parameters.ConnectionStatus == ConnectionStatus.Connected && parameters.ServerType != serverType)
            {
                isManuallyDisconnected = true;
                peerGetter.Peer.Disconnect();
            }
        }

        private void SubscribeToDisconnectionNotifier()
        {
            peerGetter.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            peerGetter.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            if (!isManuallyDisconnected && !isUserProfileChanged)
            {
                OnClientDisconnected();
            }

            RemoveUserIdToPeerIdFromConverter();
        }

        private void OnClientDisconnected()
        {
            var userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            var parameters = new ChangeUserProfilePropertiesRequestParameters(userId, ConnectionStatus.Disconnected);
            userProfileServiceAPI.ChangeUserProfileProperties(parameters);

            var authorizationServiceAPI = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
            authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
        }
    }
}