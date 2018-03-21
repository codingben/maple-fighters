using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components;
using ServerApplication.Common.Components;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class UserProfilePropertiesChangesNotifier : Component, IUserProfilePropertiesChangesNotifier
    {
        private IMinimalPeerGetter peerGetter;
        private IPeerContainer peerContainer;
        private IServerIdToPeerIdConverter serverIdToPeerIdConverter;
        private IUserIdToServerIdConverter userIdToServerIdConverter;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerGetter = Components.GetComponent<IMinimalPeerGetter>().AssertNotNull();

            peerContainer = Server.Components.GetComponent<IPeerContainer>().AssertNotNull();
            serverIdToPeerIdConverter = Server.Components.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
            userIdToServerIdConverter = Server.Components.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
        }

        public void Notify(UserProfilePropertiesChangedEventParameters parameters)
        {
            var userId = parameters.UserId;
            var serverIds = userIdToServerIdConverter.Get(userId);
            if (serverIds == null)
            {
                LogUtils.Log($"Could not find server ids for user with id {userId}");
                return;
            }

            foreach (var serverId in serverIds)
            {
                var peerId = serverIdToPeerIdConverter.Get(serverId);
                if (!peerId.HasValue)
                {
                    continue;
                }

                if (peerId.Value == peerGetter.PeerId)
                {
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(peerId.Value);
                if (peerWrapper == null)
                {
                    LogUtils.Log($"Could not find a peer wrapper of server with id {serverId}");
                    continue;
                }

                var eventSenderWrapper = peerWrapper.PeerLogic.Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSenderWrapper.Send((byte)UserProfileEvents.UserProfilePropertiesChanged, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}