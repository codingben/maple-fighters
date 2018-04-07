using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common;
using PeerLogic.Common.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.Components
{
    internal class UserProfilePropertiesChangesNotifier : Component, IUserProfilePropertiesChangesNotifier
    {
        private IClientPeerProvider clientPeerProvider;
        private IPeerContainer peerContainer;
        private IServerIdToPeerIdConverter serverIdToPeerIdConverter;
        private IUserIdToServerIdConverter userIdToServerIdConverter;

        protected override void OnAwake()
        {
            base.OnAwake();

            clientPeerProvider = Components.GetComponent<IClientPeerProvider>().AssertNotNull();

            peerContainer = ServerComponents.GetComponent<IPeerContainer>().AssertNotNull();
            serverIdToPeerIdConverter = ServerComponents.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
            userIdToServerIdConverter = ServerComponents.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
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

                if (peerId.Value == clientPeerProvider.PeerId)
                {
                    continue;
                }

                var peerWrapper = peerContainer.GetPeerWrapper(peerId.Value);
                if (peerWrapper == null)
                {
                    LogUtils.Log($"Could not find a peer wrapper of server with id {serverId}");
                    continue;
                }

                RaiseUserProfilePropertiesChanged(peerWrapper);
            }

            void RaiseUserProfilePropertiesChanged(IClientPeerWrapper peerWrapper)
            {
                var eventSenderWrapper = peerWrapper.PeerLogic.Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
                eventSenderWrapper.Send((byte)UserProfileEvents.UserProfilePropertiesChanged, parameters, MessageSendOptions.DefaultReliable());
            }
        }
    }
}