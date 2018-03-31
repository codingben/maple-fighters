using System;
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

        private readonly int userId;
        private readonly ServerType serverType;
        private readonly bool isUserProfileChanged;

        private IMinimalPeerGetter peerGetter;

        public UserProfileTracker(int userId, ServerType serverType, bool isUserProfileChanged = false)
        {
            this.userId = userId;
            this.serverType = serverType;
            this.isUserProfileChanged = isUserProfileChanged;
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
            var isServerChanged = parameters.ServerType != serverType;
            if (isServerChanged)
            {
                DisconnectManually();
            }

            void DisconnectManually()
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
            ChangeUserToOfflineStatus();
        }

        private void ChangeUserToOfflineStatus()
        {
            if (isManuallyDisconnected || !isUserProfileChanged)
            {
                return;
            }

            var userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            var parameters = new ChangeUserProfilePropertiesRequestParameters(userId, ConnectionStatus.Disconnected);
            userProfileServiceAPI.ChangeUserProfileProperties(parameters);

            RemoveAuthorizationForUser();
        }

        private void RemoveAuthorizationForUser()
        {
            var authorizationServiceAPI = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
            authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
        }
    }
}