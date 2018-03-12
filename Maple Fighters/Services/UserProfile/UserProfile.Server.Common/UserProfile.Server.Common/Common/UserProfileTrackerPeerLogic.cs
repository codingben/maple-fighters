using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class UserProfileTrackerPeerLogic : PeerLogicBase<EmptyOperationCode, EmptyEventCode>
    {
        private bool isManuallyDisconnected;

        private readonly int userId;
        private readonly ServerType currentServerType;

        private readonly IUserProfileServiceAPI userProfileServiceAPI;
        private readonly IAuthorizationServiceAPI authorizationServiceAPI;

        public UserProfileTrackerPeerLogic(int userId, ServerType currentServerType)
        {
            this.userId = userId;
            this.currentServerType = currentServerType;

            userProfileServiceAPI = Server.Components.GetComponent<IUserProfileServiceAPI>().AssertNotNull();
            userProfileServiceAPI.UserProfilePropertiesChanged += OnUserProfilePropertiesChanged;
            authorizationServiceAPI = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            SubscribeToDisconnectionNotifier();
        }

        public override void Dispose()
        {
            base.Dispose();

            userProfileServiceAPI.UserProfilePropertiesChanged -= OnUserProfilePropertiesChanged;
        }

        private void OnUserProfilePropertiesChanged(UserProfilePropertiesChangedEventParameters parameters)
        {
            if (parameters.ConnectionStatus != ConnectionStatus.Connected || parameters.ServerType == currentServerType)
            {
                return;
            }

            isManuallyDisconnected = true;
            PeerWrapper.Peer.Disconnect();
        }

        private void SubscribeToDisconnectionNotifier()
        {
            PeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            PeerWrapper.Peer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            if (!isManuallyDisconnected)
            {
                authorizationServiceAPI.RemoveAuthorization(new RemoveAuthorizationRequestParameters(userId));
            }
        }
    }
}